using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FisioCare_2.Models;
using Microsoft.AspNetCore.Identity;
using FisioCare_2.Services;

namespace FisioCare_2.Pages.Paciente
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Cita? ProximaCita { get; set; }
        public List<Cita> HistorialCitas { get; set; } = new();
        public int CreditosDisponibles { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Account/Login");
            CreditosDisponibles = user.CreditosDisponibles;
            // Obtener la próxima cita pendiente o confirmada, con el fisioterapeuta
            ProximaCita = await _context.Cita
                .Include(c => c.Fisioterapeuta)  // Esto carga el objeto del fisioterapeuta completo
                .Include(c => c.Servicio)
                .Where(c => c.UsuarioId == user.Id && c.HoraInicio >= DateTime.Now && (c.Estado == "Pendiente" || c.Estado == "Confirmada"))
                .OrderBy(c => c.HoraInicio)
                .FirstOrDefaultAsync();

            // Últimas 5 citas completadas, con el fisioterapeuta
            HistorialCitas = await _context.Cita
                .Include(c => c.Fisioterapeuta)  // Esto también incluye el fisioterapeuta
                .Include(c => c.Servicio)
                .Where(c => c.UsuarioId == user.Id && (c.Estado == "Completada" || c.Estado == "Confirmada" || c.Estado == "Pendiente" || c.Estado == "Cancelada"))
                .OrderByDescending(c => c.HoraInicio)
                .Take(5)
                .ToListAsync();

            return Page();
        }

    }
}
