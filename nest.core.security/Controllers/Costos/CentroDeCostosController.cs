using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nest.core.aplicacion.costos.CentroCostos.Commands;
using nest.core.aplicacion.costos.CentroCostos.Queries;
using nest.core.dominio;
using nest.core.dominio.Costos.CentroDeCostosEntities;
using nest.core.security.Extensions;

namespace nest.core.security.Controllers.Costos
{
    [Authorize]
    [ApiController]
    [ApiExplorerSettings(GroupName = ConfigureEndpoints.EndpointsEnum.COSTOS)]
    [Route($"{ConfigureEndpoints.EndpointsEnum.COSTOS}/[controller]")]
    public class CentroDeCostosController : ControllerBase
    {
        private readonly ISender sender;

        public CentroDeCostosController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<CentroDeCostos>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<CentroDeCostos>>> ObtenerTodos(CancellationToken ct)
        {
            var data = await sender.Send(new ObtenerTodosQuery(), ct);
            return Ok(data);
        }

        [HttpGet("activos")]
        [ProducesResponseType(typeof(List<CentroDeCostos>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<CentroDeCostos>>> ObtenerActivos(CancellationToken ct)
        {
            var data = await sender.Send(new ObtenerActivosQuery(), ct);
            return Ok(data);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CentroDeCostos), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<CentroDeCostos>> ObtenerPorId([FromRoute] int id, CancellationToken ct)
        {
            var data = await sender.Send(new ObtenerPorIdQuery(id), ct);
            return Ok(data);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CentroDeCostos), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<CentroDeCostos>> Agregar([FromBody] CentroDeCostosCrearCommand command, CancellationToken ct)
        {
            var data = await sender.Send(command, ct);
            return Ok(data);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(CentroDeCostos), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<CentroDeCostos>> Modificar([FromRoute] int id, [FromBody] CentroDeCostosModificarCommand command, CancellationToken ct)
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
            await sender.Send(new CentroDeCostosEliminarCommand(id), ct);
            return Ok(true);
        }
    }
}
