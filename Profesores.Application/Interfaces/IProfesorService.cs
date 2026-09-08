using Profesores.Application.Common;
using Profesores.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profesores.Application.Interfaces
{
    public interface IProfesorService
    {
        Task<Paginacion<ProfesorDto>> GetAllAsync(int page, int pageSize);
        Task<ProfesorDto?> GetByIdAsync(int id);
        Task<ProfesorDto> CreateAsync(CreateProfesorDto dto);
        Task<bool> UpdateAsync(int id, UpdateProfesorDto dto);
        Task<bool> DeleteAsync(int id);
        Task<ProfesorDto?> GetByUsuarioIdAsync(int usuarioId);
    }
}
