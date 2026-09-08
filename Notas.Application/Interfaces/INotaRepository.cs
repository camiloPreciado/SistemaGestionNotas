using Notas.Domain.Entities;
using Notas.Application.Common;
using Notas.Application.DTOs;


namespace Notas.Application.Interfaces
{
    public interface INotaRepository
    {
        Task<Paginacion<NotaDto>> GetAllAsync(int page, int pageSize);
        Task<Paginacion<NotaDto>> GetByEstudianteAsync(int idEstudiante, int page, int pageSize);
        Task<Nota?> GetByIdAsync(int id);
        Task<Nota> CreateAsync(Nota nota);
        Task UpdateAsync(Nota nota);
        Task DeleteAsync(Nota nota);
    }
}
