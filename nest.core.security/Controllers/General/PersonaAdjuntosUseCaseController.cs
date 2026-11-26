using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nest.core.aplicacion.general.PersonaUseCases.Commands;
using nest.core.aplicacion.general.PersonaUseCases.Queries;
using nest.core.dominio;
using nest.core.dominio.General.PersonaEntities;
using nest.core.security.Extensions;

namespace nest.core.security.Controllers.General
{
    [Authorize]
    [ApiController]
    [ApiExplorerSettings(GroupName = ConfigureEndpoints.EndpointsEnum.GENERAL)]
    [Route($"{ConfigureEndpoints.EndpointsEnum.GENERAL}/[controller]")]
    public class PersonaAdjuntosUseCaseController : ControllerBase
    {
        private readonly ISender sender;

        public PersonaAdjuntosUseCaseController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<Persona>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<Persona>>> ObtenerTodos(CancellationToken ct)
        {
            var entidad = await sender.Send(new ObtenerPersonasConAdjuntosQuery(), ct);
            return Ok(entidad);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Persona), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<Persona>> ObtenerPorId([FromRoute] int id, CancellationToken ct)
        {
            var entidad = await sender.Send(new ObtenerPersonaConAdjuntosPorIdQuery(id), ct);
            return Ok(entidad);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Persona), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<Persona>> Agregar([FromBody] PersonaAdjuntosUseCaseCrearCommand command, CancellationToken ct)
        {
            var entidad = await sender.Send(command, ct);
            return Ok(entidad);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Persona), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<Persona>> Modificar([FromRoute] int id, [FromBody] PersonaAdjuntosUseCaseModificarCommand command, CancellationToken ct)
        {
            var cmd = command with { Id = id };
            var entidad = await sender.Send(cmd, ct);
            return Ok(entidad);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult> Eliminar([FromRoute] int id, CancellationToken ct)
        {
            await sender.Send(new PersonaAdjuntosUseCaseEliminarCommand(id), ct);
            return Ok();
        }
    }
}
