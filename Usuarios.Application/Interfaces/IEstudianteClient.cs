using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Usuarios.Application.Interfaces
{
    public interface IEstudianteClient
    {
        Task<bool> CrearAsync(int usuarioId, string nombre);
    }
}
