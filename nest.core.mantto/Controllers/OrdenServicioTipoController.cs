using Microsoft.AspNetCore.Mvc;
using nest.core.dominio;
using nest.core.dominio.Mantto.OrdenServicioTipoEntities;
using Microsoft.AspNetCore.Authorization;
using nest.core.aplicacion.mantto.OrdenServicioTipoServices;

namespace nest.core.mantto.Controllers
{
    /// <summary>
    /// Controlador para la gestión de tipos de orden de servicio.
    /// Permite realizar operaciones CRUD y obtener registros activos.
    /// Requiere autorización para acceder.
    /// </summary>
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class OrdenServicioTipoController : ControllerBase
    {
        private readonly OrdenServicioTipoService service;
        private readonly ILogger<OrdenServicioTipoController> logger;

        /// <summary>
        /// Constructor del controlador OrdenServicioTipoController.
        /// </summary>
        /// <param name="service">Servicio para gestionar tipos de orden de servicio.</param>
        /// <param name="logger">Logger para registrar eventos y errores.</param>
        public OrdenServicioTipoController(OrdenServicioTipoService service, ILogger<OrdenServicioTipoController> logger)
        {
            this.service = service;
            this.logger = logger;
        }

        /// <summary>
        /// Obtiene todos los tipos de orden de servicio registrados.
        /// </summary>
        /// <returns>Lista de tipos de orden de servicio.</returns>
        /// <response code="200">Devuelve la lista de tipos de orden de servicio.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<OrdenServicioTipo>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<OrdenServicioTipo>>> ObtenerTodos()
        {
            try
            {
                var data = await this.service.ObtenerTodos();
                return Ok(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Obtiene un tipo de orden de servicio por su ID.
        /// </summary>
        /// <param name="id">ID del tipo de orden de servicio.</param>
        /// <returns>Tipo de orden de servicio correspondiente al ID.</returns>
        /// <response code="200">Tipo de orden de servicio encontrado.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(OrdenServicioTipo), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<OrdenServicioTipo>> ObtenerPorId(short id)
        {
            try
            {
                var data = await this.service.ObtenerPorId(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Obtiene todos los tipos de orden de servicio activos.
        /// </summary>
        /// <returns>Lista de tipos de orden de servicio activos.</returns>
        /// <response code="200">Devuelve la lista de tipos de orden de servicio activos.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpGet("activos")]
        [ProducesResponseType(typeof(List<OrdenServicioTipo>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<OrdenServicioTipo>>> ObtenerActivos()
        {
            try
            {
                var data = await this.service.ObtenerActivos();
                return Ok(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Agrega un nuevo tipo de orden de servicio.
        /// </summary>
        /// <param name="registro">DTO con la información del tipo de orden de servicio a crear.</param>
        /// <returns>Tipo de orden de servicio creado.</returns>
        /// <response code="200">Tipo de orden de servicio agregado exitosamente.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpPost]
        [ProducesResponseType(typeof(OrdenServicioTipo), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<OrdenServicioTipo>> Agregar([FromBody] OrdenServicioTipoCrearDto registro)
        {
            try
            {
                var data = await this.service.Agregar(registro);
                return Ok(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Modifica un tipo de orden de servicio existente.
        /// </summary>
        /// <param name="id">ID del tipo de orden de servicio a modificar.</param>
        /// <param name="registro">DTO con la información actualizada del tipo de orden de servicio.</param>
        /// <returns>Tipo de orden de servicio modificado.</returns>
        /// <response code="200">Tipo de orden de servicio modificado exitosamente.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(OrdenServicioTipo), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<OrdenServicioTipo>> Modificar(short id, [FromBody] OrdenServicioTipoCrearDto registro)
        {
            try
            {
                var data = await this.service.Modificar(id, registro);
                return Ok(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Elimina un tipo de orden de servicio.
        /// </summary>
        /// <param name="id">ID del tipo de orden de servicio a eliminar.</param>
        /// <returns>True si la eliminación fue exitosa.</returns>
        /// <response code="200">Tipo de orden de servicio eliminado exitosamente.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult> Eliminar(short id)
        {
            try
            {
                await this.service.Eliminar(id);
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
