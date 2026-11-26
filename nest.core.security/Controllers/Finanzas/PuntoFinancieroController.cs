using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nest.core.aplicacion.finanzas.PuntoFinancieros.Commands;
using nest.core.aplicacion.finanzas.PuntoFinancieros.Queries;
using nest.core.dominio;
using nest.core.dominio.Finanzas.PuntoFinancieroEntities;
using nest.core.security.Extensions;

namespace nest.core.security.Controllers.Finanzas
{
    /// <summary>
    /// Controlador para la gestión de puntos financieros.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiExplorerSettings(GroupName = ConfigureEndpoints.EndpointsEnum.FINANZAS)]
    [Route($"{ConfigureEndpoints.EndpointsEnum.FINANZAS}/[controller]")]
    public class PuntoFinancieroController : ControllerBase
    {
        private readonly ISender sender;
        private readonly ILogger<PuntoFinancieroController> logger;

        public PuntoFinancieroController(ISender sender, ILogger<PuntoFinancieroController> logger)
        {
            this.sender = sender;
            this.logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<PuntoFinanciero>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<PuntoFinanciero>>> ObtenerTodos()
        {
            try
            {
                var data = await sender.Send(new ObtenerTodosQuery());
                return Ok(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }

        [HttpGet("activos")]
        [ProducesResponseType(typeof(List<PuntoFinanciero>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<PuntoFinanciero>>> ObtenerActivos()
        {
            try
            {
                var data = await sender.Send(new ObtenerActivosQuery());
                return Ok(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PuntoFinanciero), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<PuntoFinanciero>> ObtenerPorId(int id)
        {
            try
            {
                var data = await sender.Send(new ObtenerPorIdQuery(id));
                return Ok(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(PuntoFinanciero), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<PuntoFinanciero>> Agregar([FromBody] PuntoFinancieroCrearCommand command)
        {
            try
            {
                var data = await sender.Send(command);
                return Ok(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(PuntoFinanciero), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<PuntoFinanciero>> Modificar(int id, [FromBody] PuntoFinancieroModificarCommand command)
        {
            try
            {
                var data = await sender.Send(command with { Id = id });
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
        public async Task<ActionResult> Eliminar(int id)
        {
            try
            {
                await sender.Send(new PuntoFinancieroEliminarCommand(id));
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
