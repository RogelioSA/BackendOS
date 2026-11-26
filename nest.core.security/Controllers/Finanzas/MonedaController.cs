using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nest.core.aplicacion.finanzas.Monedas.Commands;
using nest.core.aplicacion.finanzas.Monedas.Queries;
using nest.core.dominio;
using nest.core.dominio.Finanzas.MonedaEntities;
using nest.core.security.Extensions;

namespace nest.core.security.Controllers.Finanzas
{
    [Authorize]
    [ApiController]
    [ApiExplorerSettings(GroupName = ConfigureEndpoints.EndpointsEnum.FINANZAS)]
    [Route($"{ConfigureEndpoints.EndpointsEnum.FINANZAS}/[controller]")]
    public class MonedaController : ControllerBase
    {
        private readonly ISender sender;

        public MonedaController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<Moneda>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<Moneda>>> ObtenerTodos(CancellationToken ct)
        {
            var data = await sender.Send(new ObtenerTodosQuery(), ct);
            return Ok(data);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Moneda), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<Moneda>> ObtenerPorId([FromRoute] int id, CancellationToken ct)
        {
            var data = await sender.Send(new ObtenerPorIdQuery(id), ct);
            return Ok(data);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Moneda), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<Moneda>> Agregar([FromBody] MonedaCrearCommand command, CancellationToken ct)
        {
            var data = await sender.Send(command, ct);
            return Ok(data);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Moneda), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<Moneda>> Modificar([FromRoute] int id, [FromBody] MonedaModificarCommand command, CancellationToken ct)
        {
            var cmd = command with { Id = id };
            var data = await sender.Send(cmd, ct);
            return Ok(data);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult> Eliminar([FromRoute] int id, CancellationToken ct)
        {
            await sender.Send(new MonedaEliminarCommand(id), ct);
            return Ok(true);
        }
    }
}
