using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using FisioCare_2.Services; // Ajusta si tu ApplicationUser está en otra carpeta
using FisioCare_2.Models; // Tu modelo extendido de IdentityUser
using System.IO;

namespace FisioCare_2.Pages.Admin.UsuariosT
{
    public class CreateModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _env;

        public CreateModel(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _env = env;
        }

        [BindProperty]
        public ApplicationUser Usuario { get; set; } = new ApplicationUser();

        [BindProperty]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [BindProperty]
        public string SelectedRole { get; set; } = string.Empty;

        [BindProperty]
        public IFormFile? ImageFile { get; set; }

        public SelectList Roles { get; set; } = default!;

        public string errorMessage = "";
        public string successMessage = "";

        public void OnGet()
        {
            Roles = new SelectList(_roleManager.Roles.Select(r => r.Name).ToList());
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Roles = new SelectList(_roleManager.Roles.Select(r => r.Name).ToList());

            if (!ModelState.IsValid)
            {
                errorMessage = "Por favor revisa los campos del formulario.";
                return Page();
            }

            // Procesar imagen
            if (ImageFile != null && ImageFile.Length > 0)
            {
                string fileName = $"{Guid.NewGuid()}{Path.GetExtension(ImageFile.FileName)}";
                string folderPath = Path.Combine(_env.WebRootPath, "img", "users");

                Directory.CreateDirectory(folderPath); // Asegura que exista
                string filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                Usuario.ImageFileName = fileName;
            }

            Usuario.UserName = Usuario.Email; // ? ESTA ES LA OPCIÓN 1
            var result = await _userManager.CreateAsync(Usuario, Password);
            if (!result.Succeeded)
            {
                errorMessage = string.Join("; ", result.Errors.Select(e => e.Description));
                return Page();
            }

            if (!string.IsNullOrEmpty(SelectedRole))
            {
                var roleResult = await _userManager.AddToRoleAsync(Usuario, SelectedRole);
                if (!roleResult.Succeeded)
                {
                    errorMessage = "Usuario creado, pero no se pudo asignar el rol.";
                    return Page();
                }
            }


            successMessage = "Usuario creado exitosamente.";
            return RedirectToPage("/Admin/UsuariosT/Index");
        }
    }
}
