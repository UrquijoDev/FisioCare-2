using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using FisioCare_2.Models;
using FisioCare_2.Services;

namespace FisioCare_2.Pages.Fisioterapeuta
{
    [Authorize(Roles = "Fisioterapeuta")]
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
        public List<Cita> CitasActivas { get; set; } = new();
        public List<Cita> CitasProximas { get; set; } = new();
        public string IdFisioterapeuta { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Account/Login");
            IdFisioterapeuta = user.Id;

            // Obtener la próxima cita asignada a este fisioterapeuta
            ProximaCita = await _context.Cita
                .Include(c => c.Usuario)
                .Include(c => c.Servicio)
                .Where(c => c.FisioterapeutaId == user.Id && c.HoraInicio >= DateTime.Now && (c.Estado == "Pendiente"))
                .OrderBy(c => c.HoraInicio)
                .FirstOrDefaultAsync();

            // Citas activas (Confirmadas y con fecha posterior a hoy)
            CitasActivas = await _context.Cita
                .Include(c => c.Usuario)
                .Include(c => c.Servicio)
                .Where(c => c.FisioterapeutaId == user.Id && c.Estado == "Pendiente" && c.HoraInicio >= DateTime.Now)
                .ToListAsync();

            // Próximas citas (Pendientes y con fecha posterior a hoy)
            CitasProximas = await _context.Cita
                .Include(c => c.Usuario)
                .Include(c => c.Servicio)
                .Where(c => c.FisioterapeutaId == user.Id && c.Estado == "Pendiente" && c.HoraInicio >= DateTime.Now)
                .OrderBy(c => c.HoraInicio)
                .Take(5)
                .ToListAsync();

            // Últimas 5 citas con este fisioterapeuta
            HistorialCitas = await _context.Cita
                .Include(c => c.Usuario)
                .Include(c => c.Servicio)
                .Where(c => c.FisioterapeutaId == user.Id && (c.Estado == "Completada" || c.Estado == "Cancelada" || c.Estado == "Pendiente"))
                .OrderByDescending(c => c.HoraInicio)
                .Take(5)
                .ToListAsync();

            return Page();
        }
    }
}
