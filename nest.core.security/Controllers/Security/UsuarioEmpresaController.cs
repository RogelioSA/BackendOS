using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nest.core.aplicacion.security.UsuarioEmpresas.Commands;
using nest.core.aplicacion.security.UsuarioEmpresas.Queries;
using nest.core.dominio;
using nest.core.dominio.Security.UsuarioEmpresa;
using nest.core.security.Extensions;

namespace nest.core.security.Controllers.Security
{
    /// <summary>
    /// Controlador para administrar las relaciones entre usuarios y empresas.
    /// Permite realizar operaciones CRUD y seleccionar la empresa activa para un usuario.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiExplorerSettings(GroupName = ConfigureEndpoints.EndpointsEnum.SECURITY)]
    [Route($"{ConfigureEndpoints.EndpointsEnum.SECURITY}/[controller]")]
    public class UsuarioEmpresaController : Controller
    {
        private readonly IMediator mediator;
        private readonly ILogger<UsuarioEmpresaController> logger;

        /// <summary>
        /// Inicializa una nueva instancia del controlador <see cref="UsuarioEmpresaController"/>.
        /// </summary>
        /// <param name="mediator">Mediador que orquesta comandos y consultas de usuario-empresa.</param>
        /// <param name="logger">Logger para registrar eventos y errores.</param>
        public UsuarioEmpresaController(IMediator mediator, ILogger<UsuarioEmpresaController> logger)
        {
            this.mediator = mediator;
            this.logger = logger;
        }

        /// <summary>
        /// Obtiene todas las relaciones usuario-empresa registradas.
        /// </summary>
        /// <returns>Lista de relaciones usuario-empresa.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<UsuarioEmpresa>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<UsuarioEmpresa>>> ObtenerTodos()
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
        /// Obtiene una relación usuario-empresa por su identificador.
        /// </summary>
        /// <param name="id">Identificador de la relación usuario-empresa.</param>
        /// <returns>Relación usuario-empresa encontrada.</returns>
        [HttpGet("{id:long}")]
        [ProducesResponseType(typeof(UsuarioEmpresa), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<UsuarioEmpresa?>> ObtenerPorId(long id)
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
        /// Obtiene las relaciones usuario-empresa asociadas a un usuario específico.
        /// </summary>
        /// <param name="usuarioId">Identificador del usuario.</param>
        /// <returns>Lista de relaciones usuario-empresa para el usuario.</returns>
        [HttpGet("usuario/{usuarioId}")]
        [ProducesResponseType(typeof(List<UsuarioEmpresa>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<UsuarioEmpresa>>> ObtenerPorUsuario(string usuarioId)
        {
            try
            {
                var data = await mediator.Send(new ObtenerPorUsuarioIdQuery(usuarioId));
                return Ok(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Registra una nueva relación usuario-empresa.
        /// </summary>
        /// <param name="comando">Comando con la información de la relación usuario-empresa.</param>
        /// <returns>Relación creada.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(UsuarioEmpresa), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<UsuarioEmpresa>> Agregar([FromBody] UsuarioEmpresaCrearCommand comando)
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
        /// Modifica una relación usuario-empresa existente.
        /// </summary>
        /// <param name="comando">Comando con los datos actualizados de la relación.</param>
        /// <returns>Relación modificada.</returns>
        [HttpPut]
        [ProducesResponseType(typeof(UsuarioEmpresa), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<UsuarioEmpresa>> Modificar([FromBody] UsuarioEmpresaModificarCommand comando)
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
        /// Elimina una relación usuario-empresa.
        /// </summary>
        /// <param name="comando">Comando con el identificador de la relación a eliminar.</param>
        /// <returns>Resultado de la operación.</returns>
        [HttpDelete]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult> Eliminar([FromBody] UsuarioEmpresaEliminarCommand comando)
        {
            try
            {
                await mediator.Send(comando);
                return Ok(true);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Selecciona la empresa activa para un usuario.
        /// </summary>
        /// <param name="comando">Comando con el usuario y la empresa a seleccionar.</param>
        /// <returns>Resultado de la operación.</returns>
        [HttpPost("seleccionar")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult> SeleccionarEmpresa([FromBody] UsuarioEmpresaSeleccionarCommand comando)
        {
            try
            {
                await mediator.Send(comando);
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
