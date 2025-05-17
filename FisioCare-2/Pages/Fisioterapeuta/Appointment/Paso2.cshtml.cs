using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FisioCare_2.Pages.Fisioterapeuta.Appointment
{
    public class Paso2Model : PageModel
    {


        [BindProperty(SupportsGet = true)]
        public string FisioterapeutaId { get; set; } = string.Empty;

        [BindProperty(SupportsGet = true)]
        public int ServicioId { get; set; }
        [BindProperty(SupportsGet = true)]
        public string PacienteID { get; set; } = string.Empty;

        [BindProperty]
        public DateTime FechaSeleccionada { get; set; }
        public void OnGet(string fisioterapeutaId, int servicioId, string pacienteId)
        {
            FisioterapeutaId = fisioterapeutaId;
            ServicioId = servicioId;
            PacienteID = pacienteId;
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine("Entre aqui por menso");
                return Page();
            }

            return RedirectToPage("Paso3", new
            {
                fisioterapeutaId = FisioterapeutaId,
                servicioId = ServicioId,
                pacienteID = PacienteID,
                fecha = FechaSeleccionada.ToString("yyyy-MM-dd")
            });
        }
    }
}
