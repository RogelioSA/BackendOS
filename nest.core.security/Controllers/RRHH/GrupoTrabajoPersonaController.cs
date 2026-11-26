using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nest.core.aplicacion.rrhh.GrupoTrabajoPersonas.Commands;
using nest.core.aplicacion.rrhh.GrupoTrabajoPersonas.Queries;
using nest.core.dominio;
using nest.core.dominio.RRHH.GrupoTrabajoPersonaEntities;
using nest.core.security.Extensions;

namespace nest.core.security.Controllers.RRHH
{
    [Authorize]
    [ApiExplorerSettings(GroupName = ConfigureEndpoints.EndpointsEnum.RRHH)]
    [Route($"{ConfigureEndpoints.EndpointsEnum.RRHH}/[controller]")]
    [ApiController]
    public class GrupoTrabajoPersonaController : ControllerBase
    {
        private readonly ISender sender;

        public GrupoTrabajoPersonaController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<GrupoTrabajoPersona>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<GrupoTrabajoPersona>>> ObtenerTodos(CancellationToken ct)
        {
            var data = await sender.Send(new ObtenerGrupoTrabajoPersonasQuery(), ct);
            return Ok(data);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GrupoTrabajoPersona), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<GrupoTrabajoPersona>> ObtenerPorId(long id, CancellationToken ct)
        {
            var data = await sender.Send(new ObtenerGrupoTrabajoPersonaPorIdQuery(id), ct);
            return Ok(data);
        }

        [HttpGet("grupo/{grupoTrabajoId}")]
        [ProducesResponseType(typeof(List<GrupoTrabajoPersona>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<GrupoTrabajoPersona>>> ObtenerPorGrupo(long grupoTrabajoId, CancellationToken ct)
        {
            var data = await sender.Send(new ObtenerGrupoTrabajoPersonasPorGrupoQuery(grupoTrabajoId), ct);
            return Ok(data);
        }

        [HttpPost("filter")]
        [ProducesResponseType(typeof(LoadResult), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<LoadResult>> ObtenerPorFiltro([FromBody] DataSourceLoadOptionsBase options, CancellationToken ct)
        {
            var data = await sender.Send(new ObtenerGrupoTrabajoPersonasPorFiltroDataSourceQuery(options), ct);
            return Ok(data);
        }

        [HttpPost("filter_activos")]
        [ProducesResponseType(typeof(LoadResult), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<LoadResult>> ObtenerPorFiltroActivos([FromBody] DataSourceLoadOptionsBase options, CancellationToken ct)
        {
            var data = await sender.Send(new ObtenerGrupoTrabajoPersonasPorFiltroActivosQuery(options), ct);
            return Ok(data);
        }

        [HttpPost]
        [ProducesResponseType(typeof(GrupoTrabajoPersona), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<GrupoTrabajoPersona>> Agregar([FromBody] GrupoTrabajoPersonaCrearCommand command, CancellationToken ct)
        {
            var data = await sender.Send(command, ct);
            return Ok(data);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(GrupoTrabajoPersona), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<GrupoTrabajoPersona>> Modificar(long id, [FromBody] GrupoTrabajoPersonaModificarCommand command, CancellationToken ct)
        {
            var data = await sender.Send(command with { Id = id }, ct);
            return Ok(data);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult> Eliminar(long id, CancellationToken ct)
        {
            await sender.Send(new GrupoTrabajoPersonaEliminarCommand(id), ct);
            return Ok();
        }
    }
}
