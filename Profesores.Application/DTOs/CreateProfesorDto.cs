using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profesores.Application.DTOs
{
    public class CreateProfesorDto
    {
        [Required]
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;
    }
}
