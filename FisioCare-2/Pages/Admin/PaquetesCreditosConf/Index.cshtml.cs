using FisioCare_2.Models;
using FisioCare_2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FisioCare_2.Pages.Admin.PaquetesCreditosConf
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<PaqueteCredito> PaquetesCredito { get; set; }

        public async Task OnGetAsync()
        {
            PaquetesCredito = await _context.PaquetesCredito.ToListAsync();
        }
    }

}
