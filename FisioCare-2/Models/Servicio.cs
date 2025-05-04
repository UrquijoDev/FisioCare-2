using System.ComponentModel.DataAnnotations;
namespace FisioCare_2.Models
{
    public class Servicio
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [StringLength(500)]
        public string Descripcion { get; set; } = string.Empty;

        [Range(1, 100)]
        public int CreditosNecesarios { get; set; }

        [Range(0, 10000)]
        public decimal PrecioReferencia { get; set; } // Opcional: si también permites pagar sin créditos

        [DataType(DataType.Duration)]
        public TimeSpan Duracion { get; set; } // Por ejemplo: 00:45:00 para 45 minutos

        public List<Feature> Features { get; set; } = new();

    }
}
