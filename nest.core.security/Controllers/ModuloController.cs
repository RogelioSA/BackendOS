using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nest.core.aplicacion.security.Aplicacion;
using nest.core.dominio;
using nest.core.dominio.Aplicacion.Modulo;

namespace nest.core.security.Controllers
{
    /// <summary>
    /// Controlador para la gestión de módulos.
    /// Proporciona endpoints para obtener, crear, modificar y eliminar módulos.
    /// Requiere autorización para acceder a sus métodos.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ModuloController : Controller
    {
        private readonly ModuloService service;
        private readonly ILogger<ModuloController> logger;

        /// <summary>
        /// Constructor del controlador ModuloController.
        /// </summary>
        /// <param name="service">Servicio para operaciones con módulos.</param>
        /// <param name="logger">Logger para registrar eventos y errores.</param>
        public ModuloController(ModuloService service, ILogger<ModuloController> logger)
        {
            this.service = service;
            this.logger = logger;
        }

        /// <summary>
        /// Obtiene todos los módulos.
        /// </summary>
        /// <returns>Lista de módulos.</returns>
        /// <response code="200">Lista de módulos obtenida correctamente.</response>
        /// <response code="400">Error en la solicitud.</response>
        /// <response code="401">No autorizado.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<Modulo>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<List<Modulo>>> ObtenerTodos()
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
        /// Obtiene un módulo por su ID.
        /// </summary>
        /// <param name="id">ID del módulo.</param>
        /// <returns>El módulo solicitado.</returns>
        /// <response code="200">Módulo obtenido correctamente.</response>
        /// <response code="400">Error en la solicitud.</response>
        /// <response code="401">No autorizado.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Modulo), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<Modulo>> ObtenerPorId(int id)
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
        /// Obtiene un módulo que cumple con las propiedades indicadas.
        /// </summary>
        /// <param name="filtros">Diccionario con los filtros para la búsqueda.</param>
        /// <returns>Módulo que cumple con los filtros.</returns>
        /// <response code="200">Módulo obtenido correctamente.</response>
        /// <response code="400">Error en la solicitud.</response>
        /// <response code="401">No autorizado.</response>
        [HttpPost("filter")]
        [ProducesResponseType(typeof(Modulo), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<Modulo>> ObtenerPorUnaPropiedad([FromBody] Dictionary<string, object> filtros)
        {
            try
            {
                var data = await this.service.ObtenerPorUnaPropiedad(filtros);
                return Ok(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Agrega un nuevo módulo.
        /// </summary>
        /// <param name="registro">Datos para crear el módulo.</param>
        /// <returns>Módulo creado.</returns>
        /// <response code="200">Módulo creado correctamente.</response>
        /// <response code="400">Error en la solicitud.</response>
        /// <response code="401">No autorizado.</response>
        [HttpPost]
        [ProducesResponseType(typeof(Modulo), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<Modulo>> Agregar([FromBody] ModuloCrearDto registro)
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
        /// Modifica un módulo existente.
        /// </summary>
        /// <param name="id">ID del módulo a modificar.</param>
        /// <param name="registro">Datos actualizados del módulo.</param>
        /// <returns>Módulo modificado.</returns>
        /// <response code="200">Módulo modificado correctamente.</response>
        /// <response code="400">Error en la solicitud.</response>
        /// <response code="401">No autorizado.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Modulo), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<Modulo>> Modificar(int id, [FromBody] ModuloCrearDto registro)
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
        /// Elimina un módulo por su ID.
        /// </summary>
        /// <param name="id">ID del módulo a eliminar.</param>
        /// <response code="204">Módulo eliminado correctamente.</response>
        /// <response code="400">Error en la solicitud.</response>
        /// <response code="401">No autorizado.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult> Eliminar(int id)
        {
            try
            {
                await this.service.Eliminar(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
