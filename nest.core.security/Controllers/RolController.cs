using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nest.core.aplicacion.security.Aplicacion;
using nest.core.aplicacion.security.Security;
using nest.core.dominio.Aplicacion.Modulo;
using nest.core.dominio;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using nest.core.dominio.Security;
using AutoMapper;

namespace nest.core.security.Controllers
{
    /// <summary>
    /// Controlador para la gestión de roles de aplicación.
    /// Permite obtener, agregar, modificar y eliminar roles.
    /// Requiere autorización para acceder.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class RolController : Controller
    {
        private readonly RoleService service;
        private readonly ILogger<RolController> logger;

        /// <summary>
        /// Constructor del controlador RolController.
        /// </summary>
        /// <param name="service">Servicio para gestionar roles.</param>
        /// <param name="logger">Logger para registrar eventos y errores.</param>
        public RolController(RoleService service, ILogger<RolController> logger)
        {
            this.service = service;
            this.logger = logger;
        }

        /// <summary>
        /// Obtiene la lista de todos los roles.
        /// </summary>
        /// <returns>Lista de roles.</returns>
        /// <response code="200">Lista de roles obtenida correctamente.</response>
        /// <response code="400">Error en la solicitud.</response>
        /// <response code="401">No autorizado.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<ApplicationRole>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<List<ApplicationRole>>> ObtenerTodos()
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
        /// Obtiene un rol por su identificador.
        /// </summary>
        /// <param name="id">Identificador del rol.</param>
        /// <returns>El rol solicitado.</returns>
        /// <response code="200">Rol obtenido correctamente.</response>
        /// <response code="400">Error en la solicitud.</response>
        /// <response code="401">No autorizado.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApplicationRole), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<ApplicationRole>> ObtenerPorId(string id)
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
        /// Agrega un nuevo rol.
        /// </summary>
        /// <param name="registro">Datos del rol a agregar.</param>
        /// <returns>El rol agregado.</returns>
        /// <response code="200">Rol agregado correctamente.</response>
        /// <response code="400">Error en la solicitud.</response>
        /// <response code="401">No autorizado.</response>
        [HttpPost]
        [ProducesResponseType(typeof(ApplicationRole), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<ApplicationRole>> Agregar([FromBody] ApplicationRoleDto registro)
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
        /// Modifica un rol existente.
        /// </summary>
        /// <param name="registro">Datos del rol a modificar.</param>
        /// <param name="roleId">Id del rol.</param>
        /// <returns>El rol modificado.</returns>
        /// <response code="200">Rol modificado correctamente.</response>
        /// <response code="400">Error en la solicitud.</response>
        /// <response code="401">No autorizado.</response>
        [HttpPut("{roleId}")]
        [ProducesResponseType(typeof(ApplicationRole), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<ApplicationRole>> Modificar([FromBody] ApplicationRoleDto registro, int roleId)
        {
            try
            {
                var data = await this.service.Modificar(registro, roleId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Elimina un rol.
        /// </summary>
        /// <param name="roleId">Id del rol a eliminar.</param>
        /// <returns>Verdadero si se eliminó correctamente.</returns>
        /// <response code="200">Rol eliminado correctamente.</response>
        /// <response code="400">Error en la solicitud.</response>
        /// <response code="401">No autorizado.</response>
        [HttpDelete("{roleId}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult> Eliminar(int roleId)
        {
            try
            {
                await this.service.Eliminar(roleId);
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
