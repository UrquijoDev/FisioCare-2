using FisioCare_2.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace FisioCare_2.Pages.Recepcionista.Appointment
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
        [BindProperty(SupportsGet = true)]
        public string idFisioterapeuta { get; set; } = string.Empty;
        [BindProperty(SupportsGet = true)]
        public string IDPaciente { get; set; } = string.Empty;

        [BindProperty(SupportsGet = true)]
        public int ServicioId { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime fecha { get; set; }
        // Lista de horarios generados
        public List<TimeSpan> HorariosDisponibles { get; set; } = new();
        public void OnGet()
        {


            string diaSemanax = fecha.ToString("dddd", new CultureInfo("es-ES")).ToLower();

            var horario = _context.Horarios
                .FirstOrDefault(h =>
                    h.UsuarioId == idFisioterapeuta &&
                    h.DiaSemana.ToLower() == diaSemanax);

            if (horario != null)
            {
                TimeSpan horaInicioFisioterapeuta = horario.HoraInicio;
                TimeSpan horaFinFisioterapeuta = horario.HoraFin;

                // Obtener duración del servicio
                var servicio = _context.Servicio.FirstOrDefault(s => s.Id == ServicioId);
                if (servicio == null)
                {
                    HorariosDisponibles = new(); // No hay servicio
                    return;
                }

                TimeSpan duracionServicio = servicio.Duracion;

                // Obtener citas existentes del fisioterapeuta para esa fecha
                var citasExistentes = _context.Cita
                    .Where(c =>
                        c.FisioterapeutaId == idFisioterapeuta &&
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
            if (string.IsNullOrEmpty(idFisioterapeuta) || ServicioId == 0 || fecha == default || HoraSeleccionada == default)
            {
                // Manejar error, redirigir o mostrar mensaje
                ModelState.AddModelError(string.Empty, "Faltan datos para confirmar la cita.");
                return Page();
            }

            Console.WriteLine("Paciente " +IDPaciente+ "Fisio" +idFisioterapeuta+ "Servicio" +ServicioId + "Fecha " + fecha+ "hora " + HoraSeleccionada);

            // Redirigir a la página Confirmacion con parámetros en la URL
            return RedirectToPage("/Recepcionista/Appointment/Confirmacion", new
            {
                IDPaciente,
                idFisioterapeuta,
                servicioId = ServicioId,
                fecha = fecha.ToString("yyyy-MM-dd"),
                hora = HoraSeleccionada.ToString()
            });
        }
    }
}
