using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Usuarios.Application.DTOs;
using Usuarios.Application.Interfaces;
using Usuarios.Application.Services;

namespace Usuarios.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly UsuarioService _usuarioServiceLogin;

        public UsuarioController(IUsuarioService usuarioService, UsuarioService usuarioServiceLogin)
        {
            _usuarioService = usuarioService;
            _usuarioServiceLogin = usuarioServiceLogin;
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

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var resultado = await _usuarioServiceLogin.LoginAsync(dto);

            if (resultado is null)
                return Unauthorized(new
                {
                    mensaje = "Correo o contraseña incorrectos."
                });

            return Ok(resultado);
        }

        //[Authorize]
        //[HttpGet("protegido")]
        //public IActionResult Protegido()
        //{
        //    return Ok(new
        //    {
        //        mensaje = "Tienes acceso al endpoint protegido."
        //    });
        //}
    }
}
