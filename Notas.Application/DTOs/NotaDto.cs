using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notas.Application.DTOs
{
    public class NotaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int IdEstudiante { get; set; }
        public string NombreEstudiante { get; set; } = string.Empty;
        public int IdProfesor { get; set; }
        public string NombreProfesor { get; set; } = string.Empty;
        public decimal Valor { get; set; }
    }
}
