using FisioCare_2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FisioCare_2.Pages
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IndexModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (_signInManager.IsSignedIn(User))
            {
                var user = await _userManager.GetUserAsync(User);
                var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

                if (isAdmin)
                {
                    // Redirige a la p�gina de Admin si es admin
                    return RedirectToPage("/Admin/Index");
                }
            }

            return Page(); // Muestra la p�gina normalmente si no es admin
        }
    }

}
