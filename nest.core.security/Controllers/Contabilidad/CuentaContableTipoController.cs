using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nest.core.aplicacion.contabilidad.CuentaContableTipos.Commands;
using nest.core.aplicacion.contabilidad.CuentaContableTipos.Queries;
using nest.core.dominio;
using nest.core.dominio.Contabilidad.CuentaContableTipoEntities;
using nest.core.security.Extensions;

namespace nest.core.security.Controllers.Contabilidad
{
    /// <summary>
    /// Controlador para la gestión de tipos de cuentas contables.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiExplorerSettings(GroupName = ConfigureEndpoints.EndpointsEnum.CONTABILIDAD)]
    [Route($"{ConfigureEndpoints.EndpointsEnum.CONTABILIDAD}/[controller]")]
    public class CuentaContableTipoController : ControllerBase
    {
        private readonly ISender sender;

        public CuentaContableTipoController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<CuentaContableTipo>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<CuentaContableTipo>>> ObtenerTodos([FromQuery] ObtenerTodosQuery query, CancellationToken ct)
        {
            var entidad = await sender.Send(query, ct);
            return Ok(entidad);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CuentaContableTipo), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<CuentaContableTipo>> ObtenerPorId([FromQuery] ObtenerPorIdQuery query, CancellationToken ct)
        {
            var entidad = await sender.Send(query, ct);
            return Ok(entidad);
        }

        [HttpGet("activos")]
        [ProducesResponseType(typeof(List<CuentaContableTipo>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<CuentaContableTipo>>> ObtenerActivos([FromQuery] ObtenerActivosQuery query, CancellationToken ct)
        {
            var entidad = await sender.Send(query, ct);
            return Ok(entidad);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CuentaContableTipo), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<CuentaContableTipo>> Agregar([FromBody] CuentaContableTipoCrearCommand command, CancellationToken ct)
        {
            var entidad = await sender.Send(command, ct);
            return Ok(entidad);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(CuentaContableTipo), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<CuentaContableTipo>> Modificar([FromRoute] int id, [FromBody] CuentaContableTipoModificarCommand command, CancellationToken ct)
        {
            var cmd = command with { Id = id };
            var entidad = await sender.Send(cmd, ct);
            return Ok(entidad);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult> Eliminar([FromBody] CuentaContableTipoEliminarCommand command, CancellationToken ct)
        {
            await sender.Send(command, ct);
            return Ok();
        }
    }
}
