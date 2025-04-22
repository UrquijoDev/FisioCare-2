using Microsoft.AspNetCore.Identity;

namespace FisioCare_2.Models
{
    public class ApplicationUser: IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string ImageFileName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
