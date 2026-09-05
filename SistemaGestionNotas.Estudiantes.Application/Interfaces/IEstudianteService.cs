using Estudiantes.Application.Common;
using Estudiantes.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estudiantes.Application.Interfaces
{
    public interface IEstudianteService
    {
        Task<Paginacion<EstudianteDto>> GetAllAsync(int page, int pageSize);
        Task<EstudianteDto?> GetByIdAsync(int id);
        Task<EstudianteDto> CreateAsync(CreateEstudianteDto dto);
        Task<bool> UpdateAsync(int id, UpdateEstudianteDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
