using FisioCare_2.Models;
using FisioCare_2.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FisioCare_2.Pages.Recepcionista.Appointment
{
    public class Paso2Model : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public Paso2Model(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }
        [BindProperty(SupportsGet = true)]
        public string IDPaciente { get; set; } = string.Empty;
        [BindProperty(SupportsGet = true)]
        public string idFisioterapeuta { get; set; } = string.Empty;
        public List<Servicio> Servicios { get; set; } = new();
        public void OnGet()
        {
            Servicios = _context.Servicio.ToList();
            Console.WriteLine("Paciente" + IDPaciente + "Fisio"+ idFisioterapeuta);
        }
    }
}
