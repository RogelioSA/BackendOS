using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nest.core.aplicacion.general.DocumentoIdentidadTipos.Commands;
using nest.core.aplicacion.general.DocumentoIdentidadTipos.Queries;
using nest.core.dominio;
using nest.core.dominio.General.DocumentoIdentidadTipoEntities;
using nest.core.security.Extensions;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace nest.core.security.Controllers.General
{
    [Authorize]
    [ApiController]
    [ApiExplorerSettings(GroupName = ConfigureEndpoints.EndpointsEnum.GENERAL)]
    [Route($"{ConfigureEndpoints.EndpointsEnum.GENERAL}/[controller]")]
    public class DocumentoIdentidadTipoController : ControllerBase
    {
        private readonly ISender sender;

        public DocumentoIdentidadTipoController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<DocumentoIdentidadTipo>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<DocumentoIdentidadTipo>>> ObtenerTodos(CancellationToken ct)
        {
            var entidad = await sender.Send(new ObtenerTodosQuery(), ct);
            return Ok(entidad);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DocumentoIdentidadTipo), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<DocumentoIdentidadTipo>> ObtenerPorId([FromRoute] byte id, CancellationToken ct)
        {
            var entidad = await sender.Send(new ObtenerPorIdQuery(id), ct);
            return Ok(entidad);
        }

        [HttpGet("activos")]
        [ProducesResponseType(typeof(List<DocumentoIdentidadTipo>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<DocumentoIdentidadTipo>>> ObtenerActivos(CancellationToken ct)
        {
            var entidad = await sender.Send(new ObtenerActivosQuery(), ct);
            return Ok(entidad);
        }

        [HttpPost]
        [ProducesResponseType(typeof(DocumentoIdentidadTipo), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<DocumentoIdentidadTipo>> Agregar([FromBody] DocumentoIdentidadTipoCrearCommand command, CancellationToken ct)
        {
            var entidad = await sender.Send(command, ct);
            return Ok(entidad);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(DocumentoIdentidadTipo), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<DocumentoIdentidadTipo>> Modificar([FromRoute] byte id, [FromBody] DocumentoIdentidadTipoModificarCommand command, CancellationToken ct)
        {
            var cmd = command with { Id = id };
            var entidad = await sender.Send(cmd, ct);
            return Ok(entidad);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult> Eliminar([FromRoute] byte id, CancellationToken ct)
        {
            await sender.Send(new DocumentoIdentidadTipoEliminarCommand(id), ct);
            return Ok();
        }
    }
}
