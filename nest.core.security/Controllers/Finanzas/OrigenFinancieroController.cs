using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nest.core.aplicacion.finanzas.OrigenFinancieros.Commands;
using nest.core.aplicacion.finanzas.OrigenFinancieros.Queries;
using nest.core.dominio;
using nest.core.dominio.Finanzas.OrigenFinancieroEntities;
using nest.core.security.Extensions;

namespace nest.core.security.Controllers.Finanzas
{
    /// <summary>
    /// Controlador para la gestión de orígenes financieros.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiExplorerSettings(GroupName = ConfigureEndpoints.EndpointsEnum.FINANZAS)]
    [Route($"{ConfigureEndpoints.EndpointsEnum.FINANZAS}/[controller]")]
    public class OrigenFinancieroController : ControllerBase
    {
        private readonly ISender sender;
        private readonly ILogger<OrigenFinancieroController> logger;

        public OrigenFinancieroController(ISender sender, ILogger<OrigenFinancieroController> logger)
        {
            this.sender = sender;
            this.logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<OrigenFinanciero>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<OrigenFinanciero>>> ObtenerTodos()
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
        [ProducesResponseType(typeof(List<OrigenFinanciero>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<OrigenFinanciero>>> ObtenerActivos()
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
        [ProducesResponseType(typeof(OrigenFinanciero), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<OrigenFinanciero>> ObtenerPorId(short id)
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
        [ProducesResponseType(typeof(OrigenFinanciero), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<OrigenFinanciero>> Agregar([FromBody] OrigenFinancieroCrearCommand command)
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
        [ProducesResponseType(typeof(OrigenFinanciero), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<OrigenFinanciero>> Modificar(short id, [FromBody] OrigenFinancieroModificarCommand command)
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
        public async Task<ActionResult> Eliminar(short id)
        {
            try
            {
                await sender.Send(new OrigenFinancieroEliminarCommand(id));
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
