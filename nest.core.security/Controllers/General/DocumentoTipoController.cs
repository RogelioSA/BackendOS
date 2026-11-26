using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nest.core.aplicacion.general.DocumentoTipos.Commands;
using nest.core.aplicacion.general.DocumentoTipos.Queries;
using nest.core.dominio;
using nest.core.dominio.General.DocumentoTipoEntities;
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
    public class DocumentoTipoController : ControllerBase
    {
        private readonly ISender sender;

        public DocumentoTipoController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<DocumentoTipo>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<DocumentoTipo>>> ObtenerTodos(CancellationToken ct)
        {
            var entidad = await sender.Send(new ObtenerTodosQuery(), ct);
            return Ok(entidad);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DocumentoTipo), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<DocumentoTipo>> ObtenerPorId([FromRoute] int id, CancellationToken ct)
        {
            var entidad = await sender.Send(new ObtenerPorIdQuery(id), ct);
            return Ok(entidad);
        }

        [HttpGet("activos")]
        [ProducesResponseType(typeof(List<DocumentoTipo>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<DocumentoTipo>>> ObtenerActivos(CancellationToken ct)
        {
            var entidad = await sender.Send(new ObtenerActivosQuery(), ct);
            return Ok(entidad);
        }

        [HttpPost]
        [ProducesResponseType(typeof(DocumentoTipo), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<DocumentoTipo>> Agregar([FromBody] DocumentoTipoCrearCommand command, CancellationToken ct)
        {
            var entidad = await sender.Send(command, ct);
            return Ok(entidad);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(DocumentoTipo), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<DocumentoTipo>> Modificar([FromRoute] int id, [FromBody] DocumentoTipoModificarCommand command, CancellationToken ct)
        {
            var cmd = command with { Id = id };
            var entidad = await sender.Send(cmd, ct);
            return Ok(entidad);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult> Eliminar([FromRoute] int id, CancellationToken ct)
        {
            await sender.Send(new DocumentoTipoEliminarCommand(id), ct);
            return Ok();
        }
    }
}
