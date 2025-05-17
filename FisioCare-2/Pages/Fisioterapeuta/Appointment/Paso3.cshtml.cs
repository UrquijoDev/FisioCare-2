using FisioCare_2.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace FisioCare_2.Pages.Fisioterapeuta.Appointment
{
    public class Paso3Model : PageModel
    {

        private readonly ApplicationDbContext _context;

        public Paso3Model(ApplicationDbContext context)
        {
            _context = context;
        }
        [BindProperty]
        public TimeSpan HoraSeleccionada { get; set; }
        [BindProperty(SupportsGet = true)]
        public string FisioterapeutaId { get; set; } = string.Empty;

        [BindProperty(SupportsGet = true)]
        public int ServicioId { get; set; }
        [BindProperty(SupportsGet = true)]
        public string PacienteID { get; set; } = string.Empty;

        [BindProperty(SupportsGet = true)]
        public DateTime Fecha { get; set; }
        // Lista de horarios generados
        public List<TimeSpan> HorariosDisponibles { get; set; } = new();
        public void OnGet(string fisioterapeutaId, int servicioId, string pacienteId, DateTime fecha)
        {
            FisioterapeutaId = fisioterapeutaId;
            ServicioId = servicioId;
            PacienteID = pacienteId;
            Fecha = fecha;

            string diaSemanax = fecha.ToString("dddd", new CultureInfo("es-ES")).ToLower();

            var horario = _context.Horarios
                .FirstOrDefault(h =>
                    h.UsuarioId == fisioterapeutaId &&
                    h.DiaSemana.ToLower() == diaSemanax);

            if (horario != null)
            {
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
            if (string.IsNullOrEmpty(FisioterapeutaId) || ServicioId == 0 || Fecha == default || HoraSeleccionada == default )
            {
                // Manejar error, redirigir o mostrar mensaje
                ModelState.AddModelError(string.Empty, "Faltan datos para confirmar la cita.");
                return Page();
            }

            Console.WriteLine("XDDDDDDDDDDDDDDDDDDDDDDD" + PacienteID + "si no sale es que esta vacio xd");

            // Redirigir a la página Confirmacion con parámetros en la URL
            return RedirectToPage("/Fisioterapeuta/Appointment/Confirmacion", new
            {
                fisioterapeutaId = FisioterapeutaId,
                idPaciente = PacienteID,
                servicioId = ServicioId,
                fecha = Fecha.ToString("yyyy-MM-dd"),
                hora = HoraSeleccionada.ToString()
            });
        }
    }
}
