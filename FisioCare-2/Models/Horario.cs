namespace FisioCare_2.Models
{
    public class Horario
    {
        public int Id { get; set; }

        public string DiaSemana { get; set; } = string.Empty;

        public TimeSpan HoraInicio { get; set; }

        public TimeSpan HoraFin { get; set; }

        // Relación con Usuario
        public string? UsuarioId { get; set; } = String.Empty; // Deja este campo nullable, porque puede no tener un usuario al inicio
        public ApplicationUser? Usuario { get; set; }  // Esta propiedad debe ser nullable
    }
}
