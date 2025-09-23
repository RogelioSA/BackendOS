using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nest.core.aplicacion.security.UsuarioEmpresaServices;
using nest.core.dominio;
using nest.core.dominio.Security.UsuarioEmpresa;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace nest.core.security.Controllers
{
    /// <summary>
    /// Controlador para administrar las relaciones entre usuarios y empresas.
    /// Permite realizar operaciones CRUD y seleccionar la empresa activa para un usuario.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsuarioEmpresaController : Controller
    {
        private readonly UsuarioEmpresaService service;
        private readonly ILogger<UsuarioEmpresaController> logger;

        /// <summary>
        /// Inicializa una nueva instancia del controlador <see cref="UsuarioEmpresaController"/>.
        /// </summary>
        /// <param name="service">Servicio que gestiona las relaciones usuario-empresa.</param>
        /// <param name="logger">Logger para registrar eventos y errores.</param>
        public UsuarioEmpresaController(UsuarioEmpresaService service, ILogger<UsuarioEmpresaController> logger)
        {
            this.service = service;
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
        /// Obtiene una relación usuario-empresa por su identificador.
        /// </summary>
        /// <param name="id">Identificador de la relación usuario-empresa.</param>
        /// <returns>Relación usuario-empresa encontrada.</returns>
        [HttpGet("{id:long}")]
        [ProducesResponseType(typeof(UsuarioEmpresa), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<UsuarioEmpresa>> ObtenerPorId(long id)
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
                var data = await service.ObtenerByUsuarioIdAsync(usuarioId);
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
        /// <param name="registro">Información de la relación usuario-empresa.</param>
        /// <returns>Relación creada.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(UsuarioEmpresa), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<UsuarioEmpresa>> Agregar([FromBody] UsuarioEmpresaCrearDto registro)
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
        /// Modifica una relación usuario-empresa existente.
        /// </summary>
        /// <param name="id">Identificador de la relación a modificar.</param>
        /// <param name="registro">Datos actualizados de la relación.</param>
        /// <returns>Relación modificada.</returns>
        [HttpPut("{id:long}")]
        [ProducesResponseType(typeof(UsuarioEmpresa), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<UsuarioEmpresa>> Modificar(long id, [FromBody] UsuarioEmpresaCrearDto registro)
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
        /// Elimina una relación usuario-empresa.
        /// </summary>
        /// <param name="id">Identificador de la relación a eliminar.</param>
        /// <returns>Resultado de la operación.</returns>
        [HttpDelete("{id:long}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult> Eliminar(long id)
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

        /// <summary>
        /// Selecciona la empresa activa para un usuario.
        /// </summary>
        /// <param name="registro">Datos con el usuario y la empresa a seleccionar.</param>
        /// <returns>Resultado de la operación.</returns>
        [HttpPost("seleccionar")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult> SeleccionarEmpresa([FromBody] UsuarioEmpresaSeleccionarDto registro)
        {
            try
            {
                await service.Seleccionar(registro);
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
