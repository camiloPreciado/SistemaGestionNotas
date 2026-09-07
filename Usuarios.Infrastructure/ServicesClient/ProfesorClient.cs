using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Usuarios.Application.Interfaces;

namespace Usuarios.Infrastructure.ServicesClient
{
    public class ProfesorClient : IProfesorClient
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProfesorClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> CrearAsync(int usuarioId, string nombre)
        {
            var authorization = _httpContextAccessor.HttpContext?.Request.Headers.Authorization.ToString();

            if (!string.IsNullOrWhiteSpace(authorization))
            {
                _httpClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(authorization);
            }


            var response = await _httpClient.PostAsJsonAsync(
                "api/Profesores",
                new
                {
                    usuarioId,
                    nombre
                });

            return response.IsSuccessStatusCode;
        }
    }
}
