using BCrypt.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Usuarios.Application.DTOs;
using Usuarios.Application.Interfaces;
using Usuarios.Domain.Constants;
using Usuarios.Domain.Entities;
using Usuarios.Application.Common;

namespace Usuarios.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ITokenService _tokenService;
        private readonly IEstudianteClient _estudianteClient;

        public UsuarioService(
            IUsuarioRepository usuarioRepository,
            ITokenService tokenService,
            IEstudianteClient estudianteClient)
        {
            _usuarioRepository = usuarioRepository;
            _tokenService = tokenService;
            _estudianteClient = estudianteClient;
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
                throw new ValidationException(
                    "El rol especificado no es válido.");
            }

            var correoExiste = await ExisteCorreoAsync(dto.Correo);

            if (correoExiste)
            {
                throw new ValidationException(
                    "Ya existe un usuario con ese correo.");
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

            return usuarioCreado.Id;
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginDto dto)
        {
            var usuario = await _usuarioRepository
                .GetByCorreoAsync(dto.Correo);

            if (usuario is null)
                return null;

            var contraseniaValida = BCrypt.Net.BCrypt.Verify(
                dto.Contrasenia,
                usuario.ContrasenaHash);

            if (!contraseniaValida)
                return null;

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
                throw new ArgumentException(
                    "Ya existe un usuario con ese correo.");
            }

            var usuario = new Usuario
            {
                Correo = dto.Correo,
                ContrasenaHash = BCrypt.Net.BCrypt.HashPassword(
                    dto.Contrasena),
                Rol = Roles.Estudiante
            };

            var usuarioCreado = await _usuarioRepository
                .CreateAsync(usuario);

            var estudianteCreado = await _estudianteClient.CrearAsync(
                usuarioCreado.Id,
                dto.Nombre);

            if (!estudianteCreado)
            {
                throw new InvalidOperationException(
                    "No fue posible crear el estudiante.");
            }

            return usuarioCreado.Id;
        }
    }
}
