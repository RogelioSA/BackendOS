using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nest.core.aplicacion.general.DocumentoIdentidadTipoServices;
using nest.core.dominio;
using nest.core.dominio.General.DocumentoIdentidadTipoEntities;

namespace nest.core.general.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class DocumentoIdentidadTipoController : ControllerBase
    {
        private readonly DocumentoIdentidadTipoService service;
        private readonly ILogger<DocumentoIdentidadTipoController> logger;
        public DocumentoIdentidadTipoController(DocumentoIdentidadTipoService service, ILogger<DocumentoIdentidadTipoController> logger)
        {
            this.service = service;
            this.logger = logger;
        }
        [HttpGet]
        [ProducesResponseType(typeof(List<DocumentoIdentidadTipo>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<DocumentoIdentidadTipo>>> ObtenerTodos()
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
        [ProducesResponseType(typeof(DocumentoIdentidadTipo), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<DocumentoIdentidadTipo>> ObtenerPorId(byte id)
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
        [ProducesResponseType(typeof(DocumentoIdentidadTipo), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<DocumentoIdentidadTipo>> Agregar([FromBody] DocumentoIdentidadTipoCrearDto registro)
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
        [ProducesResponseType(typeof(DocumentoIdentidadTipo), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<DocumentoIdentidadTipo>> Modificar(byte id, [FromBody] DocumentoIdentidadTipoCrearDto registro)
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
