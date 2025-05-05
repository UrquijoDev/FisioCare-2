using FisioCare_2.Models;
using FisioCare_2.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

            // Obtener RoleId del rol "Paciente"
            var pacienteRoleId = await _context.Roles
                .Where(r => r.Name == "PACIENTE")
                .Select(r => r.Id)
                .FirstOrDefaultAsync();

            // Obtener IDs de los usuarios con ese rol
            var userIdsConRolPaciente = await _context.UserRoles
                .Where(ur => ur.RoleId == pacienteRoleId)
                .Select(ur => ur.UserId)
                .ToListAsync();

            // Traer los usuarios con ese ID
            Pacientes = await _context.Users
                .Where(u => userIdsConRolPaciente.Contains(u.Id))
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostAsignarCreditosAsync(string pacienteId, int paqueteId)
        {
            if (string.IsNullOrEmpty(pacienteId) || paqueteId == 0)
            {
                TempData["Error"] = "Debe seleccionar un paciente y un paquete de créditos.";
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

            TempData["Success"] = "Créditos asignados correctamente al paciente.";
            return RedirectToPage();
        }
    }
}
