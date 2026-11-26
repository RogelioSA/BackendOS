using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nest.core.aplicacion.mantto.OrdenServicioMantenimientoExternos.Commands;
using nest.core.aplicacion.mantto.OrdenServicioMantenimientoExternos.Queries;
using nest.core.dominio;
using nest.core.dominio.Mantto.OrdenServicioMantenimientoExternoEntities;
using nest.core.security.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace nest.core.security.Controllers.Mantto
{
    /// <summary>
    /// Controlador para la administración de órdenes de servicio de mantenimiento externo.
    /// Permite realizar operaciones CRUD básicas sobre las órdenes externas asociadas.
    /// </summary>
    [Authorize]
    [ApiExplorerSettings(GroupName = ConfigureEndpoints.EndpointsEnum.MANTTO)]
    [Route($"{ConfigureEndpoints.EndpointsEnum.MANTTO}/[controller]")]
    [ApiController]
    public class OrdenServicioMantenimientoExternoController : ControllerBase
    {
        private readonly ISender sender;

        public OrdenServicioMantenimientoExternoController(ISender sender)
        {
            this.sender = sender;
        }

        /// <summary>
        /// Obtiene todas las órdenes de servicio de mantenimiento externo registradas.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<OrdenServicioMantenimientoExterno>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<OrdenServicioMantenimientoExterno>>> ObtenerTodos()
            => Ok(await sender.Send(new ObtenerTodosQuery()));

        /// <summary>
        /// Obtiene una orden de servicio de mantenimiento externo por su identificador.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(OrdenServicioMantenimientoExterno), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<OrdenServicioMantenimientoExterno>> ObtenerPorId(long id)
            => Ok(await sender.Send(new ObtenerPorIdQuery(id)));

        /// <summary>
        /// Crea una nueva orden de servicio de mantenimiento externo.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(OrdenServicioMantenimientoExterno), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<OrdenServicioMantenimientoExterno>> Agregar([FromBody] OrdenServicioMantenimientoExternoCrearCommand command)
            => Ok(await sender.Send(command));

        /// <summary>
        /// Actualiza la información de una orden de servicio de mantenimiento externo existente.
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(OrdenServicioMantenimientoExterno), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<OrdenServicioMantenimientoExterno>> Modificar([FromRoute] long id, [FromBody] OrdenServicioMantenimientoExternoModificarCommand command)
        {
            var cmd = command with { Id = id };
            return Ok(await sender.Send(cmd));
        }

        /// <summary>
        /// Elimina una orden de servicio de mantenimiento externo.
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult> Eliminar(long id)
        {
            await sender.Send(new OrdenServicioMantenimientoExternoEliminarCommand(id));
            return Ok(true);
        }
    }
}
