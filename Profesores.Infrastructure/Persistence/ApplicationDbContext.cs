using Microsoft.EntityFrameworkCore;
using Profesores.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profesores.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Profesor> Profesores => Set<Profesor>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Profesor>(entity =>
            {
                entity.ToTable("Profesores");

                entity.HasKey(e => e.Id);

                entity.HasIndex(e => e.UsuarioId)
                    .IsUnique();

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(100);
            });
        }
    }
}
