using FisioCare_2.Models;
using FisioCare_2.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FisioCare_2.Pages.Admin.ServiciosFisioConf
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Servicio> Servicios { get; set; } = new();

        public void OnGet()
        {
            Servicios = _context.Servicio
                .Include(s => s.Features)  // Incluir características
                .ToList();
        }
    }
}
