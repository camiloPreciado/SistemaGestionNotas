using Notas.Application.Common;
using Notas.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notas.Application.Interfaces
{
    public interface INotaService
    {
        Task<Paginacion<NotaDto>> GetAllAsync(int page, int pageSize);
        Task<Paginacion<NotaDto>> GetByEstudianteAsync(int idEstudiante, int page, int pageSize);
        Task<NotaDto?> GetByIdAsync(int id);
        Task<NotaDto> CreateAsync(CreateNotaDto dto);
        Task<bool> UpdateAsync(int id, UpdateNotaDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
