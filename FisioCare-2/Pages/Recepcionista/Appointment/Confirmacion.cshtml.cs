using FisioCare_2.Models;
using FisioCare_2.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;

namespace FisioCare_2.Pages.Recepcionista.Appointment
{
    public class ConfirmacionModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ConfirmacionModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty(SupportsGet = true)]
        public string idFisioterapeuta { get; set; } = string.Empty;
        [BindProperty(SupportsGet = true)]
        public string IDPaciente { get; set; } = string.Empty;
        [BindProperty]
        public TimeSpan HoraSeleccionada { get; set; }

        [BindProperty(SupportsGet = true)]
        public int ServicioId { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime Fecha { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Hora { get; set; } = string.Empty;
        // Propiedades para mostrar en vista
        public string NombreFisioterapeuta { get; set; } = string.Empty;
        public string NombrePaciente { get; set; } = string.Empty;
        public string NombreServicio { get; set; } = string.Empty;
        public string FechaFormateada { get; set; } = string.Empty;
        public string HoraFormateada { get; set; } = string.Empty;
        public async Task<IActionResult> OnGetAsync()
        {
            // Validación rápida
            if (string.IsNullOrEmpty(idFisioterapeuta) || ServicioId == 0 || Fecha == default || string.IsNullOrEmpty(Hora))
            {
                return RedirectToPage("/Error");
            }
            Console.WriteLine("IDPaciente" + IDPaciente + "idFisioterapeuta" + idFisioterapeuta + "ServicioId" + ServicioId + "Fecha'" + Fecha + "'Hora'" + Hora);
            var fisioterapeuta = await _context.Users.FindAsync(idFisioterapeuta);
            var servicio = await _context.Servicio.FindAsync(ServicioId);
            var paciente = await _context.Users.FindAsync(IDPaciente);

            if (fisioterapeuta == null || servicio == null)
            {
                return RedirectToPage("/Error");
            }

            NombreFisioterapeuta = $"{fisioterapeuta.FirstName} {fisioterapeuta.LastName}";
            NombreServicio = servicio.Nombre;
            NombrePaciente = $"{paciente.FirstName} {paciente.LastName}";

            FechaFormateada = Fecha.ToString("dddd d 'de' MMMM 'de' yyyy", new CultureInfo("es-ES"));
            if (TimeSpan.TryParse(Hora, out TimeSpan horaParsed))
            {
                HoraFormateada = DateTime.Today.Add(horaParsed).ToString("hh:mm tt", new CultureInfo("es-ES"));
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Validaciones básicas
            if (string.IsNullOrEmpty(idFisioterapeuta) || string.IsNullOrEmpty(IDPaciente) || ServicioId == 0 || Fecha == default || string.IsNullOrEmpty(Hora))
            {
                return RedirectToPage("/Error");
            }

            // Obtener el paciente usando el ID recibido
            var paciente = await _context.Users.FindAsync(IDPaciente);
            if (paciente == null)
            {
                return RedirectToPage("/Error");
            }

            // Obtener el servicio
            var servicio = await _context.Servicio.FindAsync(ServicioId);
            if (servicio == null)
            {
                return RedirectToPage("/Error");
            }

            // Validar créditos suficientes
            if (paciente.CreditosDisponibles < servicio.CreditosNecesarios)
            {
                TempData["Error-Creditos-Recepcionista"] = "El paciente no tiene créditos suficientes para agendar esta cita.";
                return RedirectToPage("/Recepcionista/Index");
            }

            // Convertir hora string a TimeSpan y combinar con fecha
            if (!TimeSpan.TryParse(Hora, out TimeSpan horaParsed))
            {
                TempData["Error-Hora-Recepcionista"] = "La hora seleccionada no es válida.";
                return RedirectToPage("/Error");
            }

            DateTime horaInicio = Fecha.Date.Add(horaParsed);

            // Crear la cita
            var nuevaCita = new Cita
            {
                UsuarioId = paciente.Id,
                FisioterapeutaId = idFisioterapeuta,
                ServicioId = ServicioId,
                HoraInicio = horaInicio,
                Estado = "Pendiente",
                CreatedAt = DateTime.Now
            };

            _context.Cita.Add(nuevaCita);

            // Descontar créditos del paciente
            paciente.CreditosDisponibles -= servicio.CreditosNecesarios;
            _context.Users.Update(paciente);

            await _context.SaveChangesAsync();

            TempData["Success-Cita-Recepcionista"] = "¡Cita agendada exitosamente!";
            return RedirectToPage("/Recepcionista/Index");
        }
    }
}
