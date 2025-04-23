using FisioCare_2.Models;
using FisioCare_2.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FisioCare_2.Pages.Admin.UsuariosT
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DeleteModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToPage("/Admin/UsuariosT/Index");

            var user = await _userManager.Users
                .Include(u => u.Horarios)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return RedirectToPage("/Admin/UsuariosT/Index");

            // Eliminar horarios
            if (user.Horarios != null && user.Horarios.Any())
                _context.Horarios.RemoveRange(user.Horarios);

            // Eliminar imagen
            if (!string.IsNullOrEmpty(user.ImageFileName))
            {
                var imagePath = Path.Combine("wwwroot", "img", "users", user.ImageFileName);
                if (System.IO.File.Exists(imagePath))
                    System.IO.File.Delete(imagePath);
            }

            // Eliminar usuario
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                // Loguear errores o manejar según tu sistema
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("/Admin/UsuariosT/Index");
        }
    }
}
