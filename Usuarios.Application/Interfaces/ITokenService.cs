using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Usuarios.Domain.Entities;

namespace Usuarios.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerarToken(Usuario usuario);
        DateTime ObtenerExpiracion();
    }
}
