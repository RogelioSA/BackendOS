using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nest.core.aplicacion.rrhh.GrupoTrabajos.Commands;
using nest.core.aplicacion.rrhh.GrupoTrabajos.Queries;
using nest.core.dominio;
using nest.core.dominio.RRHH.GrupoTrabajoEntities;
using nest.core.security.Extensions;

namespace nest.core.security.Controllers.RRHH
{
    [Authorize]
    [ApiExplorerSettings(GroupName = ConfigureEndpoints.EndpointsEnum.RRHH)]
    [Route($"{ConfigureEndpoints.EndpointsEnum.RRHH}/[controller]")]
    [ApiController]
    public class GrupoTrabajoController : ControllerBase
    {
        private readonly ISender sender;

        public GrupoTrabajoController(ISender sender)
        {
            this.sender = sender;
        }

        /// <summary>
        /// Obtiene todos los grupos de trabajo registrados.
        /// </summary>
        /// <returns>Lista de grupos de trabajo.</returns>
        /// <response code="200">Devuelve la lista de grupos de trabajo.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<GrupoTrabajo>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<GrupoTrabajo>>> ObtenerTodos(CancellationToken ct)
        {
            var data = await sender.Send(new ObtenerTodosQuery(), ct);
            return Ok(data);
        }

        /// <summary>
        /// Obtiene un grupo de trabajo por su identificador.
        /// </summary>
        /// <param name="id">Identificador del grupo de trabajo.</param>
        /// <returns>Grupo de trabajo asociado al identificador.</returns>
        /// <response code="200">Grupo de trabajo encontrado.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GrupoTrabajo), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<GrupoTrabajo>> ObtenerPorId(long id, CancellationToken ct)
        {
            var data = await sender.Send(new ObtenerPorIdQuery(id), ct);
            return Ok(data);
        }

        /// <summary>
        /// Obtiene los grupos de trabajo activos.
        /// </summary>
        /// <returns>Lista de grupos de trabajo con estado activo.</returns>
        /// <response code="200">Devuelve la lista de grupos de trabajo activos.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpGet("activos")]
        [ProducesResponseType(typeof(List<GrupoTrabajo>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<GrupoTrabajo>>> ObtenerActivos(CancellationToken ct)
        {
            var data = await sender.Send(new ObtenerActivosQuery(), ct);
            return Ok(data);
        }

        [HttpPost("filter")]
        [ProducesResponseType(typeof(LoadResult), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<LoadResult>> ObtenerPorFiltro([FromBody] DataSourceLoadOptionsBase options, CancellationToken ct)
        {
            var data = await sender.Send(new ObtenerGrupoTrabajosPorFiltroDataSourceQuery(options), ct);
            return Ok(data);
        }

        [HttpPost("filter_activos")]
        [ProducesResponseType(typeof(LoadResult), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<LoadResult>> ObtenerPorFiltroActivos([FromBody] DataSourceLoadOptionsBase options, CancellationToken ct)
        {
            var data = await sender.Send(new ObtenerGrupoTrabajosPorFiltroActivosQuery(options), ct);
            return Ok(data);
        }

        /// <summary>
        /// Crea un nuevo grupo de trabajo con sus integrantes.
        /// </summary>
        /// <param name="registro">Información del grupo y su detalle de personas.</param>
        /// <returns>Grupo de trabajo creado.</returns>
        /// <response code="200">Grupo de trabajo registrado correctamente.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpPost]
        [ProducesResponseType(typeof(GrupoTrabajo), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<GrupoTrabajo>> Agregar([FromBody] GrupoTrabajoCrearCommand command, CancellationToken ct)
        {
            var data = await sender.Send(command, ct);
            return Ok(data);
        }

        /// <summary>
        /// Actualiza la información de un grupo de trabajo y sus integrantes.
        /// </summary>
        /// <param name="id">Identificador del grupo de trabajo.</param>
        /// <param name="registro">Información actualizada del grupo y detalle de personas.</param>
        /// <returns>Grupo de trabajo actualizado.</returns>
        /// <response code="200">Grupo de trabajo modificado correctamente.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(GrupoTrabajo), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<GrupoTrabajo>> Modificar(long id, [FromBody] GrupoTrabajoModificarCommand command, CancellationToken ct)
        {
            var cmd = command with { Id = id };
            var data = await sender.Send(cmd, ct);
            return Ok(data);
        }

        /// <summary>
        /// Elimina un grupo de trabajo existente.
        /// </summary>
        /// <param name="id">Identificador del grupo de trabajo a eliminar.</param>
        /// <returns>Resultado de la operación.</returns>
        /// <response code="200">Grupo de trabajo eliminado correctamente.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult> Eliminar(long id, CancellationToken ct)
        {
            await sender.Send(new GrupoTrabajoEliminarCommand(id), ct);
            return Ok(true);
        }
    }
}
