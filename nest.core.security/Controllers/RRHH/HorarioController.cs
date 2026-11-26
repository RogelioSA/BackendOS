using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nest.core.aplicacion.rrhh.Horarios.Commands;
using nest.core.aplicacion.rrhh.Horarios.Queries;
using nest.core.dominio;
using nest.core.dominio.RRHH.HorarioCabeceraEntities;
using nest.core.security.Extensions;

namespace nest.core.security.Controllers.RRHH
{
    [Authorize]
    [ApiExplorerSettings(GroupName = ConfigureEndpoints.EndpointsEnum.RRHH)]
    [Route($"{ConfigureEndpoints.EndpointsEnum.RRHH}/[controller]")]
    [ApiController]
    public class HorarioController : ControllerBase
    {
        private readonly ISender sender;

        public HorarioController(ISender sender)
        {
            this.sender = sender;
        }

        /// <summary>
        /// Obtiene todos los horarios registrados.
        /// </summary>
        /// <returns>Lista de horarios.</returns>
        /// <response code="200">Devuelve la lista de horarios.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<HorarioCabecera>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<HorarioCabecera>>> ObtenerTodos(CancellationToken ct)
        {
            var data = await sender.Send(new ObtenerTodosQuery(), ct);
            return Ok(data);
        }

        /// <summary>
        /// Obtiene un horario por su ID.
        /// </summary>
        /// <param name="id">ID del horario.</param>
        /// <returns>Horario correspondiente al ID.</returns>
        /// <response code="200">Horario encontrado.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(HorarioCabecera), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<HorarioCabecera>> ObtenerPorId(int id, CancellationToken ct)
        {
            var data = await sender.Send(new ObtenerPorIdQuery(id), ct);
            return Ok(data);
        }

        [HttpPost("filter")]
        [ProducesResponseType(typeof(LoadResult), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<LoadResult>> ObtenerPorFiltro([FromBody] DataSourceLoadOptionsBase options, CancellationToken ct)
        {
            var data = await sender.Send(new ObtenerHorariosPorFiltroDataSourceQuery(options), ct);
            return Ok(data);
        }

        [HttpPost("filter_activos")]
        [ProducesResponseType(typeof(LoadResult), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<LoadResult>> ObtenerPorFiltroActivos([FromBody] DataSourceLoadOptionsBase options, CancellationToken ct)
        {
            var data = await sender.Send(new ObtenerHorariosPorFiltroActivosQuery(options), ct);
            return Ok(data);
        }

        /// <summary>
        /// Agrega un nuevo horario.
        /// </summary>
        /// <param name="registro">DTO con la información del horario a crear (cabecera, detalles y eventos).</param>
        /// <returns>Horario creado.</returns>
        /// <response code="200">Horario agregado exitosamente.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpPost]
        [ProducesResponseType(typeof(HorarioCabecera), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<HorarioCabecera>> Agregar([FromBody] HorarioCrearCommand command, CancellationToken ct)
        {
            var data = await sender.Send(command, ct);
            return Ok(data);
        }

        /// <summary>
        /// Modifica un horario existente.
        /// </summary>
        /// <param name="id">ID del horario a modificar.</param>
        /// <param name="registro">DTO con la información actualizada del horario (cabecera, detalles y eventos).</param>
        /// <returns>Horario modificado.</returns>
        /// <response code="200">Horario modificado exitosamente.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(HorarioCabecera), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<HorarioCabecera>> Modificar(int id, [FromBody] HorarioModificarCommand command, CancellationToken ct)
        {
            var cmd = command with { Id = id };
            var data = await sender.Send(cmd, ct);
            return Ok(data);
        }

        /// <summary>
        /// Elimina un horario.
        /// </summary>
        /// <param name="id">ID del horario a eliminar.</param>
        /// <returns>True si la eliminación fue exitosa.</returns>
        /// <response code="200">Horario eliminado exitosamente.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult> Eliminar(int id, CancellationToken ct)
        {
            await sender.Send(new HorarioEliminarCommand(id), ct);
            return Ok(true);
        }
    }
}
