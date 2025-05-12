using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FisioCare_2.Models;
using System.Globalization;
using FisioCare_2.Services;

namespace FisioCare_2.Pages.Paciente.Appointment
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public DateTime Fecha { get; set; }

        [BindProperty]
        public TimeSpan Hora { get; set; }

        public Cita Cita { get; set; } = null!;
        public List<TimeSpan> HorariosDisponibles { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            Cita = await _context.Cita
                .Include(c => c.Servicio)
                .FirstOrDefaultAsync(c => c.Id == Id);

            if (Cita == null || Cita.Estado == "Cancelada")
            {
                return NotFound();
            }

            Fecha = Cita.HoraInicio.Date;
            Hora = Cita.HoraInicio.TimeOfDay;

            await CargarHorariosDisponibles();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var cita = await _context.Cita
                .Include(c => c.Servicio)
                .FirstOrDefaultAsync(c => c.Id == Id);

            if (cita == null || cita.Estado == "Cancelada")
            {
                return NotFound();
            }

            var horaNueva = Fecha.Date + Hora;
            var horaFinNueva = horaNueva + cita.Servicio.Duracion;

            // Revisar disponibilidad
            var conflictos = await _context.Cita
                .Where(c =>
                    c.Id != Id &&
                    c.FisioterapeutaId == cita.FisioterapeutaId &&
                    c.HoraInicio.Date == Fecha.Date &&
                    c.Estado != "Cancelada")
                .Include(c => c.Servicio)
                .ToListAsync();

            var hayConflicto = conflictos.Any(c =>
                horaNueva < c.HoraInicio + c.Servicio.Duracion &&
                horaFinNueva > c.HoraInicio
            );

            if (hayConflicto)
            {
                ModelState.AddModelError(string.Empty, "La hora seleccionada no está disponible.");
                await CargarHorariosDisponibles();
                return Page();
            }

            // Actualizar cita
            cita.HoraInicio = horaNueva;
            await _context.SaveChangesAsync();

            return RedirectToPage("/Paciente/Index");
        }

        private async Task CargarHorariosDisponibles()
        {
            Cita = await _context.Cita
                .Include(c => c.Servicio)
                .FirstOrDefaultAsync(c => c.Id == Id);

            var horario = _context.Horarios
                .FirstOrDefault(h =>
                    h.UsuarioId == Cita.FisioterapeutaId &&
                    h.DiaSemana.ToLower() == Fecha.ToString("dddd", new CultureInfo("es-ES")).ToLower());

            if (horario == null) return;

            var citasExistentes = _context.Cita
                .Where(c =>
                    c.FisioterapeutaId == Cita.FisioterapeutaId &&
                    c.HoraInicio.Date == Fecha.Date &&
                    c.Estado != "Cancelada" &&
                    c.Id != Cita.Id)
                .Include(c => c.Servicio)
                .ToList();

            var bloquesDisponibles = new List<TimeSpan>();
            var duracion = Cita.Servicio.Duracion;
            var horaActual = horario.HoraInicio;

            while (horaActual + duracion <= horario.HoraFin)
            {
                var inicioPropuesto = Fecha.Date + horaActual;
                var finPropuesto = inicioPropuesto + duracion;

                bool enConflicto = citasExistentes.Any(c =>
                    inicioPropuesto < (c.HoraInicio + c.Servicio.Duracion) &&
                    finPropuesto > c.HoraInicio
                );

                if (!enConflicto)
                    bloquesDisponibles.Add(horaActual);

                horaActual = horaActual.Add(TimeSpan.FromMinutes(30));
            }

            HorariosDisponibles = bloquesDisponibles;
        }
    }
}
