using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<EstudianteClient> _logger;

        public EstudianteClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, ILogger<EstudianteClient> logger)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<bool> ExisteAsync(int idEstudiante)
        {
            var authorization = _httpContextAccessor.HttpContext?.Request.Headers.Authorization.ToString();

            if (!string.IsNullOrWhiteSpace(authorization))
            {
                _httpClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(authorization);
            }

            var response = await _httpClient.GetAsync($"api/Estudiantes/{idEstudiante}");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Estudiantes.API respondió con estado {StatusCode} al consultar el estudiante {IdEstudiante}.", (int)response.StatusCode, idEstudiante);
                return false;
            }

            _logger.LogInformation("Estudiante validado correctamente desde Estudiantes.API. IdEstudiante: {IdEstudiante}", idEstudiante);
            return response.IsSuccessStatusCode;
        }

        public async Task<int?> ObtenerIdEstudianteAsync()
        {
            AgregarToken();

            var response = await _httpClient.GetAsync("api/Estudiantes/ObtenerIdEstudiante");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("No fue posible obtener el estudiante autenticado desde Estudiantes.API. StatusCode: {StatusCode}", (int)response.StatusCode);
                return null;
            }

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
