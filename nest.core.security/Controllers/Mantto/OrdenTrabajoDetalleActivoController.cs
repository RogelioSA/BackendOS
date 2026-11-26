using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nest.core.aplicacion.mantto.OrdenTrabajoDetalleActivos.Commands;
using nest.core.aplicacion.mantto.OrdenTrabajoDetalleActivos.Queries;
using nest.core.dominio;
using nest.core.dominio.Mantto.OrdenTrabajoDetalleActivoEntities;
using nest.core.security.Extensions;

namespace nest.core.security.Controllers.Mantto
{
    /// <summary>
    /// Controlador para administrar los activos asociados a los detalles de orden de trabajo.
    /// </summary>
    [Authorize]
    [ApiExplorerSettings(GroupName = ConfigureEndpoints.EndpointsEnum.MANTTO)]
    [Route($"{ConfigureEndpoints.EndpointsEnum.MANTTO}/[controller]")]
    [ApiController]
    public class OrdenTrabajoDetalleActivoController : ControllerBase
    {
        private readonly ISender sender;
        private readonly ILogger<OrdenTrabajoDetalleActivoController> logger;

        /// <summary>
        /// Inicializa el controlador de activos por detalle de orden de trabajo.
        /// </summary>
        /// <param name="service">Servicio de dominio para los activos.</param>
        /// <param name="logger">Logger para auditoría.</param>
        public OrdenTrabajoDetalleActivoController(ISender sender, ILogger<OrdenTrabajoDetalleActivoController> logger)
        {
            this.sender = sender;
            this.logger = logger;
        }

        /// <summary>
        /// Obtiene un activo asociado a un detalle específico.
        /// </summary>
        /// <param name="id">Identificador del detalle/activo.</param>
        /// <returns>Detalle activo encontrado.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(OrdenTrabajoDetalleActivo), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<OrdenTrabajoDetalleActivo>> ObtenerPorId(long id)
        {
            try
            {
                var data = await sender.Send(new ObtenerPorIdQuery(id));
                return Ok(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al obtener el activo del detalle {DetalleId}", id);
                throw;
            }
        }

        /// <summary>
        /// Obtiene los activos registrados para un detalle de orden de trabajo.
        /// </summary>
        /// <param name="ordenTrabajoDetalleId">Identificador del detalle.</param>
        /// <returns>Lista de activos asociados.</returns>
        [HttpGet("detalle/{ordenTrabajoDetalleId}")]
        [ProducesResponseType(typeof(List<OrdenTrabajoDetalleActivo>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<OrdenTrabajoDetalleActivo>>> ObtenerPorDetalle(long ordenTrabajoDetalleId)
        {
            try
            {
                var data = await sender.Send(new ObtenerPorDetalleQuery(ordenTrabajoDetalleId));
                return Ok(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al obtener los activos del detalle {DetalleId}", ordenTrabajoDetalleId);
                throw;
            }
        }

        /// <summary>
        /// Crea un nuevo registro de activo asociado a un detalle.
        /// </summary>
        /// <param name="registro">Datos del activo a registrar.</param>
        /// <returns>Registro creado.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(OrdenTrabajoDetalleActivo), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<OrdenTrabajoDetalleActivo>> Agregar([FromBody] OrdenTrabajoDetalleActivoCrearCommand command)
        {
            try
            {
                var data = await sender.Send(command);
                return Ok(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al asociar un activo al detalle de orden de trabajo");
                throw;
            }
        }

        /// <summary>
        /// Actualiza la información de un activo asociado a un detalle.
        /// </summary>
        /// <param name="id">Identificador del registro.</param>
        /// <param name="registro">Datos actualizados.</param>
        /// <returns>Registro actualizado.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(OrdenTrabajoDetalleActivo), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<OrdenTrabajoDetalleActivo>> Modificar(long id, [FromBody] OrdenTrabajoDetalleActivoModificarCommand command)
        {
            try
            {
                var updatedCommand = command with { Id = id };
                var data = await sender.Send(updatedCommand);
                return Ok(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al modificar el activo del detalle {DetalleId}", id);
                throw;
            }
        }

        /// <summary>
        /// Elimina el registro de un activo asociado a un detalle.
        /// </summary>
        /// <param name="id">Identificador del registro.</param>
        /// <returns>Resultado de la operación.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult> Eliminar(long id)
        {
            try
            {
                await sender.Send(new OrdenTrabajoDetalleActivoEliminarCommand(id));
                return Ok(true);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al eliminar el activo del detalle {DetalleId}", id);
                throw;
            }
        }
    }
}
