using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nest.core.aplicacion.finanzas.FinancieroServices;
using nest.core.dominio;
using nest.core.dominio.Finanzas.FinancieroCabeceraEntities;
using nest.core.dominio.Finanzas.FinancieroDetalleEntities;

namespace nest.core.finanzas.Controllers
{
    /// <summary>
    /// Controlador para la gestión financiera de cabeceras y detalles.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class FinancieroController : ControllerBase
    {
        private readonly FinancieroService service;
        private readonly ILogger<FinancieroController> logger;

        public FinancieroController(FinancieroService service, ILogger<FinancieroController> logger)
        {
            this.service = service;
            this.logger = logger;
        }

        /// <summary>
        /// Obtiene todas las cabeceras financieras.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<FinancieroCabecera>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<FinancieroCabecera>>> ObtenerTodos()
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
        /// Obtiene una cabecera financiera por su Id.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(FinancieroCabecera), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<FinancieroCabecera>> ObtenerPorId(long id)
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
        /// Crea una nueva cabecera financiera con sus detalles.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(FinancieroCabecera), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<FinancieroCabecera>> Agregar([FromBody] FinancieroDto registro)
        {
            try
            {
                var data = await service.Agregar(registro, true);
                return Ok(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Crea un nuevo detalle financiero
        /// <param name="idCabecera">ID de la cabecera.</param>
        /// <param name="registro">Cuerpo del detalle financiero.</param>
        /// <returns>FinancieroDetalle</returns>
        /// </summary>
        [HttpPost("detalle/{idCabecera}")]
        [ProducesResponseType(typeof(FinancieroCabecera), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<FinancieroDetalle>> AgregarDetalle(long idCabecera, [FromBody] FinancieroDetalleCrearDto registro)
        {
            try
            {
                var data = await service.AgregarDetalle(idCabecera, registro);
                return Ok(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Modifica una cabecera financiera existente.
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(FinancieroCabecera), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<FinancieroCabecera>> Modificar(long id, [FromBody] FinancieroDto registro)
        {
            try
            {
                var data = await service.Modificar(id, registro, true);
                return Ok(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Modifica un detalle financiero existente.
        /// <param name="id">ID del detalle financiero</param>
        /// <param name="registro">Registro completo del detalle</param>
        /// </summary>
        [HttpPut("detalle/{id}")]
        [ProducesResponseType(typeof(FinancieroCabecera), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<FinancieroCabecera>> ModificarDetalle(long id, [FromBody] FinancieroDetalleCrearDto registro)
        {
            try
            {
                var data = await service.ModificarDetalle(id, registro);
                return Ok(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Elimina una cabecera financiera.
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

        /// <summary>
        /// Elimina una detalle financiero.
        /// <param name="id">ID del detalle financiero</param>
        /// </summary>
        [HttpDelete("detalle/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult> EliminarDetalle(long id)
        {
            try
            {
                await service.EliminarDetalle(id);
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
