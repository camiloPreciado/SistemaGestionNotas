using Profesores.Application.Common;
using Profesores.Application.DTOs;
using Profesores.Application.Interfaces;
using Profesores.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profesores.Application.Services
{
    public class ProfesorService : IProfesorService
    {
        private readonly IProfesorRepository _repository;

        public ProfesorService(IProfesorRepository repository)
        {
            _repository = repository;
        }

        public async Task<Paginacion<ProfesorDto>> GetAllAsync(int page, int pageSize)
        {
            if (page < 1) { throw new ValidationException("La página debe ser mayor o igual a 1."); }
            ;

            if (pageSize < 1 || pageSize > 100) { throw new ValidationException("El tamaño de página debe estar entre 1 y 100."); }
            ;

            var result = await _repository.GetAllAsync(page, pageSize);

            return new Paginacion<ProfesorDto>
            {
                Items = result.Items.Select(e => new ProfesorDto
                {
                    Id = e.Id,
                    UsuarioId = e.UsuarioId,
                    Nombre = e.Nombre
                }),
                Page = result.Page,
                PageSize = result.PageSize,
                TotalItems = result.TotalItems
            };
        }

        public async Task<ProfesorDto?> GetByIdAsync(int id)
        {
            var profesor = await _repository.GetByIdAsync(id);

            if (profesor is null) { return null; }
            ;

            return new ProfesorDto
            {
                Id = profesor.Id,
                UsuarioId = profesor.UsuarioId,
                Nombre = profesor.Nombre
            };
        }

        public async Task<ProfesorDto> CreateAsync(CreateProfesorDto dto)
        {
            var profesor = new Profesor
            {
                UsuarioId = dto.UsuarioId,
                Nombre = dto.Nombre
            };

            var created = await _repository.CreateAsync(profesor);

            return new ProfesorDto
            {
                Id = created.Id,
                UsuarioId = created.UsuarioId,
                Nombre = created.Nombre
            };
        }

        public async Task<bool> UpdateAsync(int id, UpdateProfesorDto dto)
        {
            var profesor = await _repository.GetByIdAsync(id);

            if (profesor is null) { return false; }
            ;

            profesor.Nombre = dto.Nombre;

            await _repository.UpdateAsync(profesor);

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var profesor = await _repository.GetByIdAsync(id);

            if (profesor is null) { return false; }
            ;

            await _repository.DeleteAsync(profesor);

            return true;
        }

        public async Task<ProfesorDto?> GetByUsuarioIdAsync(int usuarioId)
        {
            var profesor = await _repository.GetByUsuarioIdAsync(usuarioId);

            if (profesor is null)
                return null;

            return new ProfesorDto
            {
                Id = profesor.Id,
                UsuarioId = profesor.UsuarioId,
                Nombre = profesor.Nombre
            };
        }
    }
}
