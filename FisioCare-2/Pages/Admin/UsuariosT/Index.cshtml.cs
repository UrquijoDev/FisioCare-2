using FisioCare_2.Models;
using FisioCare_2.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FisioCare_2.Pages.Admin.UsuariosT
{
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
            // Cargar los usuarios y sus horarios
            Usuarios = await _userManager.Users
                .Include(u => u.Horarios) // Incluir los horarios asociados a los usuarios
                .ToListAsync();

            // Cargar los roles de cada usuario
            foreach (var user in Usuarios)
            {
                var roles = await _userManager.GetRolesAsync(user);
                RolesPorUsuario[user.Id] = roles.ToList();
            }
        }

    }
}
