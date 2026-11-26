using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nest.core.aplicacion.patrimonial.Activos.Commands;
using nest.core.aplicacion.patrimonial.Activos.Queries;
using nest.core.dominio;
using nest.core.dominio.Patrimonial.ActivoEntities;
using nest.core.security.Extensions;

namespace nest.core.security.Controllers.Patrimonial
{
    /// <summary>
    /// Controlador para la gestión de activos patrimoniales.
    /// Permite consultar, registrar, actualizar y eliminar activos.
    /// Todos los endpoints requieren autorización mediante token JWT.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiExplorerSettings(GroupName = ConfigureEndpoints.EndpointsEnum.PATRIMONIAL)]
    [Route($"{ConfigureEndpoints.EndpointsEnum.PATRIMONIAL}/[controller]")]
    public class ActivoController : ControllerBase
    {
        private readonly ISender sender;
        private readonly ILogger<ActivoController> logger;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="ActivoController"/>.
        /// </summary>
        /// <param name="service">Servicio de aplicación para la gestión de activos.</param>
        /// <param name="logger">Registrador para auditoría y trazabilidad de errores.</param>
        public ActivoController(ISender sender, ILogger<ActivoController> logger)
        {
            this.sender = sender;
            this.logger = logger;
        }

        /// <summary>
        /// Obtiene todos los activos registrados.
        /// </summary>
        /// <returns>Lista con los activos disponibles.</returns>
        /// <response code="200">Listado obtenido correctamente.</response>
        /// <response code="400">La solicitud es inválida.</response>
        /// <response code="401">No autorizado.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<Activo>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<List<Activo>>> ObtenerTodos()
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
        /// Obtiene un activo según su identificador.
        /// </summary>
        /// <param name="id">Identificador único del activo.</param>
        /// <returns>El activo correspondiente al identificador proporcionado.</returns>
        /// <response code="200">Activo encontrado.</response>
        /// <response code="400">La solicitud es inválida.</response>
        /// <response code="401">No autorizado.</response>
        [HttpGet("{id:long}")]
        [ProducesResponseType(typeof(Activo), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<Activo>> ObtenerPorId(long id)
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
        /// Crea un nuevo activo patrimonial.
        /// </summary>
        /// <param name="registro">Información del activo a registrar.</param>
        /// <returns>El activo registrado.</returns>
        /// <response code="200">Activo registrado correctamente.</response>
        /// <response code="400">La solicitud es inválida.</response>
        /// <response code="401">No autorizado.</response>
        [HttpPost]
        [ProducesResponseType(typeof(Activo), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<Activo>> Agregar([FromBody] ActivoCrearCommand command)
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
        /// Actualiza la información de un activo existente.
        /// </summary>
        /// <param name="id">Identificador del activo a modificar.</param>
        /// <param name="registro">Datos actualizados del activo.</param>
        /// <returns>El activo actualizado.</returns>
        /// <response code="200">Activo modificado exitosamente.</response>
        /// <response code="400">La solicitud es inválida.</response>
        /// <response code="401">No autorizado.</response>
        [HttpPut("{id:long}")]
        [ProducesResponseType(typeof(Activo), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<Activo>> Modificar(long id, [FromBody] ActivoModificarCommand command)
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
        /// Elimina un activo patrimonial.
        /// </summary>
        /// <param name="id">Identificador del activo a eliminar.</param>
        /// <returns>Resultado de la operación.</returns>
        /// <response code="200">Activo eliminado correctamente.</response>
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
                await sender.Send(new ActivoEliminarCommand(id));
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
