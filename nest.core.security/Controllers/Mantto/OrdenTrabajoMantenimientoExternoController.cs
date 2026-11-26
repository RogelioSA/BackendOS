using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nest.core.aplicacion.mantto.OrdenTrabajo.Commands;
using nest.core.aplicacion.mantto.OrdenTrabajo.Queries;
using nest.core.dominio;
using nest.core.dominio.Mantto.OrdenTrabajoCabeceraEntities;
using nest.core.security.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace nest.core.security.Controllers.Mantto
{
    /// <summary>
    /// Controlador para gestionar órdenes de trabajo de mantenimiento externo
    /// junto con sus detalles y activos asociados.
    /// </summary>
    [Authorize]
    [ApiExplorerSettings(GroupName = ConfigureEndpoints.EndpointsEnum.MANTTO)]
    [Route($"{ConfigureEndpoints.EndpointsEnum.MANTTO}/OrdenTrabajo/MantenimientoExterno")]
    [ApiController]
    public class OrdenTrabajoMantenimientoExternoController : ControllerBase
    {
        private readonly ISender sender;
        private readonly ILogger<OrdenTrabajoMantenimientoExternoController> logger;

        /// <summary>
        /// Inicializa una nueva instancia del controlador.
        /// </summary>
        /// <param name="sender">Mediador usado para enviar comandos y consultas.</param>
        /// <param name="logger">Logger para auditoría y trazabilidad.</param>
        public OrdenTrabajoMantenimientoExternoController(ISender sender, ILogger<OrdenTrabajoMantenimientoExternoController> logger)
        {
            this.sender = sender;
            this.logger = logger;
        }

        /// <summary>
        /// Obtiene todas las órdenes de trabajo de mantenimiento externo.
        /// </summary>
        /// <returns>Listado de órdenes de trabajo.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<OrdenTrabajoCabecera>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<OrdenTrabajoCabecera>>> ObtenerTodos()
        {
            try
            {
                var data = await sender.Send(new ObtenerTodosQuery());
                return Ok(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al obtener las órdenes de trabajo de mantenimiento externo");
                throw;
            }
        }

        /// <summary>
        /// Obtiene una orden de trabajo de mantenimiento externo por su identificador.
        /// </summary>
        /// <param name="id">Identificador de la orden de trabajo.</param>
        /// <returns>Orden de trabajo encontrada.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(OrdenTrabajoCabecera), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<OrdenTrabajoCabecera>> ObtenerPorId(long id)
        {
            try
            {
                var data = await sender.Send(new ObtenerPorIdQuery(id));
                return Ok(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al obtener la orden de trabajo {OrdenTrabajoId}", id);
                throw;
            }
        }

        /// <summary>
        /// Obtiene las órdenes de trabajo de mantenimiento externo asociadas a una orden de servicio.
        /// </summary>
        /// <param name="ordenServicioCabeceraId">Identificador de la orden de servicio.</param>
        /// <returns>Listado de órdenes de trabajo asociadas.</returns>
        [HttpGet("orden-servicio/{ordenServicioCabeceraId}")]
        [ProducesResponseType(typeof(List<OrdenTrabajoCabecera>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<OrdenTrabajoCabecera>>> ObtenerPorOrdenServicio(long ordenServicioCabeceraId)
        {
            try
            {
                var data = await sender.Send(new ObtenerPorOrdenServicioQuery(ordenServicioCabeceraId));
                return Ok(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al obtener las órdenes de trabajo para la orden de servicio {OrdenServicioId}", ordenServicioCabeceraId);
                throw;
            }
        }

        /// <summary>
        /// Crea una nueva orden de trabajo de mantenimiento externo con sus detalles y activos.
        /// </summary>
        /// <param name="command">Comando con la información de cabecera, detalles y personal.</param>
        /// <returns>Orden de trabajo creada.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(OrdenTrabajoCabecera), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<OrdenTrabajoCabecera>> Agregar([FromBody] OTMantenimientoExternoCrearCommand command)
        {
            try
            {
                var data = await sender.Send(command);
                return Ok(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al crear la orden de trabajo de mantenimiento externo");
                throw;
            }
        }

        /// <summary>
        /// Modifica una orden de trabajo de mantenimiento externo existente.
        /// </summary>
        /// <param name="id">Identificador de la orden de trabajo.</param>
        /// <param name="command">Comando con la información actualizada de cabecera, detalles y personal.</param>
        /// <returns>Orden de trabajo actualizada.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(OrdenTrabajoCabecera), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<OrdenTrabajoCabecera>> Modificar(long id, [FromBody] OTMantenimientoExternoModificarCommand command)
        {
            try
            {
                var cmd = command with { Id = id };
                var data = await sender.Send(cmd);
                return Ok(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al modificar la orden de trabajo {OrdenTrabajoId}", id);
                throw;
            }
        }

        /// <summary>
        /// Elimina una orden de trabajo de mantenimiento externo.
        /// </summary>
        /// <param name="id">Identificador de la orden de trabajo.</param>
        /// <returns>Resultado de la operación.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult> Eliminar(long id)
        {
            try
            {
                await sender.Send(new OTMantenimientoExternoEliminarCommand(id));
                return Ok(true);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al eliminar la orden de trabajo {OrdenTrabajoId}", id);
                throw;
            }
        }
    }
}
