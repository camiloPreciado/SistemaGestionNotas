using Estudiantes.Application.DTOs;
using Estudiantes.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Estudiantes.Domain.Constants;

namespace Estudiantes.API.Controllers
{
    /// <summary>
    /// Controlador encargado de la gestión de estudiantes.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class EstudiantesController : ControllerBase
    {
        private readonly IEstudianteService _service;

        public EstudiantesController(IEstudianteService service)
        {
            _service = service;
        }

        /// <summary>
        /// Obtiene una lista paginada de los estudiantes.
        /// </summary>
        [Authorize(Roles = Roles.Admin + "," + Roles.Profesor)]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _service.GetAllAsync(page, pageSize);

            return Ok(result);
        }

        /// <summary>
        /// Obtiene un estudiante por el id
        /// </summary>
        [Authorize(Roles = Roles.Admin + "," + Roles.Profesor)]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var estudiante = await _service.GetByIdAsync(id);

            if (estudiante is null) { return NotFound(); };

            return Ok(estudiante);
        }


        /// <summary>
        /// Crea un nuevo estudiante.
        /// </summary>
        /// <param name="dto">Información del estudiante a crear.</param>
        /// <returns>El estudiante creado.</returns>
        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEstudianteDto dto)
        {
            var estudiante = await _service.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = estudiante.Id },
                estudiante);
        }

        /// <summary>
        /// Actualiza las propiedades de un estudiante
        /// </summary>
        [Authorize(Roles = Roles.Admin)]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateEstudianteDto dto)
        {
            var updated = await _service.UpdateAsync(id, dto);

            if (!updated) { return NotFound(); };

            return NoContent();
        }

        /// <summary>
        /// Elimina un estudiante por el id
        /// </summary>
        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);

            if (!deleted) { return NotFound(); };

            return NoContent();
        }

        /// <summary>
        /// Obtiene el Id de un estudiante por el IdUsuario
        /// </summary>
        [Authorize(Roles = Roles.Estudiante)]
        [HttpGet("ObtenerIdEstudiante")]
        public async Task<IActionResult> ObtenerIdEstudiante()
        {
            var usuarioIdClaim = User.FindFirst(
                System.Security.Claims.ClaimTypes.NameIdentifier);

            if (usuarioIdClaim is null)
                return Unauthorized();

            if (!int.TryParse(usuarioIdClaim.Value, out var usuarioId))
                return Unauthorized();
            
            var estudiante = await _service.GetByUsuarioIdAsync(usuarioId);

            if (estudiante is null)
                return NotFound();

            return Ok(estudiante);
        }
    }
}
