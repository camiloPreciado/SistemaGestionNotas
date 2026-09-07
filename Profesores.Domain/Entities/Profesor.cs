using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profesores.Domain.Entities
{
    public class Profesor
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }
}
