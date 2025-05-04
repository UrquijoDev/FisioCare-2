using FisioCare_2.Models;
using FisioCare_2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FisioCare_2.Pages.Admin.ServiciosFisioConf
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Servicio Servicio { get; set; } = new Servicio();

        [BindProperty]
        public string FeaturesAsString { get; set; }

        // Mensajes de éxito y error
        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || id <= 0)
            {
                ErrorMessage = "ID no válido.";
                return RedirectToPage("/Admin/ServiciosFisioConf/Index");
            }

            var servicio = await _context.Servicio
                .Include(s => s.Features) // Cargar las características asociadas si las hay
                .FirstOrDefaultAsync(s => s.Id == id);

            if (servicio == null)
            {
                ErrorMessage = "Servicio no encontrado.";
                return RedirectToPage("/Admin/ServiciosFisioConf/Index");
            }

            Servicio = servicio;
            FeaturesAsString = string.Join(",", servicio.Features.Select(f => f.Description)); // Convertir las características a una cadena separada por comas.
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ErrorMessage = "Por favor corrige los errores del formulario.";
                return Page();
            }

            var servicioExistente = await _context.Servicio
                .Include(s => s.Features)
                .FirstOrDefaultAsync(s => s.Id == Servicio.Id);

            if (servicioExistente == null)
            {
                ErrorMessage = "No se encontró el servicio.";
                return RedirectToPage("/Admin/ServiciosFisioConf/Index");
            }

            // Actualizar las propiedades del servicio
            servicioExistente.Nombre = Servicio.Nombre;
            servicioExistente.Descripcion = Servicio.Descripcion;
            servicioExistente.CreditosNecesarios = Servicio.CreditosNecesarios;
            servicioExistente.PrecioReferencia = Servicio.PrecioReferencia;
            servicioExistente.Duracion = Servicio.Duracion;

            // Actualizar características (si es necesario)
            if (!string.IsNullOrEmpty(FeaturesAsString))
            {
                var features = FeaturesAsString.Split(',').Select(f => f.Trim()).ToList();
                servicioExistente.Features.Clear();
                foreach (var feature in features)
                {
                    servicioExistente.Features.Add(new Feature
                    {
                        Description = feature,
                        ServicioId = servicioExistente.Id
                    });
                }
            }

            try
            {
                await _context.SaveChangesAsync();
                SuccessMessage = "Servicio actualizado correctamente.";
                return RedirectToPage("/Admin/ServiciosFisioConf/Index");
            }
            catch (DbUpdateException)
            {
                ErrorMessage = "Hubo un problema al guardar los cambios.";
                return Page();
            }
        }
    }
}
