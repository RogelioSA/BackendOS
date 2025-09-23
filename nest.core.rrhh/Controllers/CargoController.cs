using Microsoft.AspNetCore.Mvc;
using nest.core.dominio;
using nest.core.aplicacion.rrhh.CargoServices;
using nest.core.dominio.RRHH.CargoEntities;
using Microsoft.AspNetCore.Authorization;

namespace nest.core.rrhh.Controllers
{
    /// <summary>
    /// Controlador para la gestión de cargos.
    /// Permite realizar operaciones CRUD y obtener cargos activos.
    /// Requiere autorización para acceder.
    /// </summary>
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class CargoController : ControllerBase
    {
        private readonly CargoService service;
        private readonly ILogger<CargoController> logger;

        /// <summary>
        /// Constructor del controlador CargoController.
        /// </summary>
        /// <param name="service">Servicio para gestionar cargos.</param>
        /// <param name="logger">Logger para registrar eventos y errores.</param>
        public CargoController(CargoService service, ILogger<CargoController> logger)
        {
            this.service = service;
            this.logger = logger;
        }

        /// <summary>
        /// Obtiene todos los cargos registrados.
        /// </summary>
        /// <returns>Lista de cargos.</returns>
        /// <response code="200">Devuelve la lista de cargos.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<Cargo>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<Cargo>>> ObtenerTodos()
        {
            try
            {
                var data = await service.ObtenerTodos();
                return Ok(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Obtiene un cargo por su ID.
        /// </summary>
        /// <param name="id">ID del cargo.</param>
        /// <returns>Cargo correspondiente al ID.</returns>
        /// <response code="200">Cargo encontrado.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Cargo), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<Cargo>> ObtenerPorId(int id)
        {
            try
            {
                var data = await service.ObtenerPorId(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Obtiene todos los cargos activos.
        /// </summary>
        /// <returns>Lista de cargos activos.</returns>
        /// <response code="200">Devuelve la lista de cargos activos.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpGet("activos")]
        [ProducesResponseType(typeof(List<Cargo>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<Cargo>>> ObtenerActivos()
        {
            try
            {
                var data = await service.ObtenerActivos();
                return Ok(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Agrega un nuevo cargo.
        /// </summary>
        /// <param name="registro">DTO con la información del cargo a crear.</param>
        /// <returns>Cargo creado.</returns>
        /// <response code="200">Cargo agregado exitosamente.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpPost]
        [ProducesResponseType(typeof(Cargo), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<Cargo>> Agregar([FromBody] CargoCrearDto registro)
        {
            try
            {
                var data = await service.Agregar(registro);
                return Ok(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Modifica un cargo existente.
        /// </summary>
        /// <param name="id">ID del cargo a modificar.</param>
        /// <param name="registro">DTO con la información actualizada del cargo.</param>
        /// <returns>Cargo modificado.</returns>
        /// <response code="200">Cargo modificado exitosamente.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Cargo), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<Cargo>> Modificar(int id, [FromBody] CargoCrearDto registro)
        {
            try
            {
                var data = await service.Modificar(id, registro);
                return Ok(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Elimina un cargo.
        /// </summary>
        /// <param name="id">ID del cargo a eliminar.</param>
        /// <returns>True si la eliminación fue exitosa.</returns>
        /// <response code="200">Cargo eliminado exitosamente.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult> Eliminar(int id)
        {
            try
            {
                await service.Eliminar(id);
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
