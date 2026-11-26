using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nest.core.aplicacion.legal.ContratoTipos.Commands;
using nest.core.aplicacion.legal.ContratoTipos.Queries;
using nest.core.dominio;
using nest.core.dominio.Legal.ContratoTipoEntities;
using nest.core.security.Extensions;

namespace nest.core.security.Controllers.Legal
{
    /// <summary>
    /// Controlador para la gestión de tipos de contrato.
    /// </summary>
    [Authorize]
    [ApiExplorerSettings(GroupName = ConfigureEndpoints.EndpointsEnum.LEGAL)]
    [Route($"{ConfigureEndpoints.EndpointsEnum.LEGAL}/[controller]")]
    [ApiController]
    public class ContratoTipoController : ControllerBase
    {
        private readonly ISender sender;
        private readonly ILogger<ContratoTipoController> logger;

        public ContratoTipoController(ISender sender, ILogger<ContratoTipoController> logger)
        {
            this.sender = sender;
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
                var data = await sender.Send(new ObtenerTodosQuery());
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
                var data = await sender.Send(new ObtenerPorIdQuery(id));
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
        public async Task<ActionResult<ContratoTipo>> Agregar([FromBody] ContratoTipoCrearCommand command)
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

        /// <summary>
        /// Modifica un tipo de contrato existente.
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ContratoTipo), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<ContratoTipo>> Modificar(byte id, [FromBody] ContratoTipoModificarCommand command)
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
                await sender.Send(new ContratoTipoEliminarCommand(id));
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
