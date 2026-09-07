using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Usuarios.Application.DTOs;
using Usuarios.Application.Interfaces;
using Usuarios.Application.Services;
using Usuarios.Domain.Constants;

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

        [Authorize(Roles = Roles.Admin)]
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


        [Authorize(Roles = Roles.Admin)]
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


        [Authorize(Roles = Roles.Admin)]
        [HttpPost("registro-profesor")]
        public async Task<IActionResult> RegistrarProfesor([FromBody] RegistroProfesorDto dto)
        {
            var usuarioId = await _usuarioService
                .RegistrarProfesorAsync(dto);

            return Ok(new
            {
                id = usuarioId,
                mensaje = "Profesor registrado correctamente."
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
