using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FisioCare_2.Pages.Recepcionista.Appointment
{
    public class Paso3Model : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string idFisioterapeuta { get; set; } = string.Empty;
        [BindProperty(SupportsGet = true)]
        public string IDPaciente { get; set; } = string.Empty;

        [BindProperty(SupportsGet = true)]
        public int ServicioId { get; set; }

        [BindProperty]
        public DateTime FechaSeleccionada { get; set; }
        public void OnGet()
        {
            Console.WriteLine("OnGet called Fisio" + idFisioterapeuta + "Paciente"+ IDPaciente + "Servicio"+ServicioId );
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            return RedirectToPage("Paso4", new
            {
                IDPaciente,
                idFisioterapeuta,
                servicioId = ServicioId,
                fecha = FechaSeleccionada.ToString("yyyy-MM-dd")
            });
        }
    }
}
