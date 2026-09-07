using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profesores.Application.DTOs
{
    public class ProfesorDto
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }
}
