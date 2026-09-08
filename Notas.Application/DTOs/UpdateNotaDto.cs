using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notas.Application.DTOs
{
    public class UpdateNotaDto
    {

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [Range(typeof(decimal), "0", "5", ErrorMessage = "La nota debe estar entre 0 y 5.")]
        public decimal Valor { get; set; }
    }
}
