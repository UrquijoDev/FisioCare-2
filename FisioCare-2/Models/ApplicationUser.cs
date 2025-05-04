using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FisioCare_2.Models
{
    public class ApplicationUser: IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string ImageFileName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        // Agregado para manejar el saldo de créditos del paciente
        [Range(0, int.MaxValue)]
        public int CreditosDisponibles { get; set; } = 0;  // Crédito disponible para el paciente

        public ICollection<Horario> Horarios { get; set; } = new List<Horario>();

    }
}
