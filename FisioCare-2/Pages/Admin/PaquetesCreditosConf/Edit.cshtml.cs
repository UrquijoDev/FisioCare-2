using FisioCare_2.Models;
using FisioCare_2.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FisioCare_2.Pages.Admin.PaquetesCreditosConf
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public PaqueteCredito Paquete { get; set; } = new PaqueteCredito();

        public string errorMessage = "";
        public string successMessage = "";

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || id <= 0)
            {
                errorMessage = "ID no válido.";
                return RedirectToPage("/Admin/PaquetesCreditosConf/Index");
            }

            var paquete = await _context.PaquetesCredito.FirstOrDefaultAsync(p => p.Id == id);

            if (paquete == null)
            {
                errorMessage = "Paquete no encontrado.";
                return RedirectToPage("/Admin/PaquetesCreditosConf/Index");
            }

            Paquete = paquete;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                errorMessage = "Por favor corrige los errores del formulario.";
                return Page();
            }

            var paqueteExistente = await _context.PaquetesCredito
                .FirstOrDefaultAsync(p => p.Id == Paquete.Id);

            if (paqueteExistente == null)
            {
                errorMessage = "No se encontró el paquete.";
                return RedirectToPage("/Admin/PaquetesCreditosConf/Index");
            }

            // Actualizar propiedades
            paqueteExistente.Nombre = Paquete.Nombre;
            paqueteExistente.Precio = Paquete.Precio;
            paqueteExistente.CantidadCreditos = Paquete.CantidadCreditos;
            paqueteExistente.Descripcion = Paquete.Descripcion;

            try
            {
                await _context.SaveChangesAsync();
                successMessage = "Paquete actualizado correctamente.";
                return RedirectToPage("/Admin/PaquetesCreditosConf/Index");
            }
            catch (DbUpdateException)
            {
                errorMessage = "Hubo un problema al guardar los cambios.";
                return Page();
            }
        }
    }
}
