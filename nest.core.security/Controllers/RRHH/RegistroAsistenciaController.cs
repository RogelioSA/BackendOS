using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nest.core.aplicacion.rrhh.RegistroAsistenciaOrdenTrabajos.Commands;
using nest.core.aplicacion.rrhh.RegistroAsistencias.Commands;
using nest.core.aplicacion.rrhh.RegistroAsistencias.Queries;
using nest.core.dominio;
using nest.core.dominio.RRHH.RegistroAsistenciaEntities;
using nest.core.security.Extensions;

namespace nest.core.security.Controllers.RRHH
{
    [Authorize]
    [ApiExplorerSettings(GroupName = ConfigureEndpoints.EndpointsEnum.RRHH)]
    [Route($"{ConfigureEndpoints.EndpointsEnum.RRHH}/[controller]")]
    [ApiController]
    public class RegistroAsistenciaController : ControllerBase
    {
        private readonly ISender sender;

        public RegistroAsistenciaController(ISender sender)
        {
            this.sender = sender;
        }

        /// <summary>
        /// Obtiene todos los registros de asistencia disponibles.
        /// </summary>
        /// <returns>Lista de registros de asistencia.</returns>
        /// <response code="200">Devuelve la colección de registros de asistencia.</response>
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
        /// Obtiene un registro de asistencia específico por su identificador.
        /// </summary>
        /// <param name="id">Identificador del registro de asistencia.</param>
        /// <returns>Registro de asistencia correspondiente.</returns>
        /// <response code="200">Registro encontrado.</response>
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
        /// Busca registros de asistencia de un personal en un rango de fechas.
        /// </summary>
        /// <param name="personalId">Identificador del personal.</param>
        /// <param name="fechaInicio">Fecha inicial del filtro (inclusive).</param>
        /// <param name="fechaFin">Fecha final del filtro (inclusive).</param>
        /// <returns>Lista de registros de asistencia que coinciden con el filtro.</returns>
        /// <response code="200">Devuelve la lista filtrada de registros.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpGet("personal/{personalId}")]
        [ProducesResponseType(typeof(List<RegistroAsistencia>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<RegistroAsistencia>>> BuscarPorPersonalIdRangoFecha(int personalId, [FromQuery] DateTime fechaInicio, [FromQuery] DateTime fechaFin, CancellationToken ct)
        {
            var query = new BuscarPorPersonalIdRangoFechaQuery(personalId, fechaInicio, fechaFin);
            var data = await sender.Send(query, ct);
            return Ok(data);
        }

        [HttpGet("range_date")]
        [ProducesResponseType(typeof(List<RegistroAsistenciaQueryView>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<RegistroAsistenciaQueryView>>> BuscarPorRangoFecha([FromQuery] DateTime fechaInicio, [FromQuery] DateTime fechaFin, CancellationToken ct)
        {
            var query = new BuscarPorRangoFechaQuery(fechaInicio, fechaFin);
            var data = await sender.Send(query, ct);
            return Ok(data);
        }

        [HttpGet("personal_range_date")]
        [ProducesResponseType(typeof(List<RegistroAsistencia>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<RegistroAsistencia>>> BuscarPersonalAsistenciasRangoFechas([FromQuery] DateTime fechaInicio, [FromQuery] DateTime fechaFin, CancellationToken ct)
        {
            var query = new BuscarPersonalAsistenciasRangoFechasQuery(fechaInicio, fechaFin);
            var data = await sender.Send(query, ct);
            return Ok(data);
        }

        [HttpPost("filter")]
        [ProducesResponseType(typeof(LoadResult), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<LoadResult>> ObtenerPorFiltro([FromBody] DataSourceLoadOptionsBase options, CancellationToken ct)
        {
            var data = await sender.Send(new ObtenerRegistroAsistenciasPorFiltroDataSourceQuery(options), ct);
            return Ok(data);
        }

        [HttpPost("filter_activos")]
        [ProducesResponseType(typeof(LoadResult), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<LoadResult>> ObtenerPorFiltroActivos([FromBody] DataSourceLoadOptionsBase options, CancellationToken ct)
        {
            var data = await sender.Send(new ObtenerRegistroAsistenciasPorFiltroActivosQuery(options), ct);
            return Ok(data);
        }

        /// <summary>
        /// Crea un nuevo registro de asistencia.
        /// </summary>
        /// <param name="registro">Datos del registro de asistencia a crear.</param>
        /// <returns>Registro de asistencia creado.</returns>
        /// <response code="200">Registro creado correctamente.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpPost]
        [ProducesResponseType(typeof(RegistroAsistencia), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<RegistroAsistencia>> Agregar([FromBody] RegistroAsistenciaCrearCommand command, CancellationToken ct)
        {
            var data = await sender.Send(command, ct);
            return Ok(data);
        }

        /// <summary>
        /// Crea un nuevo registro de asistencia.
        /// </summary>
        /// <param name="registro">Datos del registro de asistencia a crear.</param>
        /// <returns>Registro de asistencia creado.</returns>
        /// <response code="200">Registro creado correctamente.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpPost("serverdt")]
        [ProducesResponseType(typeof(RegistroAsistencia), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<RegistroAsistencia>> AgregarConDatetime([FromBody] RegistroAsistenciaCrearCommand command, CancellationToken ct)
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
        public async Task<ActionResult<RegistroAsistencia>> AgregarUsuarioActual([FromBody] RegistroAsistenciaCrearUsuarioActualCommand command, CancellationToken ct)
        {
            var data = await sender.Send(command, ct);
            return Ok(data);
        }

        /// <summary>Asocia la firma adjunta a todas las asistencias del usuario para cada fecha jornal indicada.</summary>
        [HttpPost("firmas")]
        [ProducesResponseType(typeof(int), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<int>> AdjuntarFirmas([FromBody] List<AdjuntarFirmaPorFechaItem> items, CancellationToken ct)
        {
            var cantidad = await sender.Send(new AdjuntarFirmaPorFechaCommand(items), ct);
            return Ok(cantidad);
        }

        /// <summary>
        /// Crea un nuevo registro de asistencia utilizando los parametros del token como inicio de sesion.
        /// </summary>
        /// <returns>Registro de asistencia creado.</returns>
        /// <response code="200">Registro creado correctamente.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpPost("zkteco")]
        [ProducesResponseType(typeof(RegistroAsistencia), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<RegistroAsistencia>> AgregarUsuarioActualZkTeco([FromBody] RegistroAsistenciaTerminalZKTecoCrearCommand command, CancellationToken ct)
        {
            var data = await sender.Send(command, ct);
            return Ok(data);
        }

        /// <summary>
        /// Crea un nuevo registro de asistencia utilizando los parametros del token como inicio de sesion.
        /// </summary>
        /// <returns>Registro de asistencia creado.</returns>
        /// <response code="200">Registro creado correctamente.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpPost("regularizar")]
        [ProducesResponseType(typeof(RegistroAsistencia), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<RegistroAsistencia>> AgregarMarcaRegularizar([FromBody] RegistroAsistenciaOrdenTrabajoRegularizarCrearCommand command, CancellationToken ct)
        {
            var data = await sender.Send(command, ct);
            return Ok(data);
        }

        /// <summary>
        /// Crea un nuevo registro de asistencia utilizando los parametros del token como inicio de sesion.
        /// </summary>
        /// <returns>Registro de asistencia creado.</returns>
        /// <response code="200">Registro creado correctamente.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpPut("regularizar")]
        [ProducesResponseType(typeof(RegistroAsistencia), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<RegistroAsistencia>> ModificarMarcaRegularizar([FromBody] RegistroAsistenciaOrdenTrabajoRegularizarModificarCommand command, CancellationToken ct)
        {
            var data = await sender.Send(command, ct);
            return Ok(data);
        }

        /// <summary>
        /// Actualiza un registro de asistencia existente.
        /// </summary>
        /// <param name="id">Identificador del registro que se desea actualizar.</param>
        /// <param name="registro">Datos actualizados del registro de asistencia.</param>
        /// <returns>Registro de asistencia actualizado.</returns>
        /// <response code="200">Registro actualizado correctamente.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(RegistroAsistencia), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<RegistroAsistencia>> Modificar(long id, [FromBody] RegistroAsistenciaModificarCommand command, CancellationToken ct)
        {
            var cmd = command with { Id = id };
            var data = await sender.Send(cmd, ct);
            return Ok(data);
        }

        /// <summary>
        /// Elimina un registro de asistencia.
        /// </summary>
        /// <param name="id">Identificador del registro a eliminar.</param>
        /// <returns>Resultado de la operación.</returns>
        /// <response code="200">Registro eliminado correctamente.</response>
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
