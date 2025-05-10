using FisioCare_2.Models;
using FisioCare_2.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FisioCare_2.Pages.Paciente.Appointment
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
        public string FisioterapeutaId { get; set; } = string.Empty;
        public List<Servicio> Servicios { get; set; } = new();
        public void OnGet(string fisioterapeutaId)
        {
            FisioterapeutaId = fisioterapeutaId;
            Servicios = _context.Servicio.ToList();
        }
    }
}
