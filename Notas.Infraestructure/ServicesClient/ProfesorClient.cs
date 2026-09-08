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
    public class ProfesorClient : IProfesorClient
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<EstudianteClient> _logger;

        public ProfesorClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, ILogger<EstudianteClient> logger)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<bool> ExisteAsync(int idProfesor)
        {
            var authorization = _httpContextAccessor.HttpContext?.Request.Headers.Authorization.ToString();

            if (!string.IsNullOrWhiteSpace(authorization))
            {
                _httpClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(authorization);
            }

            var response = await _httpClient.GetAsync($"api/Profesores/{idProfesor}");
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning(
                    "Profesores.API respondió con estado {StatusCode} al consultar el profesor {IdProfesor}.",
                    (int)response.StatusCode,
                    idProfesor);

                return false;
            }

            _logger.LogInformation("Profesor validado correctamente desde Profesores.API. IdProfesor: {IdProfesor}", idProfesor);
            return response.IsSuccessStatusCode;
        }

        public async Task<int?> ObtenerIdProfesorAsync()
        {
            AgregarToken();

            var response = await _httpClient.GetAsync("api/Profesores/ObtenerIdProfesor");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("No fue posible obtener el profesor autenticado desde Profesores.API. StatusCode: {StatusCode}", (int)response.StatusCode);
                return null;
            }

            var profesor = await response.Content
                .ReadFromJsonAsync<ProfesorResponse>();

            _logger.LogInformation("Profesor autenticado obtenido correctamente. IdProfesor: {IdProfesor}", profesor.Id);
            return profesor?.Id;
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

        private class ProfesorResponse
        {
            public int Id { get; set; }
        }
    }
}
