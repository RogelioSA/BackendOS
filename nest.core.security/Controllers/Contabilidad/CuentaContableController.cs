using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nest.core.aplicacion.contabilidad.CuentaContables.Commands;
using nest.core.aplicacion.contabilidad.CuentaContables.Queries;
using nest.core.dominio;
using nest.core.dominio.Contabilidad.CuentaContableEntities;
using nest.core.security.Extensions;

namespace nest.core.security.Controllers.Contabilidad
{
    /// <summary>
    /// Controlador para la gestión de cuentas contables.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiExplorerSettings(GroupName = ConfigureEndpoints.EndpointsEnum.CONTABILIDAD)]
    [Route($"{ConfigureEndpoints.EndpointsEnum.CONTABILIDAD}/[controller]")]
    public class CuentaContableController : ControllerBase
    {
        private readonly ISender sender;

        public CuentaContableController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<CuentaContable>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<CuentaContable>>> ObtenerTodos([FromQuery] ObtenerTodosQuery query, CancellationToken ct)
        {
            var entidad = await sender.Send(query, ct);
            return Ok(entidad);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CuentaContable), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<CuentaContable>> ObtenerPorId([FromQuery] ObtenerPorIdQuery query, CancellationToken ct)
        {
            var entidad = await sender.Send(query, ct);
            return Ok(entidad);
        }

        [HttpGet("activos")]
        [ProducesResponseType(typeof(List<CuentaContable>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<CuentaContable>>> ObtenerActivos([FromQuery] ObtenerActivosQuery query, CancellationToken ct)
        {
            var entidad = await sender.Send(query, ct);
            return Ok(entidad);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CuentaContable), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<CuentaContable>> Agregar([FromBody] CuentaContableCrearCommand command, CancellationToken ct)
        {
            var entidad = await sender.Send(command, ct);
            return Ok(entidad);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(CuentaContable), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<CuentaContable>> Modificar([FromRoute] long id, [FromBody] CuentaContableModificarCommand command, CancellationToken ct)
        {
            var cmd = command with { Id = id };
            var entidad = await sender.Send(cmd, ct);
            return Ok(entidad);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult> Eliminar([FromBody] CuentaContableEliminarCommand command, CancellationToken ct)
        {
            await sender.Send(command, ct);
            return Ok();
        }
    }
}
