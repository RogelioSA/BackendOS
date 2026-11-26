using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nest.core.aplicacion.rrhh.Cargos.Commands;
using nest.core.aplicacion.rrhh.Cargos.Queries;
using nest.core.dominio;
using nest.core.dominio.RRHH.CargoEntities;
using nest.core.security.Extensions;

namespace nest.core.security.Controllers.RRHH
{
    [Authorize]
    [ApiExplorerSettings(GroupName = ConfigureEndpoints.EndpointsEnum.RRHH)]
    [Route($"{ConfigureEndpoints.EndpointsEnum.RRHH}/[controller]")]
    [ApiController]
    public class CargoController : ControllerBase
    {
        private readonly ISender sender;

        public CargoController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<Cargo>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<Cargo>>> ObtenerTodos(CancellationToken ct)
        {
            var data = await sender.Send(new ObtenerCargosQuery(), ct);
            return Ok(data);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Cargo), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<Cargo>> ObtenerPorId(int id, CancellationToken ct)
        {
            var data = await sender.Send(new ObtenerCargoPorIdQuery(id), ct);
            return Ok(data);
        }

        [HttpGet("activos")]
        [ProducesResponseType(typeof(List<Cargo>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<Cargo>>> ObtenerActivos(CancellationToken ct)
        {
            var data = await sender.Send(new ObtenerCargosActivosQuery(), ct);
            return Ok(data);
        }

        [HttpPost("filter")]
        [ProducesResponseType(typeof(LoadResult), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<LoadResult>> ObtenerPorFiltro([FromBody] DataSourceLoadOptionsBase options, CancellationToken ct)
        {
            var data = await sender.Send(new ObtenerCargosPorFiltroDataSourceQuery(options), ct);
            return Ok(data);
        }

        [HttpPost("filter_activos")]
        [ProducesResponseType(typeof(LoadResult), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<LoadResult>> ObtenerPorFiltroActivos([FromBody] DataSourceLoadOptionsBase options, CancellationToken ct)
        {
            var data = await sender.Send(new ObtenerCargosPorFiltroActivosQuery(options), ct);
            return Ok(data);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Cargo), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<Cargo>> Agregar([FromBody] CargoCrearCommand command, CancellationToken ct)
        {
            var data = await sender.Send(command, ct);
            return Ok(data);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Cargo), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<Cargo>> Modificar(int id, [FromBody] CargoModificarCommand command, CancellationToken ct)
        {
            var data = await sender.Send(command with { Id = id }, ct);
            return Ok(data);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult> Eliminar(int id, CancellationToken ct)
        {
            await sender.Send(new CargoEliminarCommand(id), ct);
            return Ok();
        }
    }
}
