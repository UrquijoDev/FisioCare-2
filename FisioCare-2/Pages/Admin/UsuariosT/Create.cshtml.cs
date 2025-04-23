using FisioCare_2.Models;
using FisioCare_2.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace FisioCare_2.Pages.Admin.UsuariosT
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public CreateModel(UserManager<ApplicationUser> userManager,
                           RoleManager<IdentityRole> roleManager,
                           ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        [BindProperty]
        public ApplicationUser Usuario { get; set; }

        [BindProperty]
        [Required]
        public string Password { get; set; }

        [BindProperty]
        public IFormFile ImageFile { get; set; }

        [BindProperty]
        [Required]
        public string SelectedRole { get; set; }

        [BindProperty]
        public List<Horario> Horarios { get; set; } = new();

        public List<SelectListItem> Roles { get; set; }

        public string? errorMessage;
        public string? successMessage;

        public async Task<IActionResult> OnGetAsync()
        {
            Roles = _roleManager.Roles
                .Select(r => new SelectListItem { Value = r.Name, Text = r.Name })
                .ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Roles = _roleManager.Roles
                .Where(r => r.Name != "Paciente") // Excluye pacientes
                .Select(r => new SelectListItem { Value = r.Name, Text = r.Name })
                .ToList();

            if (SelectedRole == "Admin")
            {
                // Limpia los errores de validación que puedan haberse generado para los horarios
                for (int i = 0; i < Horarios.Count; i++)
                {
                    ModelState.Remove($"Horarios[{i}].HoraInicio");
                    ModelState.Remove($"Horarios[{i}].HoraFin");
                }
            }

            if (!ModelState.IsValid)
            {
                foreach (var entry in ModelState)
                {
                    foreach (var error in entry.Value.Errors)
                    {
                        Console.WriteLine($"Error en {entry.Key}: {error.ErrorMessage}");
                    }
                }

                errorMessage = "Datos inválidos. Revisa los logs para más detalles.";
                return Page();
            }


            if (ImageFile != null && ImageFile.Length > 0)
            {
                string fileName = $"{Guid.NewGuid()}{Path.GetExtension(ImageFile.FileName)}";
                string path = Path.Combine("wwwroot", "img", "users");
                Directory.CreateDirectory(path);
                string fullPath = Path.Combine(path, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                Usuario.ImageFileName = fileName;
            }

            Usuario.UserName = Usuario.Email;
            var result = await _userManager.CreateAsync(Usuario, Password);

            if (!result.Succeeded)
            {
                errorMessage = string.Join(" | ", result.Errors.Select(e => e.Description));
                return Page();
            }

            await _userManager.AddToRoleAsync(Usuario, SelectedRole);

            // Ahora que el usuario ya está creado y tiene su ID, podemos crear sus horarios
            if (SelectedRole != "Admin" && Horarios != null)
            {
                foreach (var horario in Horarios)
                {
                    if (horario.HoraInicio != default && horario.HoraFin != default)
                    {
                        horario.UsuarioId = Usuario.Id;
                        _context.Horarios.Add(horario);
                    }
                }

                await _context.SaveChangesAsync();
            }

            successMessage = "Usuario creado exitosamente.";
            return RedirectToPage("/Admin/UsuariosT/Index");
        }

    }
}
