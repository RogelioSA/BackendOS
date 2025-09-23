using Microsoft.AspNetCore.Mvc;
using nest.core.dominio;
using nest.core.aplicacion.rrhh.GrupoHorarioServices;
using nest.core.dominio.RRHH.GrupoHorarioEntities;
using Microsoft.AspNetCore.Authorization;

namespace nest.core.rrhh.Controllers
{
    /// <summary>
    /// Controlador para la gestión de grupos horario.
    /// Permite realizar operaciones CRUD y obtener grupos horario activos.
    /// Requiere autorización para acceder.
    /// </summary>
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class GrupoHorarioController : ControllerBase
    {
        private readonly GrupoHorarioService service;
        private readonly ILogger<GrupoHorarioController> logger;

        /// <summary>
        /// Constructor del controlador GrupoHorarioController.
        /// </summary>
        /// <param name="service">Servicio para gestionar grupos horario.</param>
        /// <param name="logger">Logger para registrar eventos y errores.</param>
        public GrupoHorarioController(GrupoHorarioService service, ILogger<GrupoHorarioController> logger)
        {
            this.service = service;
            this.logger = logger;
        }

        /// <summary>
        /// Obtiene todos los grupos horario registrados.
        /// </summary>
        /// <returns>Lista de grupos horario.</returns>
        /// <response code="200">Devuelve la lista de grupos horario.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<GrupoHorario>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<GrupoHorario>>> ObtenerTodos()
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
        /// Obtiene un grupo horario por su ID.
        /// </summary>
        /// <param name="id">ID del grupo horario.</param>
        /// <returns>Grupo horario correspondiente al ID.</returns>
        /// <response code="200">Grupo horario encontrado.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GrupoHorario), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<GrupoHorario>> ObtenerPorId(int id)
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
        /// Obtiene todos los grupos horario activos.
        /// </summary>
        /// <returns>Lista de grupos horario activos.</returns>
        /// <response code="200">Devuelve la lista de grupos horario activos.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpGet("activos")]
        [ProducesResponseType(typeof(List<GrupoHorario>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<GrupoHorario>>> ObtenerActivos()
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
        /// Agrega un nuevo grupo horario.
        /// </summary>
        /// <param name="registro">DTO con la información del grupo horario a crear.</param>
        /// <returns>Grupo horario creado.</returns>
        /// <response code="200">Grupo horario agregado exitosamente.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpPost]
        [ProducesResponseType(typeof(GrupoHorario), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<GrupoHorario>> Agregar([FromBody] GrupoHorarioCrearDto registro)
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
        /// Modifica un grupo horario existente.
        /// </summary>
        /// <param name="id">ID del grupo horario a modificar.</param>
        /// <param name="registro">DTO con la información actualizada del grupo horario.</param>
        /// <returns>Grupo horario modificado.</returns>
        /// <response code="200">Grupo horario modificado exitosamente.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(GrupoHorario), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<GrupoHorario>> Modificar(int id, [FromBody] GrupoHorarioCrearDto registro)
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
        /// Elimina un grupo horario.
        /// </summary>
        /// <param name="id">ID del grupo horario a eliminar.</param>
        /// <returns>True si la eliminación fue exitosa.</returns>
        /// <response code="200">Grupo horario eliminado exitosamente.</response>
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
