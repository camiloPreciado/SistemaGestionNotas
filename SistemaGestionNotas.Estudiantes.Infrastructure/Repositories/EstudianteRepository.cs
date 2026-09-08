using Estudiantes.Application.Common;
using Estudiantes.Application.Interfaces;
using Estudiantes.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using SistemaGestionNotas.Estudiantes.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estudiantes.Infrastructure.Repositories
{
    public class EstudianteRepository : IEstudianteRepository   
    {
        private readonly ApplicationDbContext _context;

        public EstudianteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Paginacion<Estudiante>> GetAllAsync(int page, int pageSize)
        {
            var query = _context.Estudiantes
                .AsNoTracking()
                .OrderBy(e => e.Id);

            var totalItems = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new Paginacion<Estudiante>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems
            };
        }

        public async Task<Estudiante?> GetByIdAsync(int id)
        {
            return await _context.Estudiantes
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Estudiante> CreateAsync(Estudiante estudiante)
        {
            await _context.Estudiantes.AddAsync(estudiante);
            await _context.SaveChangesAsync();

            return estudiante;
        }

        public async Task UpdateAsync(Estudiante estudiante)
        {
            _context.Estudiantes.Update(estudiante);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Estudiante estudiante)
        {
            _context.Estudiantes.Remove(estudiante);

            await _context.SaveChangesAsync();
        }

        public async Task<Estudiante?> GetByUsuarioIdAsync(int usuarioId)
        {
            return await _context.Estudiantes
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.UsuarioId == usuarioId);
        }
    }
}
