using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nest.core.aplicacion.general.AdjuntoConfigProviders.Commands;
using nest.core.aplicacion.general.AdjuntoConfigProviders.Queries;
using nest.core.dominio;
using nest.core.dominio.General.AdjuntoProviderEntities;
using nest.core.security.Extensions;

namespace nest.core.security.Controllers.General
{
    /// <summary>
    /// Gestiona la configuración de proveedores de adjuntos.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiExplorerSettings(GroupName = ConfigureEndpoints.EndpointsEnum.GENERAL)]
    [Route($"{ConfigureEndpoints.EndpointsEnum.GENERAL}/[controller]")]
    public class AdjuntoConfigProviderController : ControllerBase
    {
        private readonly ISender sender;

        public AdjuntoConfigProviderController(ISender sender)
        {
            this.sender = sender;
        }

        /// <summary>
        /// Obtiene todas las configuraciones registradas.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<AdjuntoConfigProvider>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<AdjuntoConfigProvider>>> ObtenerTodos(CancellationToken ct)
        {
            var data = await sender.Send(new ObtenerTodosQuery(), ct);
            return Ok(data);
        }

        /// <summary>
        /// Obtiene una configuración por su identificador.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(AdjuntoConfigProvider), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<AdjuntoConfigProvider>> ObtenerPorId(AdjuntoConfigProviderModuloEnum id, CancellationToken ct)
        {
            var data = await sender.Send(new ObtenerPorIdQuery(id), ct);
            return Ok(data);
        }

        /// <summary>
        /// Obtiene las configuraciones activas.
        /// </summary>
        [HttpGet("activos")]
        [ProducesResponseType(typeof(List<AdjuntoConfigProvider>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<AdjuntoConfigProvider>>> ObtenerActivos(CancellationToken ct)
        {
            var data = await sender.Send(new ObtenerActivosQuery(), ct);
            return Ok(data);
        }

        /// <summary>
        /// Registra una nueva configuración.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(AdjuntoConfigProvider), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<AdjuntoConfigProvider>> Agregar([FromBody] AdjuntoConfigProviderCrearCommand command, CancellationToken ct)
        {
            var data = await sender.Send(command, ct);
            return Ok(data);
        }

        /// <summary>
        /// Actualiza una configuración existente.
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(AdjuntoConfigProvider), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<AdjuntoConfigProvider>> Modificar(AdjuntoConfigProviderModuloEnum id, [FromBody] AdjuntoConfigProviderModificarCommand command, CancellationToken ct)
        {
            var cmd = command with { Id = id };
            var data = await sender.Send(cmd, ct);
            return Ok(data);
        }

        /// <summary>
        /// Elimina una configuración.
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult> Eliminar(AdjuntoConfigProviderModuloEnum id, CancellationToken ct)
        {
            await sender.Send(new AdjuntoConfigProviderEliminarCommand(id), ct);
            return Ok();
        }
    }
}
