using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nest.core.aplicacion.finanzas.EntidadFinancieras.Commands;
using nest.core.aplicacion.finanzas.EntidadFinancieras.Queries;
using nest.core.dominio;
using nest.core.dominio.Finanzas.EntidadFinancieraEntities;
using nest.core.security.Extensions;

namespace nest.core.security.Controllers.Finanzas
{
    [Authorize]
    [ApiController]
    [ApiExplorerSettings(GroupName = ConfigureEndpoints.EndpointsEnum.FINANZAS)]
    [Route($"{ConfigureEndpoints.EndpointsEnum.FINANZAS}/[controller]")]
    public class EntidadFinancieraController : ControllerBase
    {
        private readonly ISender sender;

        public EntidadFinancieraController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<EntidadFinanciera>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<EntidadFinanciera>>> ObtenerTodos(CancellationToken ct)
        {
            var data = await sender.Send(new ObtenerTodosQuery(), ct);
            return Ok(data);
        }

        [HttpGet("activos")]
        [ProducesResponseType(typeof(List<EntidadFinanciera>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<EntidadFinanciera>>> ObtenerActivos(CancellationToken ct)
        {
            var data = await sender.Send(new ObtenerActivosQuery(), ct);
            return Ok(data);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(EntidadFinanciera), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<EntidadFinanciera>> ObtenerPorId([FromRoute] int id, CancellationToken ct)
        {
            var data = await sender.Send(new ObtenerPorIdQuery(id), ct);
            return Ok(data);
        }

        [HttpPost]
        [ProducesResponseType(typeof(EntidadFinanciera), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<EntidadFinanciera>> Agregar([FromBody] EntidadFinancieraCrearCommand command, CancellationToken ct)
        {
            var data = await sender.Send(command, ct);
            return Ok(data);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(EntidadFinanciera), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<EntidadFinanciera>> Modificar([FromRoute] int id, [FromBody] EntidadFinancieraModificarCommand command, CancellationToken ct)
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
            await sender.Send(new EntidadFinancieraEliminarCommand(id), ct);
            return Ok(true);
        }
    }
}
