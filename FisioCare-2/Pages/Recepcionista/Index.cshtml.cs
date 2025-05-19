using FisioCare_2.Models;
using FisioCare_2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FisioCare_2.Pages.Recepcionista
{
    [Authorize(Roles = "Recepcionista")]
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
        public List<Cita> TodasLasCitas { get; set; } = new();
        public List<ApplicationUser> TodosLosPacientes { get; set; } = new();

        public string IdFisioterapeuta { get; set; } = "";

        public async Task<IActionResult> OnGetAsync()
        {
            // Obtener todos los usuarios con el rol "Paciente"
            var users = await _context.Users.ToListAsync();
            TodosLosPacientes = new();

            foreach (var user in users)
            {
                if (await _userManager.IsInRoleAsync(user, "Paciente"))
                {
                    TodosLosPacientes.Add(user);
                }
            }

            // Próxima cita pendiente más cercana
            ProximaCita = await _context.Cita
                .Include(c => c.Usuario)
                .Include(c => c.Servicio)
                .Include(c => c.Fisioterapeuta)
                .Where(c => c.HoraInicio >= DateTime.Now && c.Estado == "Pendiente")
                .OrderBy(c => c.HoraInicio)
                .FirstOrDefaultAsync();

            // Citas activas (pendientes y futuras)
            CitasActivas = await _context.Cita
                .Include(c => c.Usuario)
                .Include(c => c.Servicio)
                .Include(c => c.Fisioterapeuta)
                .Where(c => c.Estado == "Pendiente" && c.HoraInicio >= DateTime.Now)
                .ToListAsync();

            // Próximas 5 citas
            CitasProximas = await _context.Cita
                .Include(c => c.Usuario)
                .Include(c => c.Servicio)
                .Include(c => c.Fisioterapeuta)
                .Where(c => c.Estado == "Pendiente" && c.HoraInicio >= DateTime.Now)
                .OrderBy(c => c.HoraInicio)
                .Take(5)
                .ToListAsync();

            // Historial de las últimas 5 citas (completadas, canceladas o pendientes pasadas)
            HistorialCitas = await _context.Cita
                .Include(c => c.Usuario)
                .Include(c => c.Servicio)
                .Include(c => c.Fisioterapeuta)
                .Where(c => c.Estado == "Completada" || c.Estado == "Cancelada" || c.Estado == "Pendiente")
                .OrderByDescending(c => c.HoraInicio)
                .ToListAsync();

            // Todas las citas para reagendar pacientes únicos
            TodasLasCitas = await _context.Cita
                .Include(c => c.Usuario)
                .Include(c => c.Servicio)
                .Include(c => c.Fisioterapeuta)
                .ToListAsync();

            return Page();
        }
    }
}
