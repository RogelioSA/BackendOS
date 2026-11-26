using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nest.core.aplicacion.corporativo.EstructuraOrganizacionales.Commands;
using nest.core.aplicacion.corporativo.EstructuraOrganizacionales.Queries;
using nest.core.dominio;
using nest.core.dominio.Corporativo.EstructuraOrganizacionalEntities;
using nest.core.security.Extensions;

namespace nest.core.security.Controllers.Corporativo
{
    /// <summary>
    /// Controlador para la gestión de estructuras organizacionales.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiExplorerSettings(GroupName = ConfigureEndpoints.EndpointsEnum.CORPORATIVO)]
    [Route($"{ConfigureEndpoints.EndpointsEnum.CORPORATIVO}/[controller]")]
    public class EstructuraOrganizacionalController : ControllerBase
    {
        private readonly ISender sender;

        public EstructuraOrganizacionalController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<EstructuraOrganizacional>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<EstructuraOrganizacional>>> ObtenerTodos(CancellationToken ct)
        {
            var entidades = await sender.Send(new ObtenerTodosQuery(), ct);
            return Ok(entidades);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(EstructuraOrganizacional), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<EstructuraOrganizacional?>> ObtenerPorId([FromRoute] int id, CancellationToken ct)
        {
            var entidad = await sender.Send(new ObtenerPorIdQuery(id), ct);
            return Ok(entidad);
        }

        [HttpGet("activos")]
        [ProducesResponseType(typeof(List<EstructuraOrganizacional>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<List<EstructuraOrganizacional>>> ObtenerActivos(CancellationToken ct)
        {
            var entidades = await sender.Send(new ObtenerActivosQuery(), ct);
            return Ok(entidades);
        }

        [HttpPost]
        [ProducesResponseType(typeof(EstructuraOrganizacional), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<EstructuraOrganizacional>> Agregar([FromBody] EstructuraOrganizacionalCrearCommand command, CancellationToken ct)
        {
            var entidad = await sender.Send(command, ct);
            return Ok(entidad);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(EstructuraOrganizacional), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult<EstructuraOrganizacional>> Modificar([FromRoute] int id, [FromBody] EstructuraOrganizacionalModificarCommand command, CancellationToken ct)
        {
            var entidad = await sender.Send(command with { Id = id }, ct);
            return Ok(entidad);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<ActionResult> Eliminar([FromRoute] int id, CancellationToken ct)
        {
            await sender.Send(new EstructuraOrganizacionalEliminarCommand(id), ct);
            return Ok(true);
        }
    }
}
