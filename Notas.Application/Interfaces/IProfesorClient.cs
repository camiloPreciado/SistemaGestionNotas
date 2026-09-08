using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notas.Application.Interfaces
{
    public interface IProfesorClient
    {
        Task<bool> ExisteAsync(int idProfesor);
        Task<int?> ObtenerIdProfesorAsync();
    }
}
