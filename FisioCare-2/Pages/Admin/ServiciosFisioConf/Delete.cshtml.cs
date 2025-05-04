using FisioCare_2.Models;
using FisioCare_2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FisioCare_2.Pages.Admin.ServiciosFisioConf
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            // Si no se proporciona un ID, redirigir al índice
            if (id == 0)
                return RedirectToPage("/Admin/ServiciosFisioConf/Index");

            // Buscar el servicio por ID
            var servicio = await _context.Servicio
                .Include(s => s.Features)  // Asumiendo que quieres eliminar características también
                .FirstOrDefaultAsync(s => s.Id == id);

            if (servicio == null)
                return RedirectToPage("/Admin/ServiciosFisioConf/Index");

            // Eliminar características si existen
            if (servicio.Features != null && servicio.Features.Any())
                _context.Feature.RemoveRange(servicio.Features);

            // Eliminar servicio
            _context.Servicio.Remove(servicio);

            // Guardar cambios
            await _context.SaveChangesAsync();

            // Redirigir al índice
            return RedirectToPage("/Admin/ServiciosFisioConf/Index");
        }
    }
}
