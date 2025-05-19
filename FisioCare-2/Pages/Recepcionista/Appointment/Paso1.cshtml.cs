using FisioCare_2.Models;
using FisioCare_2.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FisioCare_2.Pages.Recepcionista.Appointment
{
    public class Paso1Model : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public Paso1Model(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }
        public List<ApplicationUser> Fisioterapeutas { get; set; } = new();

        [BindProperty]
        public string idFisioterapeuta { get; set; } = string.Empty;
        [BindProperty(SupportsGet = true)]
        public string IDPaciente { get; set; } = string.Empty; // Agregado para almacenar el ID del paciente

        public List<Servicio> Servicios { get; set; } = new();

        public async Task OnGetAsync()
        {
            var users = _userManager.Users.ToList();
            Fisioterapeutas = new();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Contains("Fisioterapeuta"))
                {
                    Fisioterapeutas.Add(user);
                }
            }

            // Cargar servicios desde la base de datos
            Servicios = _context.Servicio.ToList();
            Console.WriteLine("Paciente " + IDPaciente);
        }
    }
}
