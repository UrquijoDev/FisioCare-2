using FisioCare_2.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class Cita
{
    public int Id { get; set; }

    // Paciente
    [Required]
    public string UsuarioId { get; set; }
    public ApplicationUser Usuario { get; set; }

    // Fisioterapeuta
    public string? FisioterapeutaId { get; set; }  // Cambié a nullable
    public ApplicationUser? Fisioterapeuta { get; set; }  // Cambié a nullable

    // Servicio asociado
    public int? ServicioId { get; set; }  // Cambié a nullable
    public Servicio? Servicio { get; set; }  // Cambié a nullable

    // Fecha y hora de inicio de la cita
    [Required]
    [DataType(DataType.DateTime)]
    public DateTime HoraInicio { get; set; }

    // Fecha y hora de fin calculada con la duración del servicio
    [NotMapped]
    public DateTime HoraFin => Servicio != null ? HoraInicio + Servicio.Duracion : HoraInicio;


    // Estado de la cita (opcional)
    [Required]
    [StringLength(20)]
    public string Estado { get; set; } = "Pendiente";

    // Motivo de la cita (opcional)
    [StringLength(500)]
    public string? Motivo { get; set; }

    // Notas del fisioterapeuta (opcional)
    [StringLength(1000)]
    public string? Notas { get; set; }

    // Fecha de creación de la cita
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
