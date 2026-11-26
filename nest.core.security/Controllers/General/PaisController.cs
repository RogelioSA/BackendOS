using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nest.core.aplicacion.general.Paises.Commands;
using nest.core.aplicacion.general.Paises.Queries;
using nest.core.dominio;
using nest.core.dominio.General.PaisEntities;
using nest.core.security.Extensions;

namespace nest.core.security.Controllers.General
{
    [Authorize]
    [ApiController]
    [ApiExplorerSettings(GroupName = ConfigureEndpoints.EndpointsEnum.GENERAL)]
    [Route($"{ConfigureEndpoints.EndpointsEnum.GENERAL}/[controller]")]
    public class PaisController : ControllerBase
    {
        private readonly ISender sender;

        public PaisController(ISender sender)
        {
            this.sender = sender;
        }
        [HttpGet]
        [ProducesResponseType(typeof(List<Pais>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<Pais>>> ObtenerTodos(CancellationToken cancellationToken)
        {
            var data = await sender.Send(new ObtenerTodosQuery(), cancellationToken);
            return Ok(data);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Pais), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<Pais>> ObtenerPorId([FromRoute] int id, CancellationToken cancellationToken)
        {
            var data = await sender.Send(new ObtenerPorIdQuery(id), cancellationToken);
            return Ok(data);
        }
        [HttpPost]
        [ProducesResponseType(typeof(Pais), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<Pais>> Agregar([FromBody] PaisCrearCommand command, CancellationToken cancellationToken)
        {
            var data = await sender.Send(command, cancellationToken);
            return Ok(data);
        }
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Pais), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<Pais>> Modificar([FromRoute] int id, [FromBody] PaisModificarCommand command, CancellationToken cancellationToken)
        {
            var cmd = command with { Id = id };
            var data = await sender.Send(cmd, cancellationToken);
            return Ok(data);
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult> Eliminar([FromRoute] int id, CancellationToken cancellationToken)
        {
            await sender.Send(new PaisEliminarCommand(id), cancellationToken);
            return Ok(true);
        }
    }
}
