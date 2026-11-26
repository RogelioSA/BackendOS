using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nest.core.aplicacion.patrimonial.UbicacionActivos.Commands;
using nest.core.aplicacion.patrimonial.UbicacionActivos.Queries;
using nest.core.dominio;
using nest.core.dominio.Patrimonial.UbicacionActivoEntities;
using nest.core.security.Extensions;

namespace nest.core.security.Controllers.Patrimonial
{
    /// <summary>
    /// Controlador para la gestión de ubicaciones físicas asignadas a los activos.
    /// Permite consultar el historial de ubicaciones, registrar nuevos traslados y actualizar registros existentes.
    /// Todos los endpoints requieren autorización mediante token JWT válido.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiExplorerSettings(GroupName = ConfigureEndpoints.EndpointsEnum.PATRIMONIAL)]
    [Route($"{ConfigureEndpoints.EndpointsEnum.PATRIMONIAL}/[controller]")]
    public class UbicacionActivoController : ControllerBase
    {
        private readonly ISender sender;
        private readonly ILogger<UbicacionActivoController> logger;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="UbicacionActivoController"/>.
        /// </summary>
        /// <param name="service">Servicio de aplicación para gestionar ubicaciones de activos.</param>
        /// <param name="logger">Registrador para auditoría y trazabilidad.</param>
        public UbicacionActivoController(ISender sender, ILogger<UbicacionActivoController> logger)
        {
            this.sender = sender;
            this.logger = logger;
        }

        /// <summary>
        /// Obtiene todas las ubicaciones registradas para todos los activos.
        /// </summary>
        /// <returns>Lista con los registros de ubicación.</returns>
        /// <response code="200">Listado obtenido correctamente.</response>
        /// <response code="400">La solicitud es inválida.</response>
        /// <response code="401">No autorizado.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<UbicacionActivo>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<List<UbicacionActivo>>> ObtenerTodos()
        {
            try
            {
                var data = await sender.Send(new ObtenerTodosQuery());
                return Ok(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Obtiene las ubicaciones históricas asociadas a un activo específico.
        /// </summary>
        /// <param name="activoId">Identificador del activo a consultar.</param>
        /// <returns>Listado de ubicaciones pertenecientes al activo.</returns>
        /// <response code="200">Listado obtenido correctamente.</response>
        /// <response code="400">La solicitud es inválida.</response>
        /// <response code="401">No autorizado.</response>
        [HttpGet("activo/{activoId:long}")]
        [ProducesResponseType(typeof(List<UbicacionActivo>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<List<UbicacionActivo>>> ObtenerPorActivo(long activoId)
        {
            try
            {
                var data = await sender.Send(new ObtenerPorActivoQuery(activoId));
                return Ok(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Obtiene un registro de ubicación por su identificador.
        /// </summary>
        /// <param name="id">Identificador del registro de ubicación.</param>
        /// <returns>Registro encontrado.</returns>
        /// <response code="200">Registro encontrado.</response>
        /// <response code="400">La solicitud es inválida.</response>
        /// <response code="401">No autorizado.</response>
        [HttpGet("{id:long}")]
        [ProducesResponseType(typeof(UbicacionActivo), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<UbicacionActivo>> ObtenerPorId(long id)
        {
            try
            {
                var data = await sender.Send(new ObtenerPorIdQuery(id));
                return Ok(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Registra una nueva ubicación para un activo.
        /// </summary>
        /// <param name="registro">Datos del traslado a registrar.</param>
        /// <returns>Registro creado.</returns>
        /// <response code="200">Registro creado correctamente.</response>
        /// <response code="400">La solicitud es inválida.</response>
        /// <response code="401">No autorizado.</response>
        [HttpPost]
        [ProducesResponseType(typeof(UbicacionActivo), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<UbicacionActivo>> Agregar([FromBody] UbicacionActivoCrearCommand command)
        {
            try
            {
                var data = await sender.Send(command);
                return Ok(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Actualiza la información de una ubicación existente.
        /// </summary>
        /// <param name="id">Identificador del registro a modificar.</param>
        /// <param name="registro">Datos actualizados del traslado.</param>
        /// <returns>Registro actualizado.</returns>
        /// <response code="200">Registro actualizado correctamente.</response>
        /// <response code="400">La solicitud es inválida.</response>
        /// <response code="401">No autorizado.</response>
        [HttpPut("{id:long}")]
        [ProducesResponseType(typeof(UbicacionActivo), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<UbicacionActivo>> Modificar(long id, [FromBody] UbicacionActivoModificarCommand command)
        {
            try
            {
                var updatedCommand = command with { Id = id };
                var data = await sender.Send(updatedCommand);
                return Ok(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Elimina un registro de ubicación.
        /// </summary>
        /// <param name="id">Identificador del registro a eliminar.</param>
        /// <returns>Resultado de la operación.</returns>
        /// <response code="200">Registro eliminado correctamente.</response>
        /// <response code="400">La solicitud es inválida.</response>
        /// <response code="401">No autorizado.</response>
        [HttpDelete("{id:long}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult> Eliminar(long id)
        {
            try
            {
                await sender.Send(new UbicacionActivoEliminarCommand(id));
                return Ok(true);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
