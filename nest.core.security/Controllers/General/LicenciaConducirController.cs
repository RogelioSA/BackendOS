using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nest.core.aplicacion.general.LicenciasConducir.Commands;
using nest.core.aplicacion.general.LicenciasConducir.Queries;
using nest.core.dominio;
using nest.core.dominio.General.LicenciaConducirEntities;
using nest.core.security.Extensions;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace nest.core.security.Controllers.General
{
    [Authorize]
    [ApiController]
    [ApiExplorerSettings(GroupName = ConfigureEndpoints.EndpointsEnum.GENERAL)]
    [Route($"{ConfigureEndpoints.EndpointsEnum.GENERAL}/[controller]")]
    public class LicenciaConducirController : ControllerBase
    {
        private readonly ISender sender;

        public LicenciaConducirController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<LicenciaConducir>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<LicenciaConducir>>> ObtenerTodos(CancellationToken ct)
        {
            var entidad = await sender.Send(new ObtenerTodosQuery(), ct);
            return Ok(entidad);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(LicenciaConducir), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<LicenciaConducir>> ObtenerPorId([FromRoute] byte id, CancellationToken ct)
        {
            var entidad = await sender.Send(new ObtenerPorIdQuery(id), ct);
            return Ok(entidad);
        }

        [HttpGet("activos")]
        [ProducesResponseType(typeof(List<LicenciaConducir>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<LicenciaConducir>>> ObtenerActivos(CancellationToken ct)
        {
            var entidad = await sender.Send(new ObtenerActivosQuery(), ct);
            return Ok(entidad);
        }

        [HttpPost]
        [ProducesResponseType(typeof(LicenciaConducir), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<LicenciaConducir>> Agregar([FromBody] LicenciaConducirCrearCommand command, CancellationToken ct)
        {
            var entidad = await sender.Send(command, ct);
            return Ok(entidad);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(LicenciaConducir), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<LicenciaConducir>> Modificar([FromRoute] byte id, [FromBody] LicenciaConducirModificarCommand command, CancellationToken ct)
        {
            var cmd = command with { Id = id };
            var entidad = await sender.Send(cmd, ct);
            return Ok(entidad);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult> Eliminar([FromRoute] byte id, CancellationToken ct)
        {
            await sender.Send(new LicenciaConducirEliminarCommand(id), ct);
            return Ok();
        }
    }
}
