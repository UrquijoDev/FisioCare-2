using FisioCare_2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FisioCare_2.Services
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var admin = new IdentityRole
            {
                Id = "b98be2a3-d36f-432f-a733-c88a2e4f8579",
                Name = "Admin",
                NormalizedName = "ADMIN"
            };

            var paciente = new IdentityRole
            {
                Id = "f41a4034-6635-4f4f-94d0-9c8fd20f2136",
                Name = "Paciente",
                NormalizedName = "PACIENTE"
            };

            var fisioterapeuta = new IdentityRole
            {
                Id = "35e75c8e-94d5-442a-8de1-16b11fa58f1a",
                Name = "Fisioterapeuta",
                NormalizedName = "FISIOTERAPEUTA"
            };

            var recepcionista = new IdentityRole
            {
                Id = "16fc874e-773d-48ff-a4c2-4b1cb2f25b69",
                Name = "Recepcionista",
                NormalizedName = "RECEPCIONISTA"
            };

            builder.Entity<IdentityRole>().HasData(admin, paciente, fisioterapeuta, recepcionista);

        }
    }
}
