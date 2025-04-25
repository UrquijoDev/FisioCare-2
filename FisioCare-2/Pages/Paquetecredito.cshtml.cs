using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FisioCare_2.Models; // Ajusta al namespace de tu modelo
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;
using FisioCare_2.Services;

namespace FisioCare_2.Pages
{
    public class PaquetecreditoModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public PaquetecreditoModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<PaqueteCredito> Paquetes { get; set; }

        public async Task OnGetAsync()
        {
            Paquetes = await _context.PaquetesCredito.ToListAsync();
        }
    }
}
