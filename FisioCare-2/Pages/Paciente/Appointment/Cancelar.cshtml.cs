using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FisioCare_2.Models;
using System.Threading.Tasks;
using FisioCare_2.Services;

namespace FisioCare_2.Pages.Paciente.Appointment
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

            // Aquí puedes agregar una validación para asegurarte de que el usuario actual sea el dueño de la cita
            if (Cita.UsuarioId != User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value)
            {
                return Forbid();
            }

            // Cambia el estado a cancelada
            Cita.Estado = "Cancelada";
            await _context.SaveChangesAsync();

            // Puedes redirigir de nuevo al index del portal del paciente
            return RedirectToPage("/Paciente/Index");
        }
    }
}
