using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nest.core.aplicacion.security.Security;
using nest.core.dominio.Security;
using nest.core.dominio;

namespace nest.core.security.Controllers
{
    /// <summary>
    /// Controlador para la gestión de usuarios.
    /// Permite operaciones CRUD y consulta por rol.
    /// Requiere autorización para acceder.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : Controller
    {
        private readonly UsuarioService service;
        private readonly ILogger<UsuarioController> logger;

        /// <summary>
        /// Constructor del controlador UsuarioController.
        /// </summary>
        /// <param name="service">Servicio para gestionar usuarios.</param>
        /// <param name="logger">Logger para registrar eventos y errores.</param>
        public UsuarioController(UsuarioService service, ILogger<UsuarioController> logger)
        {
            this.service = service;
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
        /// Obtiene un usuario por su ID.
        /// </summary>
        /// <param name="id">ID del usuario.</param>
        /// <returns>Usuario correspondiente al ID.</returns>
        /// <response code="200">Usuario encontrado.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApplicationUser), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<ApplicationUser>> ObtenerPorId(string id)
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
        /// Agrega un nuevo usuario.
        /// </summary>
        /// <param name="registro">DTO con la información del usuario y su contraseña.</param>
        /// <returns>Usuario creado.</returns>
        /// <response code="200">Usuario agregado exitosamente.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpPost]
        [ProducesResponseType(typeof(ApplicationUser), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<ApplicationUser>> Agregar([FromBody] ApplicationUserDto registro)
        {
            try
            {
                var data = await this.service.Agregar(registro.User, registro.Password);
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
        /// <param name="registro">Usuario con los datos actualizados.</param>
        /// <returns>Usuario modificado.</returns>
        /// <response code="200">Usuario modificado exitosamente.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpPut]
        [ProducesResponseType(typeof(ApplicationUser), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<ApplicationUser>> Modificar([FromBody] ApplicationUser registro)
        {
            try
            {
                var data = await this.service.Modificar(registro);
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
        /// <param name="registro">Usuario a eliminar.</param>
        /// <returns>True si la eliminación fue exitosa.</returns>
        /// <response code="200">Usuario eliminado exitosamente.</response>
        /// <response code="400">Error en la solicitud.</response>
        [HttpDelete]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult> Eliminar([FromBody] ApplicationUser registro)
        {
            try
            {
                await this.service.Eliminar(registro);
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
                var data = await this.service.ObtenerPorRol(roleName);
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
