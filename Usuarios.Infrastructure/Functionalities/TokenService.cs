using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Usuarios.Application.Interfaces;
using Usuarios.Domain.Entities;

namespace Usuarios.Infrastructure.Functionalities
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerarToken(Usuario usuario)
        {
            var key = _configuration["Jwt:Key"]
                ?? throw new InvalidOperationException(
                    "La clave JWT no está configurada.");

            var issuer = _configuration["Jwt:Issuer"]
                ?? throw new InvalidOperationException(
                    "El issuer JWT no está configurado.");

            var expirationMinutes = _configuration
                .GetValue<int>("Jwt:ExpirationMinutes");

            var expiration = DateTime.UtcNow
                .AddMinutes(expirationMinutes);

            var claims = new[]
            {
            new Claim(
                JwtRegisteredClaimNames.Sub,
                usuario.Id.ToString()),

            new Claim(
                JwtRegisteredClaimNames.Email,
                usuario.Correo),

            new Claim(
                ClaimTypes.Role,
                usuario.Rol)
        };

            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(key));

            var credentials = new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: issuer,
                claims: claims,
                expires: expiration,
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }

        public DateTime ObtenerExpiracion()
        {
            var expirationMinutes = _configuration
                .GetValue<int>("Jwt:ExpirationMinutes");

            return DateTime.UtcNow
                .AddMinutes(expirationMinutes);
        }
    }
}
