using FisioCare_2.Models;
using FisioCare_2.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FisioCare_2.Pages.Fisioterapeuta.Appointment
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
        [BindProperty(SupportsGet = true)]
        public string FisioterapeutaId { get; set; } = string.Empty;

        [BindProperty(SupportsGet = true)]
        public string PacienteID { get; set; }
        public List<Servicio> Servicios { get; set; } = new();
        public void OnGet(string idFisioterapeuta, string idPaciente)
        {
            FisioterapeutaId = idFisioterapeuta;
            PacienteID = idPaciente;
            Servicios = _context.Servicio.ToList();
        }

    }
}
