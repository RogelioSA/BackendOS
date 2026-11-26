using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nest.core.aplicacion.finanzas.FinancieroCabeceras.Commands;
using nest.core.aplicacion.finanzas.FinancieroCabeceras.Queries;
using nest.core.dominio;
using nest.core.dominio.Finanzas.FinancieroCabeceraEntities;
using nest.core.security.Extensions;

namespace nest.core.security.Controllers.Finanzas
{
    [Authorize]
    [ApiController]
    [ApiExplorerSettings(GroupName = ConfigureEndpoints.EndpointsEnum.FINANZAS)]
    [Route($"{ConfigureEndpoints.EndpointsEnum.FINANZAS}/[controller]")]
    public class FinancieroCabeceraController : ControllerBase
    {
        private readonly ISender sender;

        public FinancieroCabeceraController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<FinancieroCabecera>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<FinancieroCabecera>>> ObtenerTodos(CancellationToken ct)
        {
            var data = await sender.Send(new ObtenerTodosQuery(), ct);
            return Ok(data);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(FinancieroCabecera), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<FinancieroCabecera>> ObtenerPorId([FromRoute] long id, CancellationToken ct)
        {
            var data = await sender.Send(new ObtenerPorIdQuery(id), ct);
            return Ok(data);
        }

        [HttpPost]
        [ProducesResponseType(typeof(FinancieroCabecera), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<FinancieroCabecera>> Agregar([FromBody] FinancieroCabeceraCrearCommand command, CancellationToken ct)
        {
            var data = await sender.Send(command, ct);
            return Ok(data);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(FinancieroCabecera), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<FinancieroCabecera>> Modificar([FromRoute] long id, [FromBody] FinancieroCabeceraModificarCommand command, CancellationToken ct)
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
            await sender.Send(new FinancieroCabeceraEliminarCommand(id), ct);
            return Ok(true);
        }
    }
}
