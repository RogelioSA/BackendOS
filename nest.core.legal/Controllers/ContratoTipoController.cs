using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nest.core.aplicacion.legal.ContratoTipoServices;
using nest.core.dominio;
using nest.core.dominio.Legal.ContratoTipoEntities;

namespace nest.core.legal.Controllers
{
    /// <summary>
    /// Controlador para la gestión de tipos de contrato.
    /// </summary>
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class ContratoTipoController : ControllerBase
    {
        private readonly ContratoTipoService service;
        private readonly ILogger<ContratoTipoController> logger;

        public ContratoTipoController(ContratoTipoService service, ILogger<ContratoTipoController> logger)
        {
            this.service = service;
            this.logger = logger;
        }

        /// <summary>
        /// Obtiene todos los tipos de contrato registrados.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<ContratoTipo>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<ContratoTipo>>> ObtenerTodos()
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
        /// Obtiene un tipo de contrato por su ID.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ContratoTipo), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<ContratoTipo>> ObtenerPorId(byte id)
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
        /// Agrega un nuevo tipo de contrato.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ContratoTipo), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<ContratoTipo>> Agregar([FromBody] ContratoTipoCrearDto registro)
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
        /// Modifica un tipo de contrato existente.
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ContratoTipo), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<ContratoTipo>> Modificar(byte id, [FromBody] ContratoTipoCrearDto registro)
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
        /// Elimina un tipo de contrato.
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult> Eliminar(byte id)
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
