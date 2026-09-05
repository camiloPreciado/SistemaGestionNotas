using Estudiantes.Application.Common;
using Estudiantes.Application.DTOs;
using Estudiantes.Application.Interfaces;
using SistemaGestionNotas.Estudiantes.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estudiantes.Application.Services
{
    public class EstudianteService : IEstudianteService
    {
        private readonly IEstudianteRepository _repository;

        public EstudianteService(IEstudianteRepository repository) { 
            _repository = repository;
        }

        public async Task<Paginacion<EstudianteDto>> GetAllAsync( int page, int pageSize)
        {
            if (page < 1) { throw new ValidationException("La página debe ser mayor o igual a 1."); };

            if (pageSize < 1 || pageSize > 100) { throw new ValidationException("El tamaño de página debe estar entre 1 y 100."); };

            var result = await _repository.GetAllAsync(page, pageSize);

            return new Paginacion<EstudianteDto>
            {
                Items = result.Items.Select(e => new EstudianteDto
                {
                    Id = e.Id,
                    Nombre = e.Nombre
                }),
                Page = result.Page,
                PageSize = result.PageSize,
                TotalItems = result.TotalItems
            };
        }

        public async Task<EstudianteDto?> GetByIdAsync(int id)
        {
            var estudiante = await _repository.GetByIdAsync(id);

            if (estudiante is null) { return null; };

            return new EstudianteDto
            {
                Id = estudiante.Id,
                Nombre = estudiante.Nombre
            };
        }

        public async Task<EstudianteDto> CreateAsync(CreateEstudianteDto dto)
        {
            var estudiante = new Estudiante
            {
                Nombre = dto.Nombre
            };

            var created = await _repository.CreateAsync(estudiante);

            return new EstudianteDto
            {
                Id = created.Id,
                Nombre = created.Nombre
            };
        }

        public async Task<bool> UpdateAsync(int id, UpdateEstudianteDto dto)
        {
            var estudiante = await _repository.GetByIdAsync(id);

            if (estudiante is null) { return false; };

            estudiante.Nombre = dto.Nombre;

            await _repository.UpdateAsync(estudiante);

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var estudiante = await _repository.GetByIdAsync(id);

            if (estudiante is null) { return false; };

            await _repository.DeleteAsync(estudiante);

            return true;
        }
    }
}

