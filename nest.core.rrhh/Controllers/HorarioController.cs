using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nest.core.aplicacion.rrhh.HorarioServices;
using nest.core.dominio;
using nest.core.dominio.RRHH.HorarioCabeceraEntities;

namespace nest.core.rrhh.Controllers
{
    /// <summary>
    /// Controlador para la gestión de horarios.
    /// Permite realizar operaciones CRUD sobre la cabecera de horarios.
    /// Requiere autorización para acceder.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class HorarioController : ControllerBase
    {
        private readonly HorarioService service;
        private readonly ILogger<HorarioController> logger;

        /// <summary>
        /// Constructor del controlador HorarioController.
        /// </summary>
        /// <param name="service">Servicio para gestionar horarios.</param>
        /// <param name="logger">Logger para registrar eventos y errores.</param>
        public HorarioController(HorarioService service, ILogger<HorarioController> logger)
        {
            this.service = service;
            this.logger = logger;
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
        public async Task<ActionResult<List<HorarioCabecera>>> ObtenerTodos()
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
        /// Obtiene un horario por su ID.
        /// </summary>
        /// <param name="id">ID del horario.</param>
        /// <returns>Horario correspondiente al ID.</returns>
        /// <response code="200">Horario encontrado.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(HorarioCabecera), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<HorarioCabecera>> ObtenerPorId(int id)
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
        /// Agrega un nuevo horario.
        /// </summary>
        /// <param name="registro">DTO con la información del horario a crear.</param>
        /// <returns>Horario creado.</returns>
        /// <response code="200">Horario agregado exitosamente.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpPost]
        [ProducesResponseType(typeof(HorarioCabecera), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<HorarioCabecera>> Agregar([FromBody] HorarioDto registro)
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
        /// Modifica un horario existente.
        /// </summary>
        /// <param name="id">ID del horario a modificar.</param>
        /// <param name="registro">DTO con la información actualizada del horario.</param>
        /// <returns>Horario modificado.</returns>
        /// <response code="200">Horario modificado exitosamente.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(HorarioCabecera), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<HorarioCabecera>> Modificar(int id, [FromBody] HorarioDto registro)
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
        /// Elimina un horario.
        /// </summary>
        /// <param name="id">ID del horario a eliminar.</param>
        /// <returns>True si la eliminación fue exitosa.</returns>
        /// <response code="200">Horario eliminado exitosamente.</response>
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
