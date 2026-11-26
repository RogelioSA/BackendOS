using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nest.core.aplicacion.mantto.OrdenServicioCabeceras.Commands;
using nest.core.aplicacion.mantto.OrdenServicioCabeceras.Queries;
using nest.core.dominio;
using nest.core.dominio.Mantto.OrdenServicioCabeceraEntities;
using nest.core.security.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace nest.core.security.Controllers.Mantto
{
    /// <summary>
    /// Controlador para la administración de órdenes de servicio cabecera.
    /// Proporciona operaciones CRUD básicas para gestionar la información principal de las órdenes de servicio.
    /// </summary>
    [Authorize]
    [ApiExplorerSettings(GroupName = ConfigureEndpoints.EndpointsEnum.MANTTO)]
    [Route($"{ConfigureEndpoints.EndpointsEnum.MANTTO}/[controller]")]
    [ApiController]
    public class OrdenServicioCabeceraController : ControllerBase
    {
        private readonly ISender sender;

        public OrdenServicioCabeceraController(ISender sender)
        {
            this.sender = sender;
        }

        /// <summary>
        /// Obtiene la lista completa de órdenes de servicio cabecera.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<OrdenServicioCabecera>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<OrdenServicioCabecera>>> ObtenerTodos()
            => Ok(await sender.Send(new ObtenerTodosQuery()));

        /// <summary>
        /// Obtiene una orden de servicio cabecera por su identificador.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(OrdenServicioCabecera), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<OrdenServicioCabecera>> ObtenerPorId(long id)
            => Ok(await sender.Send(new ObtenerPorIdQuery(id)));

        /// <summary>
        /// Crea una nueva orden de servicio cabecera.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(OrdenServicioCabecera), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<OrdenServicioCabecera>> Agregar([FromBody] OrdenServicioCabeceraCrearCommand command)
            => Ok(await sender.Send(command));

        /// <summary>
        /// Actualiza la información de una orden de servicio cabecera existente.
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(OrdenServicioCabecera), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<OrdenServicioCabecera>> Modificar([FromRoute] long id, [FromBody] OrdenServicioCabeceraModificarCommand command)
        {
            var cmd = command with { Id = id };
            return Ok(await sender.Send(cmd));
        }

        /// <summary>
        /// Elimina una orden de servicio cabecera existente.
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult> Eliminar(long id)
        {
            await sender.Send(new OrdenServicioCabeceraEliminarCommand(id));
            return Ok(true);
        }
    }
}
