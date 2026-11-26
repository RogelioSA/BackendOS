using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nest.core.aplicacion.security.Usuarios.Commands;
using nest.core.aplicacion.security.Usuarios.Queries;
using nest.core.dominio;
using nest.core.dominio.Security;
using nest.core.security.Extensions;

namespace nest.core.security.Controllers.Security
{
    /// <summary>
    /// Controlador para la gestión de usuarios.
    /// Permite operaciones CRUD y consulta por rol.
    /// Requiere autorización para acceder.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiExplorerSettings(GroupName = ConfigureEndpoints.EndpointsEnum.SECURITY)]
    [Route($"{ConfigureEndpoints.EndpointsEnum.SECURITY}/[controller]")]
    public class UsuarioController : Controller
    {
        private readonly IMediator mediator;
        private readonly ILogger<UsuarioController> logger;

        /// <summary>
        /// Constructor del controlador UsuarioController.
        /// </summary>
        /// <param name="mediator">Mediador para gestionar comandos y consultas de usuarios.</param>
        /// <param name="logger">Logger para registrar eventos y errores.</param>
        public UsuarioController(IMediator mediator, ILogger<UsuarioController> logger)
        {
            this.mediator = mediator;
            this.logger = logger;
        }

        /// <summary>
        /// Obtiene todos los usuarios registrados.
        /// </summary>
        /// <returns>Lista de usuarios.</returns>
        /// <response code="200">Devuelve la lista de usuarios.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<ApplicationUser>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<ApplicationUser>>> ObtenerTodos()
        {
            try
            {
                var data = await mediator.Send(new ObtenerTodosQuery());
                return Ok(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Obtiene todos los usuarios registrados.
        /// </summary>
        /// <returns>Lista de usuarios.</returns>
        /// <response code="200">Devuelve la lista de usuarios.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpPost("filter")]
        [ProducesResponseType(typeof(List<ApplicationUser>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<LoadResult>> ObtenerFiltro(DataSourceLoadOptionsBase loadOptions)
        {
            try
            {
                var data = await mediator.Send(new ObtenerFilterQuery(loadOptions));
                return Ok(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Obtiene un usuario por su ID.
        /// </summary>
        /// <param name="id">ID del usuario.</param>
        /// <returns>Usuario correspondiente al ID.</returns>
        /// <response code="200">Usuario encontrado.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApplicationUser), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<ApplicationUser?>> ObtenerPorId(string id)
        {
            try
            {
                var data = await mediator.Send(new ObtenerPorIdQuery(id));
                return Ok(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Agrega un nuevo usuario.
        /// </summary>
        /// <param name="comando">Comando con la información del usuario y su contraseña.</param>
        /// <returns>Usuario creado.</returns>
        /// <response code="200">Usuario agregado exitosamente.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpPost]
        [ProducesResponseType(typeof(ApplicationUser), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<ApplicationUser>> Agregar([FromBody] UsuarioCrearCommand comando)
        {
            try
            {
                var data = await mediator.Send(comando);
                return Ok(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Modifica un usuario existente.
        /// </summary>
        /// <param name="id">Id del usuario.</param>
        /// <param name="comando">Comando con los datos actualizados del usuario.</param>
        /// <returns>Usuario modificado.</returns>
        /// <response code="200">Usuario modificado exitosamente.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApplicationUser), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<ApplicationUser>> Modificar([FromRoute] string id, [FromBody] UsuarioModificarCommand comando)
        {
            try
            {
                var cmd = comando with { Id = id };
                var data = await mediator.Send(cmd);
                return Ok(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Elimina un usuario.
        /// </summary>
        /// <param name="id">identificador del usuario a eliminar.</param>
        /// <returns>True si la eliminación fue exitosa.</returns>
        /// <response code="200">Usuario eliminado exitosamente.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult> Eliminar([FromRoute] string id)
        {
            try
            {
                await mediator.Send(new UsuarioEliminarCommand(id));
                return Ok(true);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Obtiene todos los usuarios asignados a un rol específico.
        /// </summary>
        /// <param name="roleName">Nombre del rol.</param>
        /// <returns>Lista de usuarios con el rol especificado.</returns>
        /// <response code="200">Usuarios obtenidos correctamente.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpGet("rol/{roleName}")]
        [ProducesResponseType(typeof(List<ApplicationUser>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<ApplicationUser>>> ObtenerPorRolName(string roleName)
        {
            try
            {
                var data = await mediator.Send(new ObtenerPorRolQuery(roleName));
                return Ok(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Modifica un usuario existente.
        /// </summary>
        /// <param name="id">Id del usuario.</param>
        /// <param name="comando">Comando con los datos actualizados del usuario.</param>
        /// <returns>Usuario modificado.</returns>
        /// <response code="200">Usuario modificado exitosamente.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpPut("resetpw")]
        [ProducesResponseType(typeof(ApplicationUser), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<ApplicationUser>> ResetPw([FromBody] UsuarioResetPwCommand comando)
        {
            try
            {
                var data = await mediator.Send(comando);
                return Ok(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
