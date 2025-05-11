using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FisioCare_2.Models;
using System;
using System.Globalization;
using System.Threading.Tasks;
using FisioCare_2.Services;

namespace FisioCare_2.Pages.Paciente.Appointment
{
    public class ConfirmacionModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ConfirmacionModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public string FisioterapeutaId { get; set; } = string.Empty;

        [BindProperty(SupportsGet = true)]
        public int ServicioId { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime Fecha { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Hora { get; set; } = string.Empty;

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

        public IActionResult OnPost()
        {
            // Aquí puedes guardar la cita si haces confirmación desde botón
            return RedirectToPage("/Paciente/Citas"); // O la página que corresponda
        }
    }
}
