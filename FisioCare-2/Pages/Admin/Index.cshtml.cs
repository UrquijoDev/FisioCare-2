using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FisioCare_2.Models;
using FisioCare_2.Services;
using Microsoft.AspNetCore.Authorization;

namespace FisioCare_2.Pages.Admin
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

        public List<UserWithRole> EmpleadosRecientes { get; set; } = new();
        public List<PaqueteCredito> PaquetesRecientes { get; set; } = new();
        public List<Servicio> ServiciosRecientes { get; set; } = new();

        public class UserWithRole
        {
            public string FullName { get; set; }
            public string Role { get; set; }
            public string ImageFileName { get; set; }
        }

        public async Task OnGetAsync()
        {
            var allUsers = await _userManager.Users
                .OrderByDescending(u => u.CreatedAt)
                .Take(5)
                .ToListAsync();

            foreach (var user in allUsers)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var mainRole = roles.FirstOrDefault();

                if (mainRole is "Fisioterapeuta" or "Recepcionista" or "Adminstrador")
                {
                    EmpleadosRecientes.Add(new UserWithRole
                    {
                        FullName = $"{user.FirstName} {user.LastName}",
                        Role = mainRole,
                        ImageFileName = user.ImageFileName
                    });
                }
            }

            PaquetesRecientes = await _context.PaquetesCredito
                .OrderByDescending(p => p.CreatedAt)
                .Take(5)
                .ToListAsync();

            ServiciosRecientes = await _context.Servicio
                   .OrderByDescending(s => s.Id)
                   .Take(5)
                   .ToListAsync();
        }
    }
}
