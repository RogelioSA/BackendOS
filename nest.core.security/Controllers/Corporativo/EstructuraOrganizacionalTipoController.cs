using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nest.core.aplicacion.corporativo.EstructuraOrganizacionalTipos.Commands;
using nest.core.aplicacion.corporativo.EstructuraOrganizacionalTipos.Queries;
using nest.core.dominio;
using nest.core.dominio.Corporativo.EstructuraOrganizacionalTipoEntities;
using nest.core.security.Extensions;

namespace nest.core.security.Controllers.Corporativo
{
    /// <summary>
    /// Controlador para la gestión de tipos de estructura organizacional.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiExplorerSettings(GroupName = ConfigureEndpoints.EndpointsEnum.CORPORATIVO)]
    [Route($"{ConfigureEndpoints.EndpointsEnum.CORPORATIVO}/[controller]")]
    public class EstructuraOrganizacionalTipoController : ControllerBase
    {
        private readonly ISender sender;

        public EstructuraOrganizacionalTipoController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<EstructuraOrganizacionalTipo>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<EstructuraOrganizacionalTipo>>> ObtenerTodos(CancellationToken ct)
        {
            var entidades = await sender.Send(new ObtenerTodosQuery(), ct);
            return Ok(entidades);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(EstructuraOrganizacionalTipo), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<EstructuraOrganizacionalTipo?>> ObtenerPorId([FromRoute] int id, CancellationToken ct)
        {
            var entidad = await sender.Send(new ObtenerPorIdQuery(id), ct);
            return Ok(entidad);
        }

        [HttpGet("activos")]
        [ProducesResponseType(typeof(List<EstructuraOrganizacionalTipo>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<EstructuraOrganizacionalTipo>>> ObtenerActivos(CancellationToken ct)
        {
            var entidades = await sender.Send(new ObtenerActivosQuery(), ct);
            return Ok(entidades);
        }

        [HttpPost]
        [ProducesResponseType(typeof(EstructuraOrganizacionalTipo), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<EstructuraOrganizacionalTipo>> Agregar([FromBody] EstructuraOrganizacionalTipoCrearCommand command, CancellationToken ct)
        {
            var entidad = await sender.Send(command, ct);
            return Ok(entidad);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(EstructuraOrganizacionalTipo), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<EstructuraOrganizacionalTipo>> Modificar([FromRoute] int id, [FromBody] EstructuraOrganizacionalTipoModificarCommand command, CancellationToken ct)
        {
            var entidad = await sender.Send(command with { Id = id }, ct);
            return Ok(entidad);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult> Eliminar([FromRoute] int id, CancellationToken ct)
        {
            await sender.Send(new EstructuraOrganizacionalTipoEliminarCommand(id), ct);
            return Ok(true);
        }
    }
}
