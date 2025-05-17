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
        public string PacienteID { get; set; } = string.Empty;
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


    }
}
