using Microsoft.EntityFrameworkCore;
using Notas.Application.Common;
using Notas.Application.DTOs;
using Notas.Application.Interfaces;
using Notas.Domain.Entities;
using Notas.Infraestructure.Persistence;
using Notas.Infraestructure.Persistence.References;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notas.Infraestructure.Repositories
{
    public class NotaRepository : INotaRepository
    {
        private readonly ApplicationDbContext _context;

        public NotaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Paginacion<NotaDto>> GetAllAsync(int page, int pageSize)
        {
            var query =
                from nota in _context.Notas.AsNoTracking()
                join estudiante in _context.Estudiantes.AsNoTracking()
                    on nota.IdEstudiante equals estudiante.Id
                join profesor in _context.Profesores.AsNoTracking()
                    on nota.IdProfesor equals profesor.Id
                orderby nota.Id
                select new NotaDto
                {
                    Id = nota.Id,
                    Nombre = nota.Nombre,
                    IdEstudiante = nota.IdEstudiante,
                    NombreEstudiante = estudiante.Nombre,
                    IdProfesor = nota.IdProfesor,
                    NombreProfesor = profesor.Nombre,
                    Valor = nota.Valor
                };

            var totalItems = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new Paginacion<NotaDto>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems
            };
        }

        public async Task<Paginacion<NotaDto>> GetByEstudianteAsync(int idEstudiante, int page, int pageSize)
        {
            var query =
                from nota in _context.Notas.AsNoTracking()
                join estudiante in _context.Estudiantes.AsNoTracking()
                    on nota.IdEstudiante equals estudiante.Id
                join profesor in _context.Profesores.AsNoTracking()
                    on nota.IdProfesor equals profesor.Id
                where nota.IdEstudiante == idEstudiante
                orderby nota.Id
                select new NotaDto
                {
                    Id = nota.Id,
                    Nombre = nota.Nombre,
                    IdEstudiante = nota.IdEstudiante,
                    NombreEstudiante = estudiante.Nombre,
                    IdProfesor = nota.IdProfesor,
                    NombreProfesor = profesor.Nombre,
                    Valor = nota.Valor
                };

            var totalItems = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new Paginacion<NotaDto>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems
            };
        }

        public async Task<Nota?> GetByIdAsync(int id)
        {
            return await _context.Notas
                .FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task<Nota> CreateAsync(Nota nota)
        {
            await _context.Notas.AddAsync(nota);

            await _context.SaveChangesAsync();

            return nota;
        }

        public async Task UpdateAsync(Nota nota)
        {
            _context.Notas.Update(nota);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Nota nota)
        {
            _context.Notas.Remove(nota);

            await _context.SaveChangesAsync();
        }
    }
}
