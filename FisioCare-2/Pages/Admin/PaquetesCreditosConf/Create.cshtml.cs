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

        // Método GET
        public void OnGet()
        {
            // Esto solo se ejecuta cuando se carga la página (no es necesario nada para esta página en este momento)
        }

        // Método POST
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page(); // Si hay algún error en la validación, volverá a la página.
            }

            try
            {
                // Agregar el paquete de crédito a la base de datos
                _context.PaquetesCredito.Add(PaqueteCredito);
                await _context.SaveChangesAsync();

                successMessage = "El paquete de créditos ha sido creado exitosamente.";
                return RedirectToPage("/Admin/PaquetesCreditosConf/Index"); // Redirige al listado de paquetes
            }
            catch (System.Exception)
            {
                errorMessage = "Hubo un error al guardar el paquete de créditos. Inténtelo nuevamente.";
                return Page(); // Si ocurre un error, regresa a la página de creación.
            }
        }
    }
}
