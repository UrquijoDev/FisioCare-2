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
        public DbSet<Horario> Horarios { get; set; }
        public DbSet<PaqueteCredito> PaquetesCredito { get; set; }
        public DbSet<Servicio> Servicio { get; set; }
        public DbSet<TransaccionCredito> TransaccionCredito { get; set; }
        public DbSet<Cita> Cita {  get; set; }

        public DbSet<Feature> Feature { get; set; }
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

            // Relación UsuarioId con no eliminación en cascada
            builder.Entity<Cita>()
                .HasOne(c => c.Usuario)
                .WithMany()
                .HasForeignKey(c => c.UsuarioId)
                .OnDelete(DeleteBehavior.NoAction);

            // Relación FisioterapeutaId con no eliminación en cascada
            builder.Entity<Cita>()
                .HasOne(c => c.Fisioterapeuta)
                .WithMany()
                .HasForeignKey(c => c.FisioterapeutaId)
                .OnDelete(DeleteBehavior.NoAction);

            // Relación ServicioId con eliminación en cascada
            builder.Entity<Cita>()
                .HasOne(c => c.Servicio)
                .WithMany()
                .HasForeignKey(c => c.ServicioId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    
    
    
    }
}
