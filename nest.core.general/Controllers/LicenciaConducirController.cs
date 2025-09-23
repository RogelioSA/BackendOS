using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nest.core.aplicacion.general.LicenciaConducirServices;
using nest.core.dominio;
using nest.core.dominio.General.LicenciaConducirEntities;

namespace nest.core.general.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class LicenciaConducirController : ControllerBase
    {
        private readonly LicenciaConducirService service;
        private readonly ILogger<LicenciaConducirController> logger;
        public LicenciaConducirController(LicenciaConducirService service, ILogger<LicenciaConducirController> logger)
        {
            this.service = service;
            this.logger = logger;
        }
        [HttpGet]
        [ProducesResponseType(typeof(List<LicenciaConducir>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<LicenciaConducir>>> ObtenerTodos()
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
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(LicenciaConducir), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<LicenciaConducir>> ObtenerPorId(byte id)
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
        [HttpPost]
        [ProducesResponseType(typeof(LicenciaConducir), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<LicenciaConducir>> Agregar([FromBody] LicenciaConducirCrearDto registro)
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
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(LicenciaConducir), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<LicenciaConducir>> Modificar(byte id, [FromBody] LicenciaConducirCrearDto registro)
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
