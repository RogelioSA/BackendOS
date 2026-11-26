using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nest.core.aplicacion.general.Distritos.Commands;
using nest.core.aplicacion.general.Distritos.Queries;
using nest.core.dominio;
using nest.core.dominio.General.DistritoEntities;
using nest.core.security.Extensions;

namespace nest.core.security.Controllers.General
{
    /// <summary>
    /// Controlador para la gestión de Distritos
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiExplorerSettings(GroupName = ConfigureEndpoints.EndpointsEnum.GENERAL)]
    [Route($"{ConfigureEndpoints.EndpointsEnum.GENERAL}/[controller]")]
    public class DistritoController : ControllerBase
    {
        private readonly ISender sender;
        public DistritoController(ISender sender)
        {
            this.sender = sender;
        }
        [HttpGet]
        [ProducesResponseType(typeof(List<Distrito>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<Distrito>>> ObtenerTodos([FromQuery] ObtenerTodosQuery command, CancellationToken ct)
        {
            var entidad = await sender.Send(command);
            return Ok(entidad);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Distrito), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<Distrito>> ObtenerPorId([FromRoute] int id, CancellationToken ct)
        {
            var entidad = await sender.Send(new ObtenerPorIdQuery(id));
            return Ok(entidad);
        }

        [HttpPost("filter")]
        [ProducesResponseType(typeof(LoadResult), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<LoadResult>> ObtenerPorFiltro([FromBody] DataSourceLoadOptionsBase options, CancellationToken ct)
        {
            var data = await sender.Send(new ObtenerDistritosPorFiltroQuery(options), ct);
            return Ok(data);
        }

        [HttpPost("filter_activos")]
        [ProducesResponseType(typeof(LoadResult), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<LoadResult>> ObtenerPorFiltroActivos([FromBody] DataSourceLoadOptionsBase options, CancellationToken ct)
        {
            var data = await sender.Send(new ObtenerDistritosPorFiltroActivosQuery(options), ct);
            return Ok(data);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Distrito), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<Distrito>> Agregar([FromBody] DistritoCrearCommand command, CancellationToken ct)
        {
            var entidad = await sender.Send(command);
            return Ok(entidad);
        }
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Distrito), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<Distrito>> Modificar([FromRoute] int id, [FromBody] DistritoModificarCommand command, CancellationToken ct)
        {
            var cmd = command with { Id = id };
            var entidad = await sender.Send(cmd);
            return Ok(entidad);
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult> Eliminar([FromBody] DistritoEliminarCommand command, CancellationToken ct)
        {
            var entidad = await sender.Send(command);
            return Ok();
        }
    }
}
