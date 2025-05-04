using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FisioCare_2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FisioCare_2.Pages
{
    public class AboutModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AboutModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public List<UserWithRole> TeamMembers { get; set; } = new();

        public async Task OnGetAsync()
        {
            var allUsers = await _userManager.Users.ToListAsync();

            foreach (var user in allUsers)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var mainRole = roles.FirstOrDefault();

                if (mainRole == "Fisioterapeuta" || mainRole == "Recepcionista" || mainRole == "Adminstrador")
                {
                    TeamMembers.Add(new UserWithRole
                    {
                        FullName = $"{user.FirstName} {user.LastName}",
                        Role = mainRole,
                        ImageFileName = user.ImageFileName
                    });
                }
            }
        }

        public class UserWithRole
        {
            public string FullName { get; set; }
            public string Role { get; set; }
            public string ImageFileName { get; set; }
        }
    }
}
