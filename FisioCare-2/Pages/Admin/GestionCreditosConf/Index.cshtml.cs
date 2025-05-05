using FisioCare_2.Models;
using FisioCare_2.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
namespace FisioCare_2.Pages.Admin.GestionCreditosConf
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<PaqueteCredito> PaquetesCredito { get; set; } = new();
        public List<ApplicationUser> Pacientes { get; set; } = new();

        public async Task OnGetAsync()
        {
            PaquetesCredito = await _context.PaquetesCredito.ToListAsync();

            Pacientes = await _context.Users
                .Where(u => u.UserName != null) // Puedes aplicar filtro por rol aqu� si lo deseas
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostAsignarCreditosAsync(string pacienteId, int paqueteId)
        {
            if (string.IsNullOrEmpty(pacienteId) || paqueteId == 0)
            {
                TempData["Error"] = "Debe seleccionar un paciente y un paquete de cr�ditos.";
                return RedirectToPage();
            }

            var paciente = await _context.Users.FirstOrDefaultAsync(u => u.Id == pacienteId);
            var paquete = await _context.PaquetesCredito.FirstOrDefaultAsync(p => p.Id == paqueteId);

            if (paciente == null || paquete == null)
            {
                TempData["Error"] = "Paciente o paquete no encontrado.";
                return RedirectToPage();
            }

            paciente.CreditosDisponibles += paquete.CantidadCreditos;

            var transaccion = new TransaccionCredito
            {
                ApplicationUserId = pacienteId,
                PaqueteCreditoId = paqueteId,
                CantidadCreditos = paquete.CantidadCreditos,
                FechaTransaccion = DateTime.Now,
                Descripcion = $"Paquete: {paquete.Nombre} asignado."
            };

            _context.TransaccionCredito.Add(transaccion);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Cr�ditos asignados correctamente al paciente.";
            return RedirectToPage();
        }
    }
}
