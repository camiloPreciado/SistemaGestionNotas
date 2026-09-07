using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Usuarios.Application.DTOs
{
    public class RegistroProfesorDto
    {
        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string Correo { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Contrasena { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Nombre { get; set; } = string.Empty;
    }
}
