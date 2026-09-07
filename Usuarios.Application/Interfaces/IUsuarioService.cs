using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Usuarios.Application.DTOs;

namespace Usuarios.Application.Interfaces
{
    public interface IUsuarioService
    {
        Task<bool> ExisteCorreoAsync(string correo);
        Task<int> CrearAsync(CreateUsuarioDto dto);
        Task<int> RegistrarEstudianteAsync(RegistroEstudianteDto dto);
        Task<int> RegistrarProfesorAsync(RegistroProfesorDto dto);
    }
}
