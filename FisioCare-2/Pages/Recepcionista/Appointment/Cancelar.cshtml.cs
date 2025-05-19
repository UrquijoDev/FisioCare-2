using FisioCare_2.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FisioCare_2.Pages.Recepcionista.Appointment
{
    public class CancelarModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CancelarModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Cita Cita { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Cita = await _context.Cita.FindAsync(id);

            if (Cita == null)
            {
                return NotFound();
            }
            // Cambia el estado a cancelada
            Cita.Estado = "Cancelada";
            await _context.SaveChangesAsync();

            // Redirigir a la página de índice del fisioterapeuta
            return RedirectToPage("/Recepcionista/Index");
        }
    }
}
