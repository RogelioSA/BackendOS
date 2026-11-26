using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nest.core.aplicacion.mantto.OrdenServicioTipos.Commands;
using nest.core.aplicacion.mantto.OrdenServicioTipos.Queries;
using nest.core.dominio;
using nest.core.dominio.Mantto.OrdenServicioTipoEntities;
using nest.core.security.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace nest.core.security.Controllers.Mantto
{
    /// <summary>
    /// Controlador para la gestión de tipos de orden de servicio.
    /// Permite realizar operaciones CRUD y obtener registros activos.
    /// Requiere autorización para acceder.
    /// </summary>
    [Authorize]
    [ApiExplorerSettings(GroupName = ConfigureEndpoints.EndpointsEnum.MANTTO)]
    [Route($"{ConfigureEndpoints.EndpointsEnum.MANTTO}/[controller]")]
    [ApiController]
    public class OrdenServicioTipoController : ControllerBase
    {
        private readonly ISender sender;

        public OrdenServicioTipoController(ISender sender)
        {
            this.sender = sender;
        }

        /// <summary>
        /// Obtiene todos los tipos de orden de servicio registrados.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<OrdenServicioTipo>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<OrdenServicioTipo>>> ObtenerTodos()
            => Ok(await sender.Send(new ObtenerTodosQuery()));

        /// <summary>
        /// Obtiene un tipo de orden de servicio por su ID.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(OrdenServicioTipo), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<OrdenServicioTipo>> ObtenerPorId(short id)
            => Ok(await sender.Send(new ObtenerPorIdQuery(id)));

        /// <summary>
        /// Obtiene todos los tipos de orden de servicio activos.
        /// </summary>
        [HttpGet("activos")]
        [ProducesResponseType(typeof(List<OrdenServicioTipo>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<OrdenServicioTipo>>> ObtenerActivos()
            => Ok(await sender.Send(new ObtenerActivosQuery()));

        /// <summary>
        /// Agrega un nuevo tipo de orden de servicio.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(OrdenServicioTipo), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<OrdenServicioTipo>> Agregar([FromBody] OrdenServicioTipoCrearCommand command)
            => Ok(await sender.Send(command));

        /// <summary>
        /// Modifica un tipo de orden de servicio existente.
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(OrdenServicioTipo), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<OrdenServicioTipo>> Modificar([FromRoute] short id, [FromBody] OrdenServicioTipoModificarCommand command)
        {
            var cmd = command with { Id = id };
            return Ok(await sender.Send(cmd));
        }

        /// <summary>
        /// Elimina un tipo de orden de servicio.
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult> Eliminar(short id)
        {
            await sender.Send(new OrdenServicioTipoEliminarCommand(id));
            return Ok(true);
        }
    }
}
