using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Profesores.Application.DTOs;
using Profesores.Application.Interfaces;
using Profesores.Domain.Constants;

namespace Profesores.API.Controllers
{
    /// <summary>
    /// Controlador encargado de la gestión de profesores.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ProfesoresController : Controller
    {
        private readonly IProfesorService _service;

        public ProfesoresController(IProfesorService service)
        {
            _service = service;
        }

        /// <summary>
        /// Obtiene una lista paginada de los profesores.
        /// </summary>
        [Authorize(Roles = Roles.Admin + "," + Roles.Profesor)]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _service.GetAllAsync(page, pageSize);

            return Ok(result);
        }

        /// <summary>
        /// Obtiene un profesor por el id
        /// </summary>
        [Authorize(Roles = Roles.Admin + "," + Roles.Profesor)]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var profesor = await _service.GetByIdAsync(id);

            if (profesor is null) { return NotFound(); }
            ;

            return Ok(profesor);
        }


        /// <summary>
        /// Crea un nuevo profesor.
        /// </summary>
        /// <param name="dto">Información del profesor a crear.</param>
        /// <returns>El profesor creado.</returns>
        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProfesorDto dto)
        {
            var profesor = await _service.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = profesor.Id },
                profesor);
        }

        /// <summary>
        /// Actualiza las propiedades de un profesor
        /// </summary>
        [Authorize(Roles = Roles.Admin)]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProfesorDto dto)
        {
            var updated = await _service.UpdateAsync(id, dto);

            if (!updated) { return NotFound(); }
            ;

            return NoContent();
        }

        /// <summary>
        /// Elimina un profesor por el id
        /// </summary>
        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);

            if (!deleted) { return NotFound(); }
            ;

            return NoContent();
        }

        /// <summary>
        /// Obtiene el Id de un profesor por el IdUsuario
        /// </summary>
        [Authorize(Roles = Roles.Profesor)]
        [HttpGet("ObtenerIdProfesor")]
        public async Task<IActionResult> ObtenerIdProfesor()
        {
            var usuarioIdClaim = User.FindFirst(
                System.Security.Claims.ClaimTypes.NameIdentifier);

            if (usuarioIdClaim is null)
                return Unauthorized();

            if (!int.TryParse(usuarioIdClaim.Value, out var usuarioId))
                return Unauthorized();

            var profesor = await _service.GetByUsuarioIdAsync(usuarioId);

            if (profesor is null)
                return NotFound();

            return Ok(profesor);
        }
    }
}
