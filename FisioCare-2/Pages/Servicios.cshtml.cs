using FisioCare_2.Models;
using FisioCare_2.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FisioCare_2.Pages
{
    public class ServiciosModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ServiciosModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<Servicio> Servicio { get; set; } = new();

        public async Task OnGet()
        {
            Servicio = await _context.Servicio
                  .Include(p => p.Features)
                  .ToListAsync();
        }
    }
}
