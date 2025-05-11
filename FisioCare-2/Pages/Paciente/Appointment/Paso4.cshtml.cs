using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FisioCare_2.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FisioCare_2.Services;

namespace FisioCare_2.Pages.Paciente.Appointment
{
    public class Paso4Model : PageModel
    {
        private readonly ApplicationDbContext _context;

        public Paso4Model(ApplicationDbContext context)
        {
            _context = context;
        }

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

            // Obtener el día de la semana en español, en minúsculas
            string diaSemanax = fecha.ToString("dddd", new CultureInfo("es-ES")).ToLower();

            // Convertir el ID entero del fisioterapeuta a string (si viene así desde la URL)
            string fisioterapeutaIdStr = fisioterapeutaId.ToString();

            // Buscar el horario correspondiente en la base de datos
            Console.WriteLine("tiren paro we" + fisioterapeutaIdStr + diaSemanax);
            
            var horario = _context.Horarios
                .FirstOrDefault(h =>
                    h.UsuarioId == fisioterapeutaIdStr && h.DiaSemana == diaSemanax);

            // Validar si existe
            if (horario != null)
            {
                TimeSpan horaInicioFisioterapeuta = horario.HoraInicio;
                TimeSpan horaFinFisioterapeuta = horario.HoraFin;

                Console.WriteLine(horaInicioFisioterapeuta); Console.WriteLine(horaFinFisioterapeuta);

            }
            else
            {
                // El fisioterapeuta no trabaja ese día
                HorariosDisponibles = new List<TimeSpan>(); // Vacío
                Console.WriteLine("estoy nullxd");
            }
        }
    }
}
