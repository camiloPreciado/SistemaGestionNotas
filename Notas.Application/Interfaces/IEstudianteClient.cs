using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notas.Application.Interfaces
{
    public interface IEstudianteClient
    {
        Task<bool> ExisteAsync(int idEstudiante);
        Task<int?> ObtenerIdEstudianteAsync();
    }
}
