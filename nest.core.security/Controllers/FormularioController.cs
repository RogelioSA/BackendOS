using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nest.core.aplicacion.security.Aplicacion;
using nest.core.dominio;
using nest.core.dominio.Aplicacion.Formulario;

namespace nest.core.security.Controllers
{
    /// <summary>
    /// Controlador para la gestión de formularios.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class FormularioController : Controller
    {
        private readonly FormularioService service;
        private readonly ILogger<FormularioController> logger;

        public FormularioController(FormularioService service, ILogger<FormularioController> logger)
        {
            this.service = service;
            this.logger = logger;
        }

        /// <summary>
        /// Obtiene todos los formularios.
        /// </summary>
        /// <returns>Lista de formularios.</returns>
        /// <response code="200">Formularios obtenidos exitosamente.</response>
        /// <response code="400">Error en el servidor al obtener los formularios.</response>
        /// <response code="401">No autorizado.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<Formulario>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<List<Formulario>>> ObtenerTodos()
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
        /// Obtiene un formulario por su identificador.
        /// </summary>
        /// <param name="id">ID del formulario.</param>
        /// <returns>Un formulario.</returns>
        /// <response code="200">Formulario encontrado.</response>
        /// <response code="400">Error en el servidor al obtener el formulario.</response>
        /// <response code="401">No autorizado.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Formulario), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<Formulario>> ObtenerPorId(int id)
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
        /// Obtiene los formularios asociados a un módulo.
        /// </summary>
        /// <param name="moduloId">ID del módulo.</param>
        /// <returns>Lista de formularios.</returns>
        /// <response code="200">Formularios del módulo obtenidos exitosamente.</response>
        /// <response code="400">Error al obtener formularios por módulo.</response>
        /// <response code="401">No autorizado.</response>
        [HttpGet("modulo/{moduloId}")]
        [ProducesResponseType(typeof(List<Formulario>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<List<Formulario>>> ObtenerPorModuloId(int moduloId)
        {
            try
            {
                var data = await this.service.ObtenerPorModuloId(moduloId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Obtiene un formulario aplicando filtros por propiedad.
        /// </summary>
        /// <param name="filtros">Diccionario con nombre de propiedad y valor a filtrar.</param>
        /// <returns>Formulario filtrado.</returns>
        /// <response code="200">Formulario encontrado.</response>
        /// <response code="400">Error al obtener formulario por filtros.</response>
        /// <response code="401">No autorizado.</response>
        [HttpPost("filter")]
        [ProducesResponseType(typeof(Formulario), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<Formulario>> ObtenerPorUnaPropiedad([FromBody] Dictionary<string, object> filtros)
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
        /// Obtiene los formularios asociados a un rol.
        /// </summary>
        /// <param name="roleId">ID del rol.</param>
        /// <returns>Lista de formularios.</returns>
        /// <response code="200">Formularios del rol obtenidos exitosamente.</response>
        /// <response code="400">Error al obtener formularios por rol.</response>
        /// <response code="401">No autorizado.</response>
        [HttpGet("rol/{roleId}")]
        [ProducesResponseType(typeof(List<Formulario>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<List<Formulario>>> ObtenerPorRolId(string roleId)
        {
            try
            {
                var data = await this.service.ObtenerPorRolId(roleId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Crea un nuevo formulario.
        /// </summary>
        /// <param name="registro">Datos del formulario a crear.</param>
        /// <returns>Formulario creado.</returns>
        /// <response code="200">Formulario creado exitosamente.</response>
        /// <response code="400">Error al crear el formulario.</response>
        /// <response code="401">No autorizado.</response>
        [HttpPost]
        [ProducesResponseType(typeof(Formulario), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<Formulario>> Agregar([FromBody] FormularioCrearDto registro)
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
        /// Modifica un formulario existente.
        /// </summary>
        /// <param name="id">ID del formulario.</param>
        /// <param name="registro">Datos actualizados del formulario.</param>
        /// <returns>Formulario modificado.</returns>
        /// <response code="200">Formulario modificado exitosamente.</response>
        /// <response code="400">Error al modificar el formulario.</response>
        /// <response code="401">No autorizado.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Formulario), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<Formulario>> Modificar(int id, [FromBody] FormularioCrearDto registro)
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
        /// Elimina un formulario.
        /// </summary>
        /// <param name="id">ID del formulario a eliminar.</param>
        /// <returns>Resultado de la operación.</returns>
        /// <response code="200">Formulario eliminado exitosamente.</response>
        /// <response code="400">Error al eliminar el formulario.</response>
        /// <response code="401">No autorizado.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        [ProducesResponseType(401)]
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
