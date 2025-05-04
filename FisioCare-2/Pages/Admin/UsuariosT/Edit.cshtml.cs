using FisioCare_2.Models;
using FisioCare_2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FisioCare_2.Pages.Admin.UsuariosT
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public EditModel(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [BindProperty]
        public ApplicationUser Usuario { get; set; } = new ApplicationUser();

        [BindProperty]
        public List<Horario> Horarios { get; set; } = new List<Horario>();

        [BindProperty]
        public IFormFile? ImageFile { get; set; }

        public string ExistingImage { get; set; } = string.Empty;
        public string errorMessage = "";
        public string successMessage = "";

        public IActionResult OnGet(string? id)
        {
            if (id == null)
            {
                return RedirectToPage("/Admin/UsuariosT/Index");
            }

            var usuario = _context.Users
                .Include(u => u.Horarios)
                .FirstOrDefault(u => u.Id == id);

            if (usuario == null)
            {
                return RedirectToPage("/Admin/UsuariosT/Index");
            }

            Usuario = usuario;
            Horarios = usuario.Horarios?.ToList() ?? new List<Horario>();
            ExistingImage = usuario.ImageFileName ?? "";

            return Page();
        }

        public IActionResult OnPost(string id)
        {
            if (!ModelState.IsValid)
            {
                errorMessage = "Datos inválidos. Por favor revisa el formulario.";
                return Page();
            }

            var usuario = _context.Users
                .Include(u => u.Horarios)
                .FirstOrDefault(u => u.Id == id);

            if (usuario == null)
            {
                errorMessage = "No se encontró el usuario.";
                return Page();
            }

            usuario.FirstName = Usuario.FirstName;
            usuario.LastName = Usuario.LastName;
            usuario.Email = Usuario.Email;
            usuario.UserName = Usuario.Email;

            // Imagen
            if (ImageFile != null && ImageFile.Length > 0)
            {
                string imageDir = Path.Combine(_environment.WebRootPath, "img", "users");

                if (!string.IsNullOrEmpty(usuario.ImageFileName))
                {
                    string oldImagePath = Path.Combine(imageDir, usuario.ImageFileName);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                string fileName = $"{Guid.NewGuid()}{Path.GetExtension(ImageFile.FileName)}";
                string fullPath = Path.Combine(imageDir, fileName);

                Directory.CreateDirectory(imageDir);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    ImageFile.CopyTo(stream);
                }

                usuario.ImageFileName = fileName;
            }

            // Horarios
            _context.Horarios.RemoveRange(usuario.Horarios);

            foreach (var horario in Horarios)
            {
                horario.UsuarioId = usuario.Id; // Asignamos el UsuarioId
                _context.Horarios.Add(horario);
            }

            _context.SaveChanges();
            successMessage = "Usuario actualizado correctamente.";
            return RedirectToPage("/Admin/UsuariosT/Index");
        }

    }
}
