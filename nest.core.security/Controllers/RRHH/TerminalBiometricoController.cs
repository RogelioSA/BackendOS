using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nest.core.aplicacion.rrhh.TerminalBiometricos.Commands;
using nest.core.aplicacion.rrhh.TerminalBiometricos.Queries;
using nest.core.dominio;
using nest.core.dominio.RRHH.TerminalBiometricoEntities;
using nest.core.security.Extensions;

namespace nest.core.security.Controllers.RRHH
{
    [Authorize]
    [ApiExplorerSettings(GroupName = ConfigureEndpoints.EndpointsEnum.RRHH)]
    [Route($"{ConfigureEndpoints.EndpointsEnum.RRHH}/[controller]")]
    [ApiController]
    public class TerminalBiometricoController : ControllerBase
    {
        private readonly ISender sender;

        public TerminalBiometricoController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<TerminalBiometrico>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<TerminalBiometrico>>> ObtenerTodos(CancellationToken ct)
        {
            var data = await sender.Send(new ObtenerTerminalBiometricosQuery(), ct);
            return Ok(data);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TerminalBiometrico), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<TerminalBiometrico>> ObtenerPorId(int id, CancellationToken ct)
        {
            var data = await sender.Send(new ObtenerTerminalBiometricoPorIdQuery(id), ct);
            return Ok(data);
        }

        [HttpPost("filter")]
        [ProducesResponseType(typeof(LoadResult), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<LoadResult>> ObtenerPorFiltro([FromBody] DataSourceLoadOptionsBase options, CancellationToken ct)
        {
            var data = await sender.Send(new ObtenerTerminalBiometricosPorFiltroDataSourceQuery(options), ct);
            return Ok(data);
        }

        [HttpPost("filter_activos")]
        [ProducesResponseType(typeof(LoadResult), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<LoadResult>> ObtenerPorFiltroActivos([FromBody] DataSourceLoadOptionsBase options, CancellationToken ct)
        {
            var data = await sender.Send(new ObtenerTerminalBiometricosPorFiltroActivosQuery(options), ct);
            return Ok(data);
        }

        [HttpPost]
        [ProducesResponseType(typeof(TerminalBiometrico), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<TerminalBiometrico>> Agregar([FromBody] TerminalBiometricoCrearCommand command, CancellationToken ct)
        {
            var data = await sender.Send(command, ct);
            return Ok(data);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(TerminalBiometrico), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<TerminalBiometrico>> Modificar(int id, [FromBody] TerminalBiometricoModificarCommand command, CancellationToken ct)
        {
            var data = await sender.Send(command with { Id = id }, ct);
            return Ok(data);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult> Eliminar(int id, CancellationToken ct)
        {
            await sender.Send(new TerminalBiometricoEliminarCommand(id), ct);
            return Ok();
        }
    }
}
