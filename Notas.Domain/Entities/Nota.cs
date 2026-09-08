using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notas.Domain.Entities
{
    public class Nota
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int IdEstudiante { get; set; }
        public int IdProfesor { get; set; }
        public decimal Valor { get; set; }
    }
}
