using Microsoft.AspNetCore.Mvc;
using Usuarios.Application.DTOs;
using Usuarios.Application.Interfaces;

namespace Usuarios.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost("usuarios")]
        public async Task<IActionResult> CrearUsuario(
            [FromBody] CreateUsuarioDto dto)
        {
            var usuarioId = await _usuarioService.CrearAsync(dto);

            return Ok(new
            {
                id = usuarioId,
                mensaje = "Usuario creado correctamente."
            });
        }

        [HttpPost("registro-estudiante")]
        public async Task<IActionResult> RegistrarEstudiante([FromBody] RegistroEstudianteDto dto)
        {
            var usuarioId = await _usuarioService
                .RegistrarEstudianteAsync(dto);

            return Ok(new
            {
                id = usuarioId,
                mensaje = "Estudiante registrado correctamente."
            });
        }
    }
}
