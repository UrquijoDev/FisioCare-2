using FisioCare_2.Models;
using FisioCare_2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FisioCare_2.Pages.Admin.UsuariosT
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public IndexModel(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public List<ApplicationUser> Usuarios { get; set; }
        public Dictionary<string, List<string>> RolesPorUsuario { get; set; } = new();
        public UserManager<ApplicationUser> UserManager => _userManager;
        public ICollection<Horario> Horarios { get; set; }
        public async Task OnGetAsync()
        {
            // Obtener todos los usuarios con sus horarios
            var todosLosUsuarios = await _userManager.Users
                .Include(u => u.Horarios)
                .ToListAsync();

            Usuarios = new List<ApplicationUser>();

            foreach (var user in todosLosUsuarios)
            {
                var roles = await _userManager.GetRolesAsync(user);

                // Ignorar usuarios que tengan el rol "Paciente"
                if (!roles.Contains("Paciente"))
                {
                    Usuarios.Add(user);
                    RolesPorUsuario[user.Id] = roles.ToList();
                }
            }
        }


    }
}
