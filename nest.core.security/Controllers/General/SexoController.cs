using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nest.core.aplicacion.general.Sexos.Commands;
using nest.core.aplicacion.general.Sexos.Queries;
using nest.core.dominio;
using nest.core.dominio.General.SexoEntities;
using nest.core.security.Extensions;

namespace nest.core.security.Controllers.General
{
    [Authorize]
    [ApiController]
    [ApiExplorerSettings(GroupName = ConfigureEndpoints.EndpointsEnum.GENERAL)]
    [Route($"{ConfigureEndpoints.EndpointsEnum.GENERAL}/[controller]")]
    public class SexoController : ControllerBase
    {
        private readonly ISender sender;
        public SexoController(ISender sender)
        {
            this.sender = sender;
        }
        [HttpGet]
        [ProducesResponseType(typeof(List<Sexo>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<Sexo>>> ObtenerTodos(CancellationToken ct)
        {
            var data = await sender.Send(new ObtenerTodosQuery(), ct);
            return Ok(data);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Sexo), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<Sexo>> ObtenerPorId(byte id, CancellationToken ct)
        {
            var data = await sender.Send(new ObtenerPorIdQuery(id), ct);
            return Ok(data);
        }
        [HttpPost]
        [ProducesResponseType(typeof(Sexo), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<Sexo>> Agregar([FromBody] SexoCrearCommand command, CancellationToken ct)
        {
            var data = await sender.Send(command, ct);
            return Ok(data);
        }
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Sexo), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<Sexo>> Modificar(byte id, [FromBody] SexoModificarCommand command, CancellationToken ct)
        {
            var cmd = command with { Id = id };
            var data = await sender.Send(cmd, ct);
            return Ok(data);
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult> Eliminar(byte id, CancellationToken ct)
        {
            await sender.Send(new SexoEliminarCommand(id), ct);
            return Ok();
        }
    }
}
