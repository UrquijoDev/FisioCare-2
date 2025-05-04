using System.ComponentModel.DataAnnotations;
namespace FisioCare_2.Models
{
    public class Feature
    {
        public int Id { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;

        public int ServicioId { get; set; }

        public Servicio Servicio { get; set; }
    }
}
