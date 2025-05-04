using FisioCare_2.Models;
using FisioCare_2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FisioCare_2.Pages.Admin.ServiciosFisioConf
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Servicio Servicio { get; set; }

        [BindProperty]
        public string FeaturesAsString { get; set; } = "";

        public string errorMessage = "";
        public string successMessage = "";

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                errorMessage = "Datos inválidos. Por favor revisa los campos.";
                return Page();
            }

            // Procesar las características separadas por coma
            var featuresList = FeaturesAsString
                .Split(',')
                .Select(desc => new Feature { Description = desc.Trim() }) // Limpiar espacios
                .ToList();

            Servicio.Features = featuresList;

            try
            {
                _context.Servicio.Add(Servicio);
                _context.SaveChanges();
                successMessage = "Servicio creado exitosamente.";
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                errorMessage = $"Error al guardar en base de datos: {ex.Message}";
                return Page();
            }
        }
    }
}
