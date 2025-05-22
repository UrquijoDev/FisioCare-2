using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FisioCare_2.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FisioCare_2.Services;
using System.Security.AccessControl;

namespace FisioCare_2.Pages.Paciente.Appointment
{
    public class Paso4Model : PageModel
    {
        private readonly ApplicationDbContext _context;

        public Paso4Model(ApplicationDbContext context)
        {
            _context = context;
        }
        [BindProperty]
        public TimeSpan HoraSeleccionada { get; set; }


        // Propiedades para almacenar los parámetros recibidos
        [BindProperty(SupportsGet = true)]
        public string FisioterapeutaId { get; set; } = string.Empty;

        [BindProperty(SupportsGet = true)]
        public int ServicioId { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime Fecha { get; set; }

        // Lista de horarios generados
        public List<TimeSpan> HorariosDisponibles { get; set; } = new();

        public void OnGet(string fisioterapeutaId, int servicioId, DateTime fecha)
        {
            FisioterapeutaId = fisioterapeutaId;
            ServicioId = servicioId;
            Fecha = fecha;

            string diaSemanax = fecha.ToString("dddd", new CultureInfo("es-ES")).ToLower();

            var horario = _context.Horarios
                .FirstOrDefault(h =>
                    h.UsuarioId == fisioterapeutaId &&
                    h.DiaSemana.ToLower() == diaSemanax);

            if (horario != null)
            {
                Console.WriteLine(FisioterapeutaId);
                TimeSpan horaInicioFisioterapeuta = horario.HoraInicio;
                TimeSpan horaFinFisioterapeuta = horario.HoraFin;

                // Obtener duración del servicio
                var servicio = _context.Servicio.FirstOrDefault(s => s.Id == servicioId);
                if (servicio == null)
                {
                    HorariosDisponibles = new(); // No hay servicio
                    return;
                }

                TimeSpan duracionServicio = servicio.Duracion;

                // Obtener citas existentes del fisioterapeuta para esa fecha
                var citasExistentes = _context.Cita
                    .Where(c =>
                        c.FisioterapeutaId == fisioterapeutaId &&
                        c.HoraInicio.Date == fecha.Date &&
                        c.Estado != "Cancelada") // solo contar las activas
                    .Include(c => c.Servicio) // importante para calcular la HoraFin
                    .ToList();

                // Generar bloques disponibles
                var bloquesDisponibles = new List<TimeSpan>();
                var horaActual = horaInicioFisioterapeuta;

                while (horaActual + duracionServicio <= horaFinFisioterapeuta)
                {
                    var inicioPropuesto = fecha.Date + horaActual;
                    var finPropuesto = inicioPropuesto + duracionServicio;

                    bool enConflicto = citasExistentes.Any(c =>
                        inicioPropuesto < (c.HoraInicio + c.Servicio.Duracion) &&
                        finPropuesto > c.HoraInicio
                    );

                    if (!enConflicto)
                    {
                        bloquesDisponibles.Add(horaActual);
                    }

                    horaActual = horaActual.Add(TimeSpan.FromMinutes(30)); // Avanza en bloques
                }

                HorariosDisponibles = bloquesDisponibles;
            }
            else
            {
                HorariosDisponibles = new List<TimeSpan>();
            }
        }


        public IActionResult OnPost()
        {
            // Asegúrate de que todos los datos estén presentes
            if (string.IsNullOrEmpty(FisioterapeutaId) || ServicioId == 0 || Fecha == default || HoraSeleccionada == default)
            {
                // Manejar error, redirigir o mostrar mensaje
                ModelState.AddModelError(string.Empty, "Faltan datos para confirmar la cita.");
                return Page();
            }

            // Redirigir a la página Confirmacion con parámetros en la URL
            return RedirectToPage("/Paciente/Appointment/Confirmacion", new
            {
                fisioterapeutaId = FisioterapeutaId,
                servicioId = ServicioId,
                fecha = Fecha.ToString("yyyy-MM-dd"),
                hora = HoraSeleccionada.ToString()
            });
        }

    }
}
