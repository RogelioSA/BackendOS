using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nest.core.aplicacion.mantto.Labores.Commands;
using nest.core.aplicacion.mantto.Labores.Queries;
using nest.core.dominio;
using nest.core.dominio.Mantto.LaborEntities;
using nest.core.security.Extensions;

namespace nest.core.security.Controllers.Mantto
{
    /// <summary>
    /// Controlador para la gestión de labores de mantenimiento.
    /// Permite realizar operaciones CRUD y obtener labores activas.
    /// Requiere autorización para acceder.
    /// </summary>
    [Authorize]
    [ApiExplorerSettings(GroupName = ConfigureEndpoints.EndpointsEnum.MANTTO)]
    [Route($"{ConfigureEndpoints.EndpointsEnum.MANTTO}/[controller]")]
    [ApiController]
    public class LaborController : ControllerBase
    {
        private readonly ISender sender;
        private readonly ILogger<LaborController> logger;

        /// <summary>
        /// Constructor del controlador LaborController.
        /// </summary>
        /// <param name="service">Servicio para gestionar las labores.</param>
        /// <param name="logger">Logger para registrar eventos y errores.</param>
        public LaborController(ISender sender, ILogger<LaborController> logger)
        {
            this.sender = sender;
            this.logger = logger;
        }

        /// <summary>
        /// Obtiene todas las labores registradas.
        /// </summary>
        /// <returns>Lista de labores.</returns>
        /// <response code="200">Devuelve la lista de labores.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<Labor>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<Labor>>> ObtenerTodos()
        {
            try
            {
                var data = await sender.Send(new ObtenerTodosQuery());
                return Ok(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Obtiene una labor por su ID.
        /// </summary>
        /// <param name="id">ID de la labor.</param>
        /// <returns>Labor correspondiente al ID.</returns>
        /// <response code="200">Labor encontrada.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Labor), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<Labor>> ObtenerPorId(int id)
        {
            try
            {
                var data = await sender.Send(new ObtenerPorIdQuery(id));
                return Ok(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Obtiene todas las labores activas.
        /// </summary>
        /// <returns>Lista de labores activas.</returns>
        /// <response code="200">Devuelve la lista de labores activas.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpGet("activos")]
        [ProducesResponseType(typeof(List<Labor>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<Labor>>> ObtenerActivos()
        {
            try
            {
                var data = await sender.Send(new ObtenerActivosQuery());
                return Ok(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Agrega una nueva labor.
        /// </summary>
        /// <param name="registro">DTO con la información de la labor a crear.</param>
        /// <returns>Labor creada.</returns>
        /// <response code="200">Labor agregada exitosamente.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpPost]
        [ProducesResponseType(typeof(Labor), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<Labor>> Agregar([FromBody] LaborCrearCommand command)
        {
            try
            {
                var data = await sender.Send(command);
                return Ok(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Modifica una labor existente.
        /// </summary>
        /// <param name="id">ID de la labor a modificar.</param>
        /// <param name="registro">DTO con la información actualizada de la labor.</param>
        /// <returns>Labor modificada.</returns>
        /// <response code="200">Labor modificada exitosamente.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Labor), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<Labor>> Modificar(int id, [FromBody] LaborModificarCommand command)
        {
            try
            {
                var data = await sender.Send(command with { Id = id });
                return Ok(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Elimina una labor.
        /// </summary>
        /// <param name="id">ID de la labor a eliminar.</param>
        /// <returns>True si la eliminación fue exitosa.</returns>
        /// <response code="200">Labor eliminada exitosamente.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult> Eliminar(int id)
        {
            try
            {
                await sender.Send(new LaborEliminarCommand(id));
                return Ok(true);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
