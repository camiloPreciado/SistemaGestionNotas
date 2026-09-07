using Profesores.Application.Common;
using Profesores.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profesores.Application.Interfaces
{
    public interface IProfesorRepository
    {
        Task<Paginacion<Profesor>> GetAllAsync(int page, int pageSize);
        Task<Profesor?> GetByIdAsync(int id);
        Task<Profesor> CreateAsync(Profesor profesor);
        Task UpdateAsync(Profesor profesor);
        Task DeleteAsync(Profesor profesor);
    }
}
