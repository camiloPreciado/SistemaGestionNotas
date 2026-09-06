using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Usuarios.Domain.Entities;

namespace Usuarios.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<Usuario> Usuarios => Set<Usuario>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("Usuarios");

                entity.HasKey(u => u.Id);

                entity.Property(u => u.Correo)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.HasIndex(u => u.Correo)
                    .IsUnique();

                entity.Property(u => u.ContrasenaHash)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(u => u.Rol)
                    .IsRequired()
                    .HasMaxLength(30);
            });
        }
    }
}
