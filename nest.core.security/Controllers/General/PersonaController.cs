using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nest.core.aplicacion.general.Personas.Commands;
using nest.core.aplicacion.general.Personas.Queries;
using nest.core.dominio;
using nest.core.dominio.General.PersonaEntities;
using nest.core.security.Extensions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace nest.core.security.Controllers.General
{
    /// <summary>
    /// Controlador para la gestión de personas.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiExplorerSettings(GroupName = ConfigureEndpoints.EndpointsEnum.GENERAL)]
    [Route($"{ConfigureEndpoints.EndpointsEnum.GENERAL}/[controller]")]
    public class PersonaController : ControllerBase
    {
        private readonly ISender sender;
        public PersonaController(ISender sender)
        {
            this.sender = sender;
        }
        [HttpGet]
        [ProducesResponseType(typeof(List<Persona>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<Persona>>> ObtenerTodos([FromQuery] ObtenerTodosQuery command, CancellationToken ct)
        {
            var entidad = await sender.Send(command);
            return Ok(entidad);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Persona), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<Persona>> ObtenerPorId([FromRoute] int id, CancellationToken ct)
        {
            var entidad = await sender.Send(new ObtenerPorIdQuery(id));
            return Ok(entidad);
        }
        [HttpGet("activos")]
        [ProducesResponseType(typeof(List<Persona>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<Persona>>> ObtenerActivos([FromQuery] ObtenerActivosQuery command, CancellationToken ct)
        {
            var entidad = await sender.Send(command);
            return Ok(entidad);
        }
        [HttpPost]
        [ProducesResponseType(typeof(Persona), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<Persona>> Agregar([FromBody] PersonaCrearCommand command, CancellationToken ct)
        {
            var entidad = await sender.Send(command);
            return Ok(entidad);
        }
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Persona), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<Persona>> Modificar([FromRoute] int id, [FromBody] PersonaModificarCommand command, CancellationToken ct)
        {
            var cmd = command with { Id = id };
            var entidad = await sender.Send(cmd);
            return Ok(entidad);
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult> Eliminar([FromRoute] int id, CancellationToken ct)
        {
            var entidad = await sender.Send(new PersonaEliminarCommand(id));
            return Ok();
        }
    }
}
