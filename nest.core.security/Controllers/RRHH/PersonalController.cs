using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nest.core.aplicacion.rrhh.Personales.Commands;
using nest.core.aplicacion.rrhh.Personales.Queries;
using nest.core.dominio;
using nest.core.dominio.RRHH.PersonalEntities;
using nest.core.security.Extensions;

namespace nest.core.security.Controllers.RRHH
{
    [Authorize]
    [ApiExplorerSettings(GroupName = ConfigureEndpoints.EndpointsEnum.RRHH)]
    [Route($"{ConfigureEndpoints.EndpointsEnum.RRHH}/[controller]")]
    [ApiController]
    public class PersonalController : ControllerBase
    {
        private readonly ISender sender;

        public PersonalController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<Personal>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<Personal>>> ObtenerTodos(CancellationToken ct)
        {
            var data = await sender.Send(new ObtenerPersonalesQuery(), ct);
            return Ok(data);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Personal), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<Personal>> ObtenerPorId(int id, CancellationToken ct)
        {
            var data = await sender.Send(new ObtenerPersonalPorIdQuery(id), ct);
            return Ok(data);
        }

        [HttpGet("activos")]
        [ProducesResponseType(typeof(List<Personal>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<Personal>>> ObtenerActivos(CancellationToken ct)
        {
            var data = await sender.Send(new ObtenerPersonalesActivosQuery(), ct);
            return Ok(data);
        }

        [HttpPost("filter")]
        [ProducesResponseType(typeof(LoadResult), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<LoadResult>> ObtenerPorFiltro([FromBody] DataSourceLoadOptionsBase options, CancellationToken ct)
        {
            var data = await sender.Send(new ObtenerPersonalesPorFiltroDataSourceQuery(options), ct);
            return Ok(data);
        }

        [HttpPost("filter_activos")]
        [ProducesResponseType(typeof(LoadResult), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<LoadResult>> ObtenerPorFiltroActivos([FromBody] DataSourceLoadOptionsBase options, CancellationToken ct)
        {
            var data = await sender.Send(new ObtenerPersonalesPorFiltroActivosQuery(options), ct);
            return Ok(data);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Personal), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<Personal>> Agregar([FromBody] PersonalCrearCommand command, CancellationToken ct)
        {
            var data = await sender.Send(command, ct);
            return Ok(data);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Personal), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<Personal>> Modificar(int id, [FromBody] PersonalModificarCommand command, CancellationToken ct)
        {
            var data = await sender.Send(command with { Id = id }, ct);
            return Ok(data);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult> Eliminar(int id, CancellationToken ct)
        {
            await sender.Send(new PersonalEliminarCommand(id), ct);
            return Ok();
        }
    }
}
