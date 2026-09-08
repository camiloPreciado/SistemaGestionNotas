using Microsoft.EntityFrameworkCore;
using Notas.Domain.Entities;
using Notas.Infraestructure.Persistence.References;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notas.Infraestructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Nota> Notas => Set<Nota>();

        public DbSet<EstudianteReferencia> Estudiantes => Set<EstudianteReferencia>();

        public DbSet<ProfesorReferencia> Profesores => Set<ProfesorReferencia>();

        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Nota>(entity =>
            {
                entity.ToTable("Notas");

                entity.HasKey(n => n.Id);

                entity.Property(n => n.Nombre)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(n => n.IdEstudiante)
                    .IsRequired();

                entity.Property(n => n.IdProfesor)
                    .IsRequired();

                entity.Property(n => n.Valor)
                    .IsRequired()
                    .HasPrecision(3, 2);
            });

            modelBuilder.Entity<EstudianteReferencia>(entity =>
            {
                entity.ToTable("Estudiantes");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<ProfesorReferencia>(entity =>
            {
                entity.ToTable("Profesores");

                entity.HasKey(p => p.Id);

                entity.Property(p => p.Nombre)
                    .IsRequired()
                    .HasMaxLength(100);
            });
        }
    }
}
