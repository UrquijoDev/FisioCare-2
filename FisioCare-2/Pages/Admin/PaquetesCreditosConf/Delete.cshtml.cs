using FisioCare_2.Models;
using FisioCare_2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FisioCare_2.Pages.Admin.PaquetesCreditosConf
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
            if (id <= 0)
                return RedirectToPage("/Admin/PaquetesCreditosConf/Index");

            var paquete = await _context.PaquetesCredito
                .FirstOrDefaultAsync(p => p.Id == id);

            if (paquete == null)
                return RedirectToPage("/Admin/PaquetesCreditosConf/Index");

            // Eliminar los registros relacionados (si existen, en caso de que haya asociaciones con otros modelos)

            // Eliminar paquete de la base de datos
            _context.PaquetesCredito.Remove(paquete);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Admin/PaquetesCreditosConf/Index");
        }
    }
}
