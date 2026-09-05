using Estudiantes.Application.Common;
using SistemaGestionNotas.Estudiantes.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estudiantes.Application.Interfaces
{
    public interface IEstudianteRepository
    {
        Task<Paginacion<Estudiante>> GetAllAsync(int page, int pageSize);
        Task<Estudiante?> GetByIdAsync(int id);
        Task<Estudiante> CreateAsync(Estudiante estudiante);
        Task UpdateAsync(Estudiante estudiante);
        Task DeleteAsync(Estudiante estudiante);
    }
}
