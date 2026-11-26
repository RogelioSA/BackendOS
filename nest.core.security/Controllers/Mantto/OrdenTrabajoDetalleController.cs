using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nest.core.aplicacion.mantto.OrdenTrabajoDetalles.Commands;
using nest.core.aplicacion.mantto.OrdenTrabajoDetalles.Queries;
using nest.core.dominio;
using nest.core.dominio.Mantto.OrdenTrabajoDetalleEntities;
using nest.core.security.Extensions;

namespace nest.core.security.Controllers.Mantto
{
    /// <summary>
    /// Controlador para la gestión de los detalles de orden de trabajo.
    /// Permite CRUD y consultas por cabecera.
    /// </summary>
    [Authorize]
    [ApiExplorerSettings(GroupName = ConfigureEndpoints.EndpointsEnum.MANTTO)]
    [Route($"{ConfigureEndpoints.EndpointsEnum.MANTTO}/[controller]")]
    [ApiController]
    public class OrdenTrabajoDetalleController : ControllerBase
    {
        private readonly ISender sender;
        private readonly ILogger<OrdenTrabajoDetalleController> logger;

        /// <summary>
        /// Inicializa una nueva instancia del controlador.
        /// </summary>
        /// <param name="service">Servicio de detalles de orden de trabajo.</param>
        /// <param name="logger">Logger para registrar eventos.</param>
        public OrdenTrabajoDetalleController(ISender sender, ILogger<OrdenTrabajoDetalleController> logger)
        {
            this.sender = sender;
            this.logger = logger;
        }

        /// <summary>
        /// Obtiene un detalle de orden de trabajo por su identificador.
        /// </summary>
        /// <param name="id">Identificador del detalle.</param>
        /// <returns>Detalle solicitado.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(OrdenTrabajoDetalle), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<OrdenTrabajoDetalle>> ObtenerPorId(long id)
        {
            try
            {
                var data = await sender.Send(new ObtenerPorIdQuery(id));
                return Ok(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al obtener el detalle {DetalleId}", id);
                throw;
            }
        }

        /// <summary>
        /// Obtiene los detalles de una cabecera de orden de trabajo específica.
        /// </summary>
        /// <param name="ordenTrabajoCabeceraId">Identificador de la cabecera.</param>
        /// <returns>Lista de detalles asociados.</returns>
        [HttpGet("cabecera/{ordenTrabajoCabeceraId}")]
        [ProducesResponseType(typeof(List<OrdenTrabajoDetalle>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<OrdenTrabajoDetalle>>> ObtenerPorCabecera(long ordenTrabajoCabeceraId)
        {
            try
            {
                var data = await sender.Send(new ObtenerPorCabeceraQuery(ordenTrabajoCabeceraId));
                return Ok(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al obtener los detalles de la cabecera {CabeceraId}", ordenTrabajoCabeceraId);
                throw;
            }
        }

        /// <summary>
        /// Crea un nuevo detalle de orden de trabajo.
        /// </summary>
        /// <param name="registro">Datos del detalle a registrar.</param>
        /// <returns>Detalle creado.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(OrdenTrabajoDetalle), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<OrdenTrabajoDetalle>> Agregar([FromBody] OrdenTrabajoDetalleCrearCommand command)
        {
            try
            {
                var data = await sender.Send(command);
                return Ok(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al crear el detalle de orden de trabajo");
                throw;
            }
        }

        /// <summary>
        /// Modifica un detalle de orden de trabajo existente.
        /// </summary>
        /// <param name="id">Identificador del detalle.</param>
        /// <param name="registro">Datos actualizados.</param>
        /// <returns>Detalle modificado.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(OrdenTrabajoDetalle), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<OrdenTrabajoDetalle>> Modificar(long id, [FromBody] OrdenTrabajoDetalleModificarCommand command)
        {
            try
            {
                var updatedCommand = command with { Id = id };
                var data = await sender.Send(updatedCommand);
                return Ok(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al modificar el detalle {DetalleId}", id);
                throw;
            }
        }

        /// <summary>
        /// Elimina un detalle de orden de trabajo.
        /// </summary>
        /// <param name="id">Identificador del detalle.</param>
        /// <returns>Resultado de la operación.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult> Eliminar(long id)
        {
            try
            {
                await sender.Send(new OrdenTrabajoDetalleEliminarCommand(id));
                return Ok(true);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al eliminar el detalle {DetalleId}", id);
                throw;
            }
        }
    }
}
