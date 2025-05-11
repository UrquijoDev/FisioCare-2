using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FisioCare_2.Models;
using System;
using System.Globalization;
using System.Threading.Tasks;
using FisioCare_2.Services;
using Microsoft.AspNetCore.Identity;

namespace FisioCare_2.Pages.Paciente.Appointment
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
        public int ServicioId { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime Fecha { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Hora { get; set; } = string.Empty;

        // Propiedades para mostrar en vista
        public string NombreFisioterapeuta { get; set; } = string.Empty;
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

            if (fisioterapeuta == null || servicio == null)
            {
                return RedirectToPage("/Error");
            }

            NombreFisioterapeuta = $"{fisioterapeuta.FirstName} {fisioterapeuta.LastName}";
            NombreServicio = servicio.Nombre;

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
            if (string.IsNullOrEmpty(FisioterapeutaId) || ServicioId == 0 || Fecha == default || string.IsNullOrEmpty(Hora))
            {
                return RedirectToPage("/Error");
            }

            // Obtener el usuario logueado (paciente)
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            // Obtener el servicio
            var servicio = await _context.Servicio.FindAsync(ServicioId);
            if (servicio == null)
            {
                return RedirectToPage("/Error");
            }

            // Validar créditos suficientes
            if (user.CreditosDisponibles < servicio.CreditosNecesarios)
            {
                TempData["Error"] = "No tienes créditos suficientes para agendar esta cita.";
                return RedirectToPage("/Paciente/Index"); // O volver a la página anterior
            }

            // Convertir hora string a TimeSpan y combinar con fecha
            if (!TimeSpan.TryParse(Hora, out TimeSpan horaParsed))
            {
                TempData["Error"] = "La hora seleccionada no es válida.";
                return RedirectToPage("/Error");
            }

            DateTime horaInicio = Fecha.Date.Add(horaParsed);

            // Crear la cita
            var nuevaCita = new Cita
            {
                UsuarioId = user.Id,
                FisioterapeutaId = FisioterapeutaId,
                ServicioId = ServicioId,
                HoraInicio = horaInicio,
                Estado = "Pendiente",
                CreatedAt = DateTime.Now
            };

            _context.Cita.Add(nuevaCita);

            // Descontar créditos del paciente
            user.CreditosDisponibles -= servicio.CreditosNecesarios;
            _context.Users.Update(user);

            await _context.SaveChangesAsync();

            TempData["Success"] = "¡Cita agendada exitosamente!";
            return RedirectToPage("/Paciente/Index");
        }
    }
}
