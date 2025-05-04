using System.ComponentModel.DataAnnotations;

namespace FisioCare_2.Models
{
    public class TransaccionCredito
    {
        public int Id { get; set; }

        [Required]
        public string ApplicationUserId { get; set; } // El paciente al que se le asignan los créditos
        public ApplicationUser ApplicationUser { get; set; }

        [Required]
        public int PaqueteCreditoId { get; set; } // El paquete de créditos asignado
        public PaqueteCredito PaqueteCredito { get; set; }

        [Required]
        public int CantidadCreditos { get; set; } // Cuántos créditos fueron asignados

        [Required]
        public DateTime FechaTransaccion { get; set; } // Fecha en que se realizó la transacción

        public string Descripcion { get; set; } = string.Empty; // Descripción de la transacción (opcional)
    }
}
