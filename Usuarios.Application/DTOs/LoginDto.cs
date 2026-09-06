using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Usuarios.Application.DTOs
{
    public class LoginDto
    {
        public string Correo { get; set; } = string.Empty;
        public string Contrasenia { get; set; } = string.Empty;
    }
}
