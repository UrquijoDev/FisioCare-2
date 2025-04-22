namespace FisioCare_2.Models
{
    public class Horario
    {
        public int Id { get; set; }

        public string DiaSemana { get; set; } = string.Empty;

        public TimeSpan HoraInicio { get; set; }

        public TimeSpan HoraFin { get; set; }

        // Relación con Usuario
        public string UsuarioId { get; set; }
        public ApplicationUser Usuario { get; set; } = null!;
    }
}
