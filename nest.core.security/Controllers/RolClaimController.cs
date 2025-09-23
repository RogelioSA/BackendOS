using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nest.core.aplicacion.security.Security;
using nest.core.dominio.Security;
using nest.core.dominio;
using System.Security.Claims;
using nest.core.dominio.Security.Dto;

namespace nest.core.security.Controllers
{
    /// <summary>
    /// Controlador para la gestión de Claims asignados a roles.
    /// Permite fusionar (merge) claims a un rol específico.
    /// Requiere autorización para acceder.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class RolClaimController : Controller
    {
        private readonly RoleClaimsService service;
        private readonly ILogger<RolClaimController> logger;

        /// <summary>
        /// Constructor del controlador RolClaimController.
        /// </summary>
        /// <param name="service">Servicio para gestionar claims de roles.</param>
        /// <param name="logger">Logger para registrar eventos y errores.</param>
        public RolClaimController(RoleClaimsService service, ILogger<RolClaimController> logger)
        {
            this.service = service;
            this.logger = logger;
        }

        /// <summary>
        /// Fusiona (merge) una lista de claims con un rol específico.
        /// </summary>
        /// <param name="roleId">Identificador del rol al que se le asignarán los claims.</param>
        /// <param name="claims">Lista de claims a asignar al rol.</param>
        /// <returns>Indica si la operación fue exitosa.</returns>
        /// <response code="200">Claims fusionados correctamente.</response>
        /// <response code="400">Error en la solicitud.</response>
        /// <response code="401">No autorizado.</response>
        [HttpPost("{roleId}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<bool>> Merge(string roleId, [FromBody] List<ClaimDto> claims)
        {
            try
            {
                List<Claim> claimsOt = claims.Select(x => new Claim(x.Type, x.Value)).ToList();
                await this.service.Merge(roleId, claimsOt);
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
