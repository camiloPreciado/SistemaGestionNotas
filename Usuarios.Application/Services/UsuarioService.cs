using BCrypt.Net;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Usuarios.Application.Common;
using Usuarios.Application.DTOs;
using Usuarios.Application.Interfaces;
using Usuarios.Domain.Constants;
using Usuarios.Domain.Entities;

namespace Usuarios.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ITokenService _tokenService;
        private readonly IEstudianteClient _estudianteClient;
        private readonly IProfesorClient _profesorClient;
        private readonly ILogger<UsuarioService> _logger;

        public UsuarioService(
            IUsuarioRepository usuarioRepository,
            ITokenService tokenService,
            IEstudianteClient estudianteClient,
            IProfesorClient profesorClient,
            ILogger<UsuarioService> logger)
        {
            _usuarioRepository = usuarioRepository;
            _tokenService = tokenService;
            _estudianteClient = estudianteClient;
            _profesorClient = profesorClient;
            _logger = logger;
        }

        public async Task<bool> ExisteCorreoAsync(string correo)
        {
            var usuario = await _usuarioRepository
                .GetByCorreoAsync(correo);

            return usuario is not null;
        }

        public async Task<int> CrearAsync(CreateUsuarioDto dto)
        {
            if (dto.Rol != Roles.Estudiante &&
                dto.Rol != Roles.Profesor &&
                dto.Rol != Roles.Admin)
            {
                _logger.LogWarning("Intento de crear usuario con rol inexistente: {Rol}", dto.Rol);
                throw new ValidationException("El rol especificado no es válido.");
            }

            var correoExiste = await ExisteCorreoAsync(dto.Correo);

            if (correoExiste)
            {
                _logger.LogWarning("Intento de crear usuario con correo existente: {Correo}", dto.Correo);
                throw new ValidationException("Ya existe un usuario con ese correo.");
            }

            var usuario = new Usuario
            {
                Correo = dto.Correo,
                ContrasenaHash = BCrypt.Net.BCrypt.HashPassword(
                    dto.Contrasena),
                Rol = dto.Rol
            };

            var usuarioCreado = await _usuarioRepository
                .CreateAsync(usuario);

            _logger.LogInformation("Usuario creado correctamente. UsuarioId: {UsuarioId}, Rol: {Rol}", usuarioCreado.Id, usuarioCreado.Rol);

            return usuarioCreado.Id;
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginDto dto)
        {
            var usuario = await _usuarioRepository
                .GetByCorreoAsync(dto.Correo);

            if (usuario is null)
            {
                _logger.LogWarning("Intento de inicio de sesión fallido. Correo no encontrado: {Correo}", dto.Correo);
                return null;
            }

            var contrasenaValida = BCrypt.Net.BCrypt.Verify(
                dto.Contrasena,
                usuario.ContrasenaHash);

            if (!contrasenaValida)
            {
                _logger.LogWarning("Intento de inicio de sesión fallido. Contraseña incorrecta para el correo: {Correo}", dto.Correo);
                return null;
            }

            _logger.LogInformation("Inicio de sesión exitoso. UsuarioId: {UsuarioId}, Rol: {Rol}", usuario.Id, usuario.Rol);

            return new LoginResponseDto
            {
                Token = _tokenService.GenerarToken(usuario),
                Expiracion = _tokenService.ObtenerExpiracion(),
                Rol = usuario.Rol
            };
        }

        public async Task<int> RegistrarEstudianteAsync(RegistroEstudianteDto dto)
        {
            if (await ExisteCorreoAsync(dto.Correo))
            {
                throw new ValidationException("Ya existe un usuario con ese correo.");
            }

            var usuario = new Usuario
            {
                Correo = dto.Correo,
                ContrasenaHash = BCrypt.Net.BCrypt.HashPassword(
                    dto.Contrasena),
                Rol = Roles.Estudiante
            };

            var usuarioCreado = await _usuarioRepository.CreateAsync(usuario);
            _logger.LogInformation("Usuario para estudiante creado. UsuarioId: {UsuarioId}", usuarioCreado.Id);


            var estudianteCreado = await _estudianteClient.CrearAsync(usuarioCreado.Id, dto.Nombre);

            if (!estudianteCreado)
            {
                _logger.LogError("No fue posible crear el estudiante asociado al UsuarioId: {UsuarioId}", usuarioCreado.Id);
                throw new InvalidOperationException("No fue posible crear el estudiante.");
            }

            _logger.LogInformation("Estudiante registrado correctamente. UsuarioId: {UsuarioId}", usuarioCreado.Id);
            return usuarioCreado.Id;
        }

        public async Task<int> RegistrarProfesorAsync(RegistroProfesorDto dto)
        {
            if (await ExisteCorreoAsync(dto.Correo))
            {
                throw new ValidationException("Ya existe un usuario con ese correo.");
            }

            var usuario = new Usuario
            {
                Correo = dto.Correo,
                ContrasenaHash = BCrypt.Net.BCrypt.HashPassword(
                    dto.Contrasena),
                Rol = Roles.Profesor
            };

            var usuarioCreado = await _usuarioRepository.CreateAsync(usuario);
            _logger.LogInformation("Usuario para profesor creado. UsuarioId: {UsuarioId}", usuarioCreado.Id);


            var profesorCreado = await _profesorClient.CrearAsync(usuarioCreado.Id, dto.Nombre);

            if (!profesorCreado)
            {
                _logger.LogError("No fue posible crear el profesor asociado al UsuarioId: {UsuarioId}", usuarioCreado.Id);
                throw new InvalidOperationException("No fue posible crear el profesor.");
            }

            _logger.LogInformation("Profesor registrado correctamente. UsuarioId: {UsuarioId}", usuarioCreado.Id);
            return usuarioCreado.Id;
        }
    }
}
