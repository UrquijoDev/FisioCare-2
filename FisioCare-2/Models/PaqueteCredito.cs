using System.ComponentModel.DataAnnotations;

namespace FisioCare_2.Models
{
    public class PaqueteCredito
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Precio { get; set; } // Precio en moneda (ej. MXN)

        [Required]
        [Range(1, int.MaxValue)]
        public int CantidadCreditos { get; set; } // Número de créditos incluidos

        [StringLength(255)]
        public string? Descripcion { get; set; } // Opcional, para explicar qué incluye o ventajas

        public DateTime CreatedAt { get; set; } = DateTime.Now; // Para control interno
    }
}
