using FisioCare_2.Models;
using FisioCare_2.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;

namespace FisioCare_2.Pages.Fisioterapeuta.Appointment
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

        // Parámetros recibidos desde los pasos anteriores
        [BindProperty(SupportsGet = true)]
        public string FisioterapeutaId { get; set; } = string.Empty;
        [BindProperty(SupportsGet = true)]
        public string idPaciente { get; set; } = string.Empty;
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
            if (string.IsNullOrEmpty(FisioterapeutaId) || ServicioId == 0 || Fecha == default || string.IsNullOrEmpty(Hora))
            {
                return RedirectToPage("/Error");
            }

            var fisioterapeuta = await _context.Users.FindAsync(FisioterapeutaId);
            var servicio = await _context.Servicio.FindAsync(ServicioId);
            var paciente = await _context.Users.FindAsync(idPaciente);

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
            if (string.IsNullOrEmpty(FisioterapeutaId) || string.IsNullOrEmpty(idPaciente) || ServicioId == 0 || Fecha == default || string.IsNullOrEmpty(Hora))
            {
                return RedirectToPage("/Error");
            }

            // Obtener el paciente usando el ID recibido
            var paciente = await _context.Users.FindAsync(idPaciente);
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
                TempData["Error-Creditos-Fisioterapeuta"] = "El paciente no tiene créditos suficientes para agendar esta cita.";
                return RedirectToPage("/Fisioterapeuta/Index");
            }

            // Convertir hora string a TimeSpan y combinar con fecha
            if (!TimeSpan.TryParse(Hora, out TimeSpan horaParsed))
            {
                TempData["Error-Hora-Fisioterapeuta"] = "La hora seleccionada no es válida.";
                return RedirectToPage("/Error");
            }

            DateTime horaInicio = Fecha.Date.Add(horaParsed);

            // Crear la cita
            var nuevaCita = new Cita
            {
                UsuarioId = paciente.Id,
                FisioterapeutaId = FisioterapeutaId,
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

            TempData["Success-Cita-Fisioterapeuta"] = "¡Cita agendada exitosamente!";
            return RedirectToPage("/Fisioterapeuta/Index");
        }



    }
}
