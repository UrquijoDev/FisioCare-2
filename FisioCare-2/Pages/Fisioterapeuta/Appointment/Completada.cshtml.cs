using FisioCare_2.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FisioCare_2.Pages.Fisioterapeuta.Appointment
{
    public class CompletadaModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CompletadaModel(ApplicationDbContext context)
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

            // Validar que el fisioterapeuta actual sea el asignado a la cita
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (Cita.FisioterapeutaId != userId)
            {
                return Forbid();
            }

            // Cambia el estado a cancelada
            Cita.Estado = "Completada";
            await _context.SaveChangesAsync();

            // Redirigir a la página de índice del fisioterapeuta
            return RedirectToPage("/Fisioterapeuta/Index");
        }
    }
}
