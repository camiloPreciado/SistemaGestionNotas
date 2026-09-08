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
    public class ProfesorClient : IProfesorClient
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProfesorClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> ExisteAsync(int idProfesor)
        {
            var authorization = _httpContextAccessor.HttpContext?.Request.Headers.Authorization.ToString();

            if (!string.IsNullOrWhiteSpace(authorization))
            {
                _httpClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(authorization);
            }

            var response = await _httpClient.GetAsync(
                $"api/Profesores/{idProfesor}");

            return response.IsSuccessStatusCode;
        }

        public async Task<int?> ObtenerIdProfesorAsync()
        {
            AgregarToken();

            var response = await _httpClient.GetAsync(
                "api/Profesores/ObtenerIdProfesor");

            if (!response.IsSuccessStatusCode)
                return null;

            var profesor = await response.Content
                .ReadFromJsonAsync<ProfesorResponse>();

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
