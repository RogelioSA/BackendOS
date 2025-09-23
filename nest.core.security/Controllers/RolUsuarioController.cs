using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nest.core.aplicacion.security.Security;
using nest.core.dominio;

namespace nest.core.security.Controllers
{
    /// <summary>
    /// Controlador para la gestión de la asignación de usuarios a roles.
    /// Permite asociar múltiples usuarios a un rol específico.
    /// Requiere autorización para acceder.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class RolUsuarioController : Controller
    {
        private readonly RoleUsuarioService service;
        private readonly ILogger<RolUsuarioController> logger;

        /// <summary>
        /// Constructor del controlador RolUsuarioController.
        /// </summary>
        /// <param name="service">Servicio para gestionar la asignación de usuarios a roles.</param>
        /// <param name="logger">Logger para registrar eventos y errores.</param>
        public RolUsuarioController(RoleUsuarioService service, ILogger<RolUsuarioController> logger)
        {
            this.service = service;
            this.logger = logger;
        }

        /// <summary>
        /// Asocia una lista de usuarios a un rol específico.
        /// </summary>
        /// <param name="roleName">Nombre del rol al cual se asignarán los usuarios.</param>
        /// <param name="usersId">Lista de identificadores de usuarios a asignar.</param>
        /// <returns>True si la operación fue exitosa.</returns>
        /// <response code="200">Usuarios asignados correctamente al rol.</response>
        /// <response code="400">Error en la solicitud.</response>
        /// <response code="401">No autorizado.</response>
        [HttpPost("{roleName}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<bool>> Merge(string roleName, [FromBody] List<string> usersId)
        {
            try
            {
                await this.service.Merge(roleName, usersId);
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
