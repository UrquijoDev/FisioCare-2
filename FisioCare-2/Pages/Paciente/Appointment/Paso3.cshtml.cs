using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FisioCare_2.Pages.Paciente.Appointment
{
    public class Paso3Model : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string FisioterapeutaId { get; set; } = string.Empty;

        [BindProperty(SupportsGet = true)]
        public int ServicioId { get; set; }

        [BindProperty]
        public DateTime FechaSeleccionada { get; set; }

        public void OnGet(string fisioterapeutaId, int servicioId)
        {
            FisioterapeutaId = fisioterapeutaId;
            ServicioId = servicioId;
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            return RedirectToPage("Paso4", new
            {
                fisioterapeutaId = FisioterapeutaId,
                servicioId = ServicioId,
                fecha = FechaSeleccionada.ToString("yyyy-MM-dd")
            });
        }
    }

}
