using FisioCare_2.Models;
using FisioCare_2.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FisioCare_2.Pages.Recepcionista
{
    public class GestionCreditosModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public GestionCreditosModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<PaqueteCredito> PaquetesCredito { get; set; } = new();
        [BindProperty(SupportsGet = true)]
        public string idPaciente { get; set; } = string.Empty;
        public async Task OnGetAsync()
        {
            PaquetesCredito = await _context.PaquetesCredito.ToListAsync();
        }

        public async Task<IActionResult> OnPostAsignarCreditosAsync(string idPaciente,int paqueteId)
        {
            if (string.IsNullOrEmpty(idPaciente) || paqueteId == 0)
            {
                TempData["Error-creditos"] = "Debe seleccionar un paciente y un paquete de créditos.";
                Console.WriteLine(idPaciente + "   " + paqueteId);
                return RedirectToPage();
            }

            var paciente = await _context.Users.FirstOrDefaultAsync(u => u.Id == idPaciente);
            var paquete = await _context.PaquetesCredito.FirstOrDefaultAsync(p => p.Id == paqueteId);

            if (paciente == null || paquete == null)
            {
                TempData["Error-paciente"] = "Paciente o paquete no encontrado.";
                return RedirectToPage();
            }

            paciente.CreditosDisponibles += paquete.CantidadCreditos;

            var transaccion = new TransaccionCredito
            {
                ApplicationUserId = idPaciente,
                PaqueteCreditoId = paqueteId,
                CantidadCreditos = paquete.CantidadCreditos,
                FechaTransaccion = DateTime.Now,
                Descripcion = $"Paquete: {paquete.Nombre} asignado."
            };

            _context.TransaccionCredito.Add(transaccion);
            await _context.SaveChangesAsync();
            TempData["Success-creditos"] = "Créditos asignados correctamente al paciente.";
            return RedirectToPage("/Recepcionista/Index");
        }
    }
}
