using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Usuarios.Application.Interfaces;

namespace Usuarios.Infrastructure.ServicesClient
{
    public class EstudianteClient : IEstudianteClient
    {
        private readonly HttpClient _httpClient;

        public EstudianteClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> CrearAsync(int usuarioId, string nombre)
        {
            var response = await _httpClient.PostAsJsonAsync(
                "api/Estudiantes",
                new
                {
                    usuarioId,
                    nombre
                });

            return response.IsSuccessStatusCode;
        }
    }
}
