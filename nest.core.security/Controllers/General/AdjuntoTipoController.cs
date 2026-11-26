using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nest.core.aplicacion.general.AdjuntoTipos.Commands;
using nest.core.aplicacion.general.AdjuntoTipos.Queries;
using nest.core.dominio;
using nest.core.dominio.General.AdjuntoTipoEntities;
using nest.core.security.Extensions;

namespace nest.core.security.Controllers.General
{
    /// <summary>
    /// Controlador para la administración de los tipos de adjuntos.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiExplorerSettings(GroupName = ConfigureEndpoints.EndpointsEnum.GENERAL)]
    [Route($"{ConfigureEndpoints.EndpointsEnum.GENERAL}/[controller]")]
    public class AdjuntoTipoController : ControllerBase
    {
        private readonly ISender sender;

        public AdjuntoTipoController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<AdjuntoTipo>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<AdjuntoTipo>>> ObtenerTodos(CancellationToken ct)
        {
            var data = await sender.Send(new ObtenerTodosQuery(), ct);
            return Ok(data);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(AdjuntoTipo), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<AdjuntoTipo>> ObtenerPorId(AdjuntoTipoEnum id, CancellationToken ct)
        {
            var data = await sender.Send(new ObtenerPorIdQuery(id), ct);
            return Ok(data);
        }

        [HttpGet("activos")]
        [ProducesResponseType(typeof(List<AdjuntoTipo>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<AdjuntoTipo>>> ObtenerActivos(CancellationToken ct)
        {
            var data = await sender.Send(new ObtenerActivosQuery(), ct);
            return Ok(data);
        }

        [HttpPost]
        [ProducesResponseType(typeof(AdjuntoTipo), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<AdjuntoTipo>> Agregar([FromBody] AdjuntoTipoCrearCommand command, CancellationToken ct)
        {
            var data = await sender.Send(command, ct);
            return Ok(data);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(AdjuntoTipo), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<AdjuntoTipo>> Modificar(AdjuntoTipoEnum id, [FromBody] AdjuntoTipoModificarCommand command, CancellationToken ct)
        {
            var cmd = command with { Id = id };
            var data = await sender.Send(cmd, ct);
            return Ok(data);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult> Eliminar(AdjuntoTipoEnum id, CancellationToken ct)
        {
            await sender.Send(new AdjuntoTipoEliminarCommand(id), ct);
            return Ok();
        }
    }
}
