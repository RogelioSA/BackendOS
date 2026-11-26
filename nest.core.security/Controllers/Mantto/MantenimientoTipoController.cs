using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nest.core.aplicacion.mantto.MantenimientoTipos.Commands;
using nest.core.aplicacion.mantto.MantenimientoTipos.Queries;
using nest.core.dominio;
using nest.core.dominio.Mantto.MantenimientoTipoEntities;
using nest.core.security.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace nest.core.security.Controllers.Mantto
{
    /// <summary>
    /// Controlador para la gestión de tipos de mantenimiento.
    /// Permite realizar operaciones CRUD y obtener registros activos.
    /// Requiere autorización para acceder.
    /// </summary>
    [Authorize]
    [ApiExplorerSettings(GroupName = ConfigureEndpoints.EndpointsEnum.MANTTO)]
    [Route($"{ConfigureEndpoints.EndpointsEnum.MANTTO}/[controller]")]
    [ApiController]
    public class MantenimientoTipoController : ControllerBase
    {
        private readonly ISender sender;

        public MantenimientoTipoController(ISender sender)
        {
            this.sender = sender;
        }

        /// <summary>
        /// Obtiene todos los tipos de mantenimiento registrados.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<MantenimientoTipo>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<MantenimientoTipo>>> ObtenerTodos()
            => Ok(await sender.Send(new ObtenerTodosQuery()));

        /// <summary>
        /// Obtiene un tipo de mantenimiento por su ID.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MantenimientoTipo), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<MantenimientoTipo>> ObtenerPorId(short id)
            => Ok(await sender.Send(new ObtenerPorIdQuery(id)));

        /// <summary>
        /// Obtiene todos los tipos de mantenimiento activos.
        /// </summary>
        [HttpGet("activos")]
        [ProducesResponseType(typeof(List<MantenimientoTipo>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<MantenimientoTipo>>> ObtenerActivos()
            => Ok(await sender.Send(new ObtenerActivosQuery()));

        /// <summary>
        /// Agrega un nuevo tipo de mantenimiento.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(MantenimientoTipo), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<MantenimientoTipo>> Agregar([FromBody] MantenimientoTipoCrearCommand command)
            => Ok(await sender.Send(command));

        /// <summary>
        /// Modifica un tipo de mantenimiento existente.
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(MantenimientoTipo), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<MantenimientoTipo>> Modificar([FromRoute] short id, [FromBody] MantenimientoTipoModificarCommand command)
        {
            var cmd = command with { Id = id };
            return Ok(await sender.Send(cmd));
        }

        /// <summary>
        /// Elimina un tipo de mantenimiento.
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult> Eliminar(short id)
        {
            await sender.Send(new MantenimientoTipoEliminarCommand(id));
            return Ok(true);
        }
    }
}
