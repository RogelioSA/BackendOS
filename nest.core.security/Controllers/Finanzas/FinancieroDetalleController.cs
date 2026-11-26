using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nest.core.aplicacion.finanzas.FinancieroDetalles.Commands;
using nest.core.aplicacion.finanzas.FinancieroDetalles.Queries;
using nest.core.dominio;
using nest.core.dominio.Finanzas.FinancieroDetalleEntities;
using nest.core.security.Extensions;

namespace nest.core.security.Controllers.Finanzas
{
    [Authorize]
    [ApiController]
    [ApiExplorerSettings(GroupName = ConfigureEndpoints.EndpointsEnum.FINANZAS)]
    [Route($"{ConfigureEndpoints.EndpointsEnum.FINANZAS}/[controller]")]
    public class FinancieroDetalleController : ControllerBase
    {
        private readonly ISender sender;

        public FinancieroDetalleController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<FinancieroDetalle>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<FinancieroDetalle>>> ObtenerTodos(CancellationToken ct)
        {
            var data = await sender.Send(new ObtenerTodosQuery(), ct);
            return Ok(data);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(FinancieroDetalle), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<FinancieroDetalle>> ObtenerPorId([FromRoute] long id, CancellationToken ct)
        {
            var data = await sender.Send(new ObtenerPorIdQuery(id), ct);
            return Ok(data);
        }

        [HttpGet("cabecera/{financieroCabeceraId}")]
        [ProducesResponseType(typeof(List<FinancieroDetalle>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<FinancieroDetalle>>> ObtenerPorCabecera([FromRoute] long financieroCabeceraId, CancellationToken ct)
        {
            var data = await sender.Send(new ObtenerPorCabeceraQuery(financieroCabeceraId), ct);
            return Ok(data);
        }

        [HttpPost]
        [ProducesResponseType(typeof(FinancieroDetalle), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<FinancieroDetalle>> Agregar([FromBody] FinancieroDetalleCrearCommand command, CancellationToken ct)
        {
            var data = await sender.Send(command, ct);
            return Ok(data);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(FinancieroDetalle), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<FinancieroDetalle>> Modificar([FromRoute] long id, [FromBody] FinancieroDetalleModificarCommand command, CancellationToken ct)
        {
            var cmd = command with { Id = id };
            var data = await sender.Send(cmd, ct);
            return Ok(data);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult> Eliminar([FromRoute] long id, CancellationToken ct)
        {
            await sender.Send(new FinancieroDetalleEliminarCommand(id), ct);
            return Ok(true);
        }
    }
}
