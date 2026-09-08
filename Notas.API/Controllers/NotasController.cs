using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notas.Application.DTOs;
using Notas.Application.Interfaces;
using Notas.Domain.Constants;

namespace Notas.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotasController : ControllerBase
    {
        private readonly INotaService _service;
        private readonly IEstudianteClient _estudianteClient;

        public NotasController(
            INotaService service,
            IEstudianteClient estudianteClient)
        {
            _service = service;
            _estudianteClient = estudianteClient;
        }

        [Authorize(Roles = Roles.Admin + "," + Roles.Profesor)]
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var result =
                await _service.GetAllAsync(page, pageSize);

            return Ok(result);
        }

        [Authorize(Roles = Roles.Estudiante)]
        [HttpGet("mis-notas")]
        public async Task<IActionResult> GetMisNotas(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var idEstudiante =
                await _estudianteClient.ObtenerIdEstudianteAsync();

            if (idEstudiante is null)
                return NotFound(
                    new
                    {
                        mensaje =
                            "No se encontró el estudiante asociado al usuario."
                    });

            var result =
                await _service.GetByEstudianteAsync(
                    idEstudiante.Value,
                    page,
                    pageSize);

            return Ok(result);
        }

        [Authorize(Roles = Roles.Admin + "," + Roles.Profesor)]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var nota =
                await _service.GetByIdAsync(id);

            if (nota is null)
                return NotFound();

            return Ok(nota);
        }

        [Authorize(Roles = Roles.Admin + "," + Roles.Profesor)]
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateNotaDto dto)
        {
            var nota =
                await _service.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = nota.Id },
                nota);
        }

        [Authorize(Roles = Roles.Admin + "," + Roles.Profesor)]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateNotaDto dto)
        {
            var updated =
                await _service.UpdateAsync(id, dto);

            if (!updated)
                return NotFound();

            return NoContent();
        }

        [Authorize(Roles = Roles.Admin + "," + Roles.Profesor)]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted =
                await _service.DeleteAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
