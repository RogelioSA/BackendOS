using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nest.core.aplicacion.rrhh.RegistroAsistenciaOrdenTrabajos.Commands;
using nest.core.aplicacion.rrhh.RegistroAsistenciaOrdenTrabajos.Queries;
using nest.core.aplicacion.rrhh.RegistroAsistencias.Commands;
using nest.core.dominio;
using nest.core.dominio.RRHH.RegistroAsistenciaEntities;
using nest.core.dominio.RRHH.RegistroAsistenciaOrdenTrabajoEntities;
using nest.core.dominio.Security.Tenant;
using nest.core.security.Extensions;

namespace nest.core.security.Controllers.RRHH
{
    [Authorize]
    [ApiExplorerSettings(GroupName = ConfigureEndpoints.EndpointsEnum.RRHH)]
    [Route($"{ConfigureEndpoints.EndpointsEnum.RRHH}/[controller]")]
    [ApiController]
    public class RegistroAsistenciaOrdenTrabajoController : ControllerBase
    {
        private readonly ISender sender;
        private readonly IConnectionStringService connectionStringService;

        public RegistroAsistenciaOrdenTrabajoController(ISender sender, IConnectionStringService connectionStringService)
        {
            this.sender = sender;
            this.connectionStringService = connectionStringService;
        }

        /// <summary>
        /// Obtiene todos los registros de asistencia vinculados a órdenes de trabajo.
        /// </summary>
        /// <returns>Listado de relaciones registradas.</returns>
        /// <response code="200">Relaciones recuperadas correctamente.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<RegistroAsistencia>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<RegistroAsistencia>>> ObtenerTodos(CancellationToken ct)
        {
            var data = await sender.Send(new ObtenerTodosQuery(), ct);
            return Ok(data);
        }

        /// <summary>
        /// Obtiene un registro de asistencia asociado a una orden de trabajo por su identificador.
        /// </summary>
        /// <param name="id">Identificador del registro de asistencia.</param>
        /// <returns>Relación encontrada.</returns>
        /// <response code="200">Relación encontrada correctamente.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RegistroAsistencia), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<RegistroAsistencia>> ObtenerPorId(long id, CancellationToken ct)
        {
            var data = await sender.Send(new ObtenerPorIdQuery(id), ct);
            return Ok(data);
        }

        /// <summary>
        /// Obtiene todos los registros de asistencia vinculados a órdenes de trabajo.
        /// </summary>
        /// <param name="request">Parametros de request.</param>
        /// <returns>Listado de relaciones registradas.</returns>
        /// <response code="200">Relaciones recuperadas correctamente.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpGet("by_currentuser_and_range_date")]
        [ProducesResponseType(typeof(List<RegistroAsistencia>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<RegistroAsistencia>>> ObtenerTodos([FromQuery] ObtenerPorIIdUsuarioYRangoFechaQuery request, CancellationToken ct)
        {
            var req = request with { UsuarioId = this.connectionStringService.UserId };
            var data = await sender.Send(req, ct);
            return Ok(data);
        }

        [HttpPost("filter")]
        [ProducesResponseType(typeof(LoadResult), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<LoadResult>> ObtenerPorFiltro([FromBody] DataSourceLoadOptionsBase options, CancellationToken ct)
        {
            var data = await sender.Send(new ObtenerRegistroAsistenciaOrdenTrabajosPorFiltroDataSourceQuery(options), ct);
            return Ok(data);
        }

        [HttpPost("filter_activos")]
        [ProducesResponseType(typeof(LoadResult), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<LoadResult>> ObtenerPorFiltroActivos([FromBody] DataSourceLoadOptionsBase options, CancellationToken ct)
        {
            var data = await sender.Send(new ObtenerRegistroAsistenciaOrdenTrabajosPorFiltroActivosQuery(options), ct);
            return Ok(data);
        }

        /// <summary>
        /// Crea una relación entre un registro de asistencia y una orden de trabajo.
        /// </summary>
        /// <param name="registro">Datos de la relación a crear.</param>
        /// <returns>Relación creada.</returns>
        /// <response code="200">Relación creada correctamente.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpPost("serverdt")]
        [ProducesResponseType(typeof(RegistroAsistencia), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<RegistroAsistencia>> AgregarConDatetime([FromBody] RegistroAsistenciaOrdenTrabajoCrearCommand command, CancellationToken ct)
        {
            var cmd = command with { Fecha = DateTime.Now };
            var data = await sender.Send(cmd, ct);
            return Ok(data);
        }

        /// <summary>
        /// Crea un nuevo registro de asistencia utilizando los parametros del token como inicio de sesion.
        /// </summary>
        /// <returns>Registro de asistencia creado.</returns>
        /// <response code="200">Registro creado correctamente.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpPost("current_user")]
        [ProducesResponseType(typeof(RegistroAsistencia), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<RegistroAsistencia>> AgregarUsuarioActual([FromBody] RegistroAsistenciaOrdenTrabajoCrearUsuarioActualCommand command, CancellationToken ct)
        {
            var data = await sender.Send(command, ct);
            return Ok(data);
        }

        /// <summary>
        /// Crea una relación entre un registro de asistencia y una orden de trabajo.
        /// </summary>
        /// <param name="registro">Datos de la relación a crear.</param>
        /// <returns>Relación creada.</returns>
        /// <response code="200">Relación creada correctamente.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpPost]
        [ProducesResponseType(typeof(RegistroAsistencia), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<RegistroAsistencia>> Agregar([FromBody] RegistroAsistenciaOrdenTrabajoCrearCommand command, CancellationToken ct)
        {
            var data = await sender.Send(command, ct);
            return Ok(data);
        }

        /// <summary>
        /// Actualiza una relación existente entre un registro de asistencia y una orden de trabajo.
        /// </summary>
        /// <param name="id">Identificador del registro de asistencia asociado.</param>
        /// <param name="registro">Datos actualizados de la relación.</param>
        /// <returns>Relación actualizada.</returns>
        /// <response code="200">Relación actualizada correctamente.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(RegistroAsistenciaOrdenTrabajo), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<RegistroAsistencia>> Modificar(long id, [FromBody] RegistroAsistenciaOrdenTrabajoModificarCommand command, CancellationToken ct)
        {
            var cmd = command with { Id = id };
            var data = await sender.Send(cmd, ct);
            return Ok(data);
        }

        /// <summary>
        /// Elimina la relación entre un registro de asistencia y una orden de trabajo.
        /// </summary>
        /// <param name="id">Identificador del registro de asistencia asociado.</param>
        /// <returns>Resultado de la operación.</returns>
        /// <response code="200">Relación eliminada correctamente.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult> Eliminar(long id, CancellationToken ct)
        {
            await sender.Send(new RegistroAsistenciaEliminarCommand(id), ct);
            return Ok(true);
        }
    }
}
