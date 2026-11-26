using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nest.core.aplicacion.mantto.OrdenTrabajoCabeceras.Commands;
using nest.core.aplicacion.mantto.OrdenTrabajoCabeceras.Queries;
using nest.core.dominio;
using nest.core.dominio.Mantto.OrdenTrabajoCabeceraEntities;
using nest.core.security.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace nest.core.security.Controllers.Mantto
{
    /// <summary>
    /// Controlador para gestionar las cabeceras de orden de trabajo.
    /// Permite realizar operaciones CRUD y consultar por orden de servicio.
    /// </summary>
    [Authorize]
    [ApiExplorerSettings(GroupName = ConfigureEndpoints.EndpointsEnum.MANTTO)]
    [Route($"{ConfigureEndpoints.EndpointsEnum.MANTTO}/[controller]")]
    [ApiController]
    public class OrdenTrabajoCabeceraController : ControllerBase
    {
        private readonly ISender sender;

        public OrdenTrabajoCabeceraController(ISender sender)
        {
            this.sender = sender;
        }

        /// <summary>
        /// Obtiene todas las cabeceras de orden de trabajo.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<OrdenTrabajoCabecera>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<OrdenTrabajoCabecera>>> ObtenerTodos()
            => Ok(await sender.Send(new ObtenerTodosQuery()));

        /// <summary>
        /// Obtiene todas las cabeceras de orden de trabajo de manera simplificada.
        /// </summary>
        [HttpGet("simplificado")]
        [ProducesResponseType(typeof(List<OrdenTrabajoCabeceraQueryView>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<OrdenTrabajoCabeceraQueryView>>> ObtenerTodosSimplificado()
            => Ok(await sender.Send(new ObtenerTodosSimplificadoQuery()));

        /// <summary>
        /// Obtiene una cabecera de orden de trabajo por su identificador.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(OrdenTrabajoCabecera), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<OrdenTrabajoCabecera>> ObtenerPorId(long id)
            => Ok(await sender.Send(new ObtenerPorIdQuery(id)));

        /// <summary>
        /// Obtiene cabeceras de orden de trabajo asociadas a una orden de servicio.
        /// </summary>
        [HttpGet("orden-servicio/{ordenServicioCabeceraId}")]
        [ProducesResponseType(typeof(List<OrdenTrabajoCabecera>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<OrdenTrabajoCabecera>>> ObtenerPorOrdenServicio(long ordenServicioCabeceraId)
            => Ok(await sender.Send(new ObtenerPorOrdenServicioQuery(ordenServicioCabeceraId)));

        /// <summary>
        /// Crea una nueva cabecera de orden de trabajo.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(OrdenTrabajoCabecera), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<OrdenTrabajoCabecera>> Agregar([FromBody] OrdenTrabajoCabeceraCrearCommand command)
            => Ok(await sender.Send(command));

        /// <summary>
        /// Actualiza una cabecera de orden de trabajo existente.
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(OrdenTrabajoCabecera), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<OrdenTrabajoCabecera>> Modificar([FromRoute] long id, [FromBody] OrdenTrabajoCabeceraModificarCommand command)
        {
            var cmd = command with { Id = id };
            return Ok(await sender.Send(cmd));
        }

        /// <summary>
        /// Elimina una cabecera de orden de trabajo.
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult> Eliminar(long id)
        {
            await sender.Send(new OrdenTrabajoCabeceraEliminarCommand(id));
            return Ok(true);
        }
    }
}
