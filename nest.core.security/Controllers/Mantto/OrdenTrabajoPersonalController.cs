using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nest.core.aplicacion.mantto.OrdenTrabajoPersonales.Commands;
using nest.core.aplicacion.mantto.OrdenTrabajoPersonales.Queries;
using nest.core.dominio;
using nest.core.dominio.Mantto.OrdenTrabajoPersonalEntities;
using nest.core.security.Extensions;

namespace nest.core.security.Controllers.Mantto
{
    /// <summary>
    /// Controlador para gestionar el personal asociado a las órdenes de trabajo.
    /// </summary>
    [Authorize]
    [ApiExplorerSettings(GroupName = ConfigureEndpoints.EndpointsEnum.MANTTO)]
    [Route($"{ConfigureEndpoints.EndpointsEnum.MANTTO}/[controller]")]
    [ApiController]
    public class OrdenTrabajoPersonalController : ControllerBase
    {
        private readonly ISender sender;
        private readonly ILogger<OrdenTrabajoPersonalController> logger;

        /// <summary>
        /// Inicializa el controlador de personal de orden de trabajo.
        /// </summary>
        /// <param name="service">Servicio del dominio de personal.</param>
        /// <param name="logger">Logger para registrar auditoría.</param>
        public OrdenTrabajoPersonalController(ISender sender, ILogger<OrdenTrabajoPersonalController> logger)
        {
            this.sender = sender;
            this.logger = logger;
        }

        /// <summary>
        /// Obtiene el listado completo de personal asignado a órdenes de trabajo.
        /// </summary>
        /// <returns>Lista de registros de personal.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<OrdenTrabajoPersonal>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<OrdenTrabajoPersonal>>> ObtenerTodos()
        {
            try
            {
                var data = await sender.Send(new ObtenerTodosQuery());
                return Ok(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al obtener el personal asignado");
                throw;
            }
        }

        /// <summary>
        /// Obtiene un registro de personal por su identificador.
        /// </summary>
        /// <param name="id">Identificador del registro.</param>
        /// <returns>Información del personal asignado.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(OrdenTrabajoPersonal), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<OrdenTrabajoPersonal>> ObtenerPorId(long id)
        {
            try
            {
                var data = await sender.Send(new ObtenerPorIdQuery(id));
                return Ok(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al obtener el personal {PersonalId}", id);
                throw;
            }
        }

        /// <summary>
        /// Obtiene el personal asignado a una cabecera de orden de trabajo.
        /// </summary>
        /// <param name="ordenTrabajoCabeceraId">Identificador de la cabecera.</param>
        /// <returns>Lista de personal asociado.</returns>
        [HttpGet("cabecera/{ordenTrabajoCabeceraId}")]
        [ProducesResponseType(typeof(List<OrdenTrabajoPersonal>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<OrdenTrabajoPersonal>>> ObtenerPorCabecera(long ordenTrabajoCabeceraId)
        {
            try
            {
                var data = await sender.Send(new ObtenerPorCabeceraQuery(ordenTrabajoCabeceraId));
                return Ok(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al obtener el personal de la cabecera {CabeceraId}", ordenTrabajoCabeceraId);
                throw;
            }
        }

        /// <summary>
        /// Registra un nuevo personal en una orden de trabajo.
        /// </summary>
        /// <param name="registro">Datos del personal a registrar.</param>
        /// <returns>Registro creado.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(OrdenTrabajoPersonal), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<OrdenTrabajoPersonal>> Agregar([FromBody] OrdenTrabajoPersonalCrearCommand command)
        {
            try
            {
                var data = await sender.Send(command);
                return Ok(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al registrar personal en la orden de trabajo");
                throw;
            }
        }

        /// <summary>
        /// Actualiza los datos del personal asignado a una orden de trabajo.
        /// </summary>
        /// <param name="id">Identificador del registro.</param>
        /// <param name="registro">Datos actualizados.</param>
        /// <returns>Registro actualizado.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(OrdenTrabajoPersonal), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<OrdenTrabajoPersonal>> Modificar(long id, [FromBody] OrdenTrabajoPersonalModificarCommand command)
        {
            try
            {
                var updatedCommand = command with { Id = id };
                var data = await sender.Send(updatedCommand);
                return Ok(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al modificar el personal {PersonalId}", id);
                throw;
            }
        }

        /// <summary>
        /// Elimina un registro de personal asociado a una orden de trabajo.
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
                await sender.Send(new OrdenTrabajoPersonalEliminarCommand(id));
                return Ok(true);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al eliminar el personal {PersonalId}", id);
                throw;
            }
        }
    }
}
