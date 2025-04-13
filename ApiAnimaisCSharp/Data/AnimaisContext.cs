using Microsoft.EntityFrameworkCore;
using ApiAnimais.Models;

namespace ApiAnimais.Data
{
    public class AnimalContext : DbContext
    {
        public DbSet<AnimalModel> Animais { get; set; }
        public DbSet<CuidadoModel> Cuidados { get; set; }
        public DbSet<AnimalCuidado> AnimaisCuidados { get; set; }

        public AnimalContext(DbContextOptions<AnimalContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AnimalCuidado>()
                .HasKey(ac => new { ac.AnimalId, ac.CuidadoId });

            modelBuilder.Entity<AnimalCuidado>()
                .HasOne(ac => ac.Animal)
                .WithMany(a => a.Cuidados)
                .HasForeignKey(ac => ac.AnimalId)
                .OnDelete(DeleteBehavior.Cascade); 
            modelBuilder.Entity<AnimalCuidado>()
                .HasOne(ac => ac.Cuidado)
                .WithMany(c => c.Animais)
                .HasForeignKey(ac => ac.CuidadoId)
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<AnimalModel>().Property(a => a.Nome).IsRequired();
            modelBuilder.Entity<CuidadoModel>().Property(c => c.Nome).IsRequired();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=animal.sqlite");
            }
        }
    }
}
