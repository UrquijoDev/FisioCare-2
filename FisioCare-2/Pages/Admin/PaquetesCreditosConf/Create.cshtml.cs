using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FisioCare_2.Models;
using System.Threading.Tasks;
using FisioCare_2.Services;

namespace FisioCare_2.Pages.Admin.PaquetesCreditosConf
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public PaqueteCredito PaqueteCredito { get; set; }

        public string? successMessage { get; set; }
        public string? errorMessage { get; set; }

        // M�todo GET
        public void OnGet()
        {
            // Esto solo se ejecuta cuando se carga la p�gina (no es necesario nada para esta p�gina en este momento)
        }

        // M�todo POST
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page(); // Si hay alg�n error en la validaci�n, volver� a la p�gina.
            }

            try
            {
                // Agregar el paquete de cr�dito a la base de datos
                _context.PaquetesCredito.Add(PaqueteCredito);
                await _context.SaveChangesAsync();

                successMessage = "El paquete de cr�ditos ha sido creado exitosamente.";
                return RedirectToPage("/Admin/PaquetesCreditosConf/Index"); // Redirige al listado de paquetes
            }
            catch (System.Exception)
            {
                errorMessage = "Hubo un error al guardar el paquete de cr�ditos. Int�ntelo nuevamente.";
                return Page(); // Si ocurre un error, regresa a la p�gina de creaci�n.
            }
        }
    }
}
