using Microsoft.AspNetCore.Http;
using Notas.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Notas.Infraestructure.ServicesClient
{
    public class EstudianteClient : IEstudianteClient
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EstudianteClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> ExisteAsync(int idEstudiante)
        {
            var authorization = _httpContextAccessor.HttpContext?.Request.Headers.Authorization.ToString();

            if (!string.IsNullOrWhiteSpace(authorization))
            {
                _httpClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(authorization);
            }

            var response = await _httpClient.GetAsync($"api/Estudiantes/{idEstudiante}");

            return response.IsSuccessStatusCode;
        }

        public async Task<int?> ObtenerIdEstudianteAsync()
        {
            AgregarToken();

            var response = await _httpClient.GetAsync(
                "api/Estudiantes/ObtenerIdEstudiante");

            if (!response.IsSuccessStatusCode)
                return null;

            var estudiante = await response.Content
                .ReadFromJsonAsync<EstudianteResponse>();

            return estudiante?.Id;
        }

        private void AgregarToken()
        {
            var token = _httpContextAccessor
                .HttpContext?
                .Request.Headers.Authorization
                .ToString();

            if (!string.IsNullOrWhiteSpace(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    AuthenticationHeaderValue.Parse(token);
            }
        }

        private class EstudianteResponse
        {
            public int Id { get; set; }
        }
    }
}
