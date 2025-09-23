using Microsoft.AspNetCore.Mvc;
using nest.core.dominio;
using nest.core.dominio.Mantto.MantenimientoTipoEntities;
using Microsoft.AspNetCore.Authorization;
using nest.core.aplicacion.mantto.MantenimientoTipoServices;

namespace nest.core.mantto.Controllers
{
    /// <summary>
    /// Controlador para la gestión de tipos de mantenimiento.
    /// Permite realizar operaciones CRUD y obtener registros activos.
    /// Requiere autorización para acceder.
    /// </summary>
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class MantenimientoTipoController : ControllerBase
    {
        private readonly MantenimientoTipoService service;
        private readonly ILogger<MantenimientoTipoController> logger;

        /// <summary>
        /// Constructor del controlador MantenimientoTipoController.
        /// </summary>
        /// <param name="service">Servicio para gestionar tipos de mantenimiento.</param>
        /// <param name="logger">Logger para registrar eventos y errores.</param>
        public MantenimientoTipoController(MantenimientoTipoService service, ILogger<MantenimientoTipoController> logger)
        {
            this.service = service;
            this.logger = logger;
        }

        /// <summary>
        /// Obtiene todos los tipos de mantenimiento registrados.
        /// </summary>
        /// <returns>Lista de tipos de mantenimiento.</returns>
        /// <response code="200">Devuelve la lista de tipos de mantenimiento.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<MantenimientoTipo>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<MantenimientoTipo>>> ObtenerTodos()
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
        /// Obtiene un tipo de mantenimiento por su ID.
        /// </summary>
        /// <param name="id">ID del tipo de mantenimiento.</param>
        /// <returns>Tipo de mantenimiento correspondiente al ID.</returns>
        /// <response code="200">Tipo de mantenimiento encontrado.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MantenimientoTipo), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<MantenimientoTipo>> ObtenerPorId(short id)
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
        /// Obtiene todos los tipos de mantenimiento activos.
        /// </summary>
        /// <returns>Lista de tipos de mantenimiento activos.</returns>
        /// <response code="200">Devuelve la lista de tipos de mantenimiento activos.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpGet("activos")]
        [ProducesResponseType(typeof(List<MantenimientoTipo>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<MantenimientoTipo>>> ObtenerActivos()
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
        /// Agrega un nuevo tipo de mantenimiento.
        /// </summary>
        /// <param name="registro">DTO con la información del tipo de mantenimiento a crear.</param>
        /// <returns>Tipo de mantenimiento creado.</returns>
        /// <response code="200">Tipo de mantenimiento agregado exitosamente.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpPost]
        [ProducesResponseType(typeof(MantenimientoTipo), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<MantenimientoTipo>> Agregar([FromBody] MantenimientoTipoCrearDto registro)
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
        /// Modifica un tipo de mantenimiento existente.
        /// </summary>
        /// <param name="id">ID del tipo de mantenimiento a modificar.</param>
        /// <param name="registro">DTO con la información actualizada del tipo de mantenimiento.</param>
        /// <returns>Tipo de mantenimiento modificado.</returns>
        /// <response code="200">Tipo de mantenimiento modificado exitosamente.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(MantenimientoTipo), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<MantenimientoTipo>> Modificar(short id, [FromBody] MantenimientoTipoCrearDto registro)
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
        /// Elimina un tipo de mantenimiento.
        /// </summary>
        /// <param name="id">ID del tipo de mantenimiento a eliminar.</param>
        /// <returns>True si la eliminación fue exitosa.</returns>
        /// <response code="200">Tipo de mantenimiento eliminado exitosamente.</response>
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
