using Microsoft.AspNetCore.Mvc;
using nest.core.dominio;
using nest.core.dominio.Logistica.AlmacenEN;
using Microsoft.AspNetCore.Authorization;
using nest.core.aplicacion.logistica.AlmacenServices;

namespace nest.core.logistica.Controllers
{
    /// <summary>
    /// Controlador para la gestión de almacenes.
    /// Permite realizar operaciones CRUD y obtener almacenes activos.
    /// Requiere autorización para acceder.
    /// </summary>
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class AlmacenController : ControllerBase
    {
        private readonly AlmacenService service;
        private readonly ILogger<AlmacenController> logger;

        /// <summary>
        /// Constructor del controlador AlmacenController.
        /// </summary>
        /// <param name="service">Servicio para gestionar almacenes.</param>
        /// <param name="logger">Logger para registrar eventos y errores.</param>
        public AlmacenController(AlmacenService service, ILogger<AlmacenController> logger)
        {
            this.service = service;
            this.logger = logger;
        }

        /// <summary>
        /// Obtiene todos los almacenes registrados.
        /// </summary>
        /// <returns>Lista de almacenes.</returns>
        /// <response code="200">Devuelve la lista de almacenes.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<Almacen>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<Almacen>>> ObtenerTodos()
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
        /// Obtiene un almacén por su ID.
        /// </summary>
        /// <param name="id">ID del almacén.</param>
        /// <returns>Almacén correspondiente al ID.</returns>
        /// <response code="200">Almacén encontrado.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Almacen), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<Almacen>> ObtenerPorId(int id)
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
        /// Obtiene todos los almacenes activos.
        /// </summary>
        /// <returns>Lista de almacenes activos.</returns>
        /// <response code="200">Devuelve la lista de almacenes activos.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpGet("activos")]
        [ProducesResponseType(typeof(List<Almacen>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<Almacen>>> ObtenerActivos()
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
        /// Agrega un nuevo almacén.
        /// </summary>
        /// <param name="registro">DTO con la información del almacén a crear.</param>
        /// <returns>Almacén creado.</returns>
        /// <response code="200">Almacén agregado exitosamente.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpPost]
        [ProducesResponseType(typeof(Almacen), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<Almacen>> Agregar([FromBody] AlmacenCrearDto registro)
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
        /// Modifica un almacén existente.
        /// </summary>
        /// <param name="id">ID del almacén a modificar.</param>
        /// <param name="registro">DTO con la información actualizada del almacén.</param>
        /// <returns>Almacén modificado.</returns>
        /// <response code="200">Almacén modificado exitosamente.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Almacen), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<Almacen>> Modificar(int id, [FromBody] AlmacenCrearDto registro)
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
        /// Elimina un almacén.
        /// </summary>
        /// <param name="id">ID del almacén a eliminar.</param>
        /// <returns>True si la eliminación fue exitosa.</returns>
        /// <response code="200">Almacén eliminado exitosamente.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult> Eliminar(int id)
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
