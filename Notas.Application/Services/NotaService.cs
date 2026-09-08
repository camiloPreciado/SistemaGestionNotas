using Microsoft.Extensions.Logging;
using Notas.Application.Common;
using Notas.Application.DTOs;
using Notas.Application.Interfaces;
using Notas.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notas.Application.Services
{
    public class NotaService : INotaService
    {
        private readonly INotaRepository _repository;
        private readonly IEstudianteClient _estudianteClient;
        private readonly IProfesorClient _profesorClient;
        private readonly ILogger<NotaService> _logger;

        public NotaService(
            INotaRepository repository,
            IEstudianteClient estudianteClient,
            IProfesorClient profesorClient,
            ILogger<NotaService> logger)
        {
            _repository = repository;
            _estudianteClient = estudianteClient;
            _profesorClient = profesorClient;
            _logger = logger;
        }

        public async Task<Paginacion<NotaDto>> GetAllAsync(int page, int pageSize)
        {
            if (page < 1)
                throw new ValidationException(
                    "La página debe ser mayor o igual a 1.");

            if (pageSize < 1 || pageSize > 100)
                throw new ValidationException(
                    "El tamaño de página debe estar entre 1 y 100.");

            return await _repository.GetAllAsync(
                page,
                pageSize);

        }


        public async Task<Paginacion<NotaDto>> GetByEstudianteAsync(int idEstudiante, int page, int pageSize)
        {
            if (page < 1)
                throw new ValidationException(
                    "La página debe ser mayor o igual a 1.");

            if (pageSize < 1 || pageSize > 100)
                throw new ValidationException(
                    "El tamaño de página debe estar entre 1 y 100.");

            return await _repository.GetByEstudianteAsync(
                idEstudiante,
                page,
                pageSize);
        }

        public async Task<NotaDto?> GetByIdAsync(int id)
        {
            var nota = await _repository.GetByIdAsync(id);

            if (nota is null)
                return null;

            return new NotaDto
            {
                Id = nota.Id,
                Nombre = nota.Nombre,
                IdEstudiante = nota.IdEstudiante,
                IdProfesor = nota.IdProfesor,
                Valor = nota.Valor
            };
        }

        public async Task<NotaDto> CreateAsync(CreateNotaDto dto)
        {
            var estudianteExiste =
                await _estudianteClient.ExisteAsync(
                    dto.IdEstudiante);

            if (!estudianteExiste)
            {
                _logger.LogWarning("No se pudo crear la nota porque el estudiante no existe. IdEstudiante: {IdEstudiante}", dto.IdEstudiante);
                throw new ValidationException("El estudiante especificado no existe.");
            }

            var idProfesor =
                await _profesorClient.ObtenerIdProfesorAsync();

            if (idProfesor is null)
            {
                _logger.LogWarning("No se pudo obtener el profesor asociado al usuario autenticado.");
                throw new ValidationException("No se pudo obtener el profesor autenticado.");
            }

            var nota = new Nota
            {
                Nombre = dto.Nombre,
                IdEstudiante = dto.IdEstudiante,
                IdProfesor = idProfesor.Value,
                Valor = dto.Valor
            };

            var created =
                await _repository.CreateAsync(nota);

            _logger.LogInformation("Nota creada correctamente. IdNota: {IdNota}, IdEstudiante: {IdEstudiante}, IdProfesor: {IdProfesor}, Valor: {Valor}",
                    created.Id,
                    created.IdEstudiante,
                    created.IdProfesor,
                    created.Valor);

            return new NotaDto
            {
                Id = created.Id,
                Nombre = created.Nombre,
                IdEstudiante = created.IdEstudiante,
                IdProfesor = created.IdProfesor,
                Valor = created.Valor
            };
        }

        public async Task<bool> UpdateAsync(int id, UpdateNotaDto dto)
        {
            var nota = await _repository.GetByIdAsync(id);

            if (nota is null)
            {
                _logger.LogWarning("No se pudo actualizar la nota porque no existe. IdNota: {IdNota}", id);
                return false;
            };

            nota.Nombre = dto.Nombre;
            nota.Valor = dto.Valor;

            await _repository.UpdateAsync(nota);

            return true;
        }


        public async Task<bool> DeleteAsync(int id)
        {
            var nota = await _repository.GetByIdAsync(id);

            if (nota is null)
            {
                _logger.LogWarning("No se pudo eliminar la nota porque no existe. IdNota: {IdNota}",id);
                return false;
            };

            await _repository.DeleteAsync(nota);

            return true;
        }
    }
}
