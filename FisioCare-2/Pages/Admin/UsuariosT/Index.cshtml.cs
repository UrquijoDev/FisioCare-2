using FisioCare_2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FisioCare_2.Pages.Admin.UsuariosT
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public List<ApplicationUser> Usuarios { get; set; }
        public Dictionary<string, List<string>> RolesPorUsuario { get; set; } = new();
        public UserManager<ApplicationUser> UserManager => _userManager;
        public async Task OnGetAsync()
        {
            Usuarios = _userManager.Users.ToList();

            foreach (var user in Usuarios)
            {
                var roles = await _userManager.GetRolesAsync(user);
                RolesPorUsuario[user.Id] = roles.ToList();
            }
        }
    }
}
