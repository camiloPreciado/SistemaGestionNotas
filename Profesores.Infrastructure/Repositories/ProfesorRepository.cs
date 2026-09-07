using Microsoft.EntityFrameworkCore;
using Profesores.Application.Common;
using Profesores.Application.Interfaces;
using Profesores.Domain.Entities;
using Profesores.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profesores.Infrastructure.Repositories
{
    public class ProfesorRepository : IProfesorRepository
    {
        private readonly ApplicationDbContext _context;

        public ProfesorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Paginacion<Profesor>> GetAllAsync(int page, int pageSize)
        {
            var query = _context.Profesores
                .AsNoTracking()
                .OrderBy(e => e.Id);

            var totalItems = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new Paginacion<Profesor>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems
            };
        }

        public async Task<Profesor?> GetByIdAsync(int id)
        {
            return await _context.Profesores
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Profesor> CreateAsync(Profesor profesor)
        {
            await _context.Profesores.AddAsync(profesor);
            await _context.SaveChangesAsync();

            return profesor;
        }

        public async Task UpdateAsync(Profesor profesor)
        {
            _context.Profesores.Update(profesor);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Profesor profesor)
        {
            _context.Profesores.Remove(profesor);

            await _context.SaveChangesAsync();
        }
    }
}
