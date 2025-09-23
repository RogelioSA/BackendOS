using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nest.core.dominio;
using nest.core.aplicacion.legal.ContratoServices;
using nest.core.dominio.Legal.ContratoCabeceraEntities;

namespace nest.core.legal.Controllers
{
    /// <summary>
    /// Controlador para la gestión de contratos personales.
    /// </summary>
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class ContratoPersonalController : ControllerBase
    {
        private readonly ContratoPersonalService service;
        private readonly ILogger<ContratoPersonalController> logger;

        public ContratoPersonalController(ContratoPersonalService service, ILogger<ContratoPersonalController> logger)
        {
            this.service = service;
            this.logger = logger;
        }

        /// <summary>
        /// Obtiene todos los tipos de contrato registrados.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<ContratoCabecera>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<ContratoCabecera>>> ObtenerTodos()
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
        /// Obtiene un con trato por su Id
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ContratoCabecera), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<ContratoCabecera>> ObtenerPorId(long id)
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
        /// Agrega un nuevo contrato de personal.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ContratoCabecera), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<ContratoCabecera>> Agregar([FromBody] ContratoPersonalDto registro)
        {
            try
            {
                var data = await service.CrearContratoPersonal(registro);
                return Ok(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Modifica un contrato existente.
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ContratoCabecera), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<ContratoCabecera>> Modificar(long id, [FromBody] ContratoPersonalDto registro)
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
        /// Elimina un contrato de un personal
        /// </summary>
        [HttpDelete("{id}")]
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
    }
}
