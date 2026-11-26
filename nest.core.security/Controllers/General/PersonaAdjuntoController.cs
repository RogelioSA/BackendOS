using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nest.core.aplicacion.general.PersonaAdjuntos.Commands;
using nest.core.aplicacion.general.PersonaAdjuntos.Queries;
using nest.core.dominio;
using nest.core.dominio.General.PersonaAdjuntoEntities;
using nest.core.security.Extensions;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace nest.core.security.Controllers.General
{
    /// <summary>
    /// Controlador para la gestión de adjuntos asociados a personas.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiExplorerSettings(GroupName = ConfigureEndpoints.EndpointsEnum.GENERAL)]
    [Route($"{ConfigureEndpoints.EndpointsEnum.GENERAL}/[controller]")]
    public class PersonaAdjuntoController : ControllerBase
    {
        private readonly ISender sender;

        public PersonaAdjuntoController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<PersonaAdjunto>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<PersonaAdjunto>>> ObtenerTodos(CancellationToken ct)
        {
            var entidad = await sender.Send(new ObtenerTodosQuery(), ct);
            return Ok(entidad);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PersonaAdjunto), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<PersonaAdjunto>> ObtenerPorId([FromRoute] long id, CancellationToken ct)
        {
            var entidad = await sender.Send(new ObtenerPorIdQuery(id), ct);
            return Ok(entidad);
        }

        [HttpGet("persona/{personaId}")]
        [ProducesResponseType(typeof(List<PersonaAdjunto>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<PersonaAdjunto>>> ObtenerPorPersona([FromRoute] int personaId, CancellationToken ct)
        {
            var entidad = await sender.Send(new ObtenerPorPersonaQuery(personaId), ct);
            return Ok(entidad);
        }

        [HttpPost]
        [ProducesResponseType(typeof(PersonaAdjunto), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<PersonaAdjunto>> Agregar([FromBody] PersonaAdjuntoCrearCommand command, CancellationToken ct)
        {
            var entidad = await sender.Send(command, ct);
            return Ok(entidad);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(PersonaAdjunto), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<PersonaAdjunto>> Modificar([FromRoute] long id, [FromBody] PersonaAdjuntoModificarCommand command, CancellationToken ct)
        {
            var cmd = command with { Id = id };
            var entidad = await sender.Send(cmd, ct);
            return Ok(entidad);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult> Eliminar([FromRoute] long id, CancellationToken ct)
        {
            await sender.Send(new PersonaAdjuntoEliminarCommand(id), ct);
            return Ok();
        }
    }
}
