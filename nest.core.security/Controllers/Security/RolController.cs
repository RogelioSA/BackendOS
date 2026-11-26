using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nest.core.aplicacion.security.Roles.Commands;
using nest.core.aplicacion.security.Roles.Queries;
using nest.core.dominio;
using nest.core.dominio.Security;
using nest.core.security.Extensions;

namespace nest.core.security.Controllers.Security;

[Authorize]
[ApiController]
[ApiExplorerSettings(GroupName = ConfigureEndpoints.EndpointsEnum.SECURITY)]
[Route($"{ConfigureEndpoints.EndpointsEnum.SECURITY}/[controller]")]
public class RolController : Controller
{
    private readonly ISender sender;

    public RolController(ISender sender)
    {
        this.sender = sender;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<ApplicationRole>), 200)]
    [ProducesResponseType(typeof(ErrorMessage), 400)]
    [ProducesResponseType(401)]
    public async Task<ActionResult<List<ApplicationRole>>> ObtenerTodos(CancellationToken ct)
    {
        var data = await sender.Send(new ObtenerRolesQuery(), ct);
        return Ok(data);
    }

    [HttpPost("filter")]
    [ProducesResponseType(typeof(LoadResult), 200)]
    [ProducesResponseType(typeof(ErrorMessage), 400)]
    [ProducesResponseType(401)]
    public async Task<ActionResult<LoadResult>> ObtenerFiltro([FromBody] DataSourceLoadOptionsBase loadOptions, CancellationToken ct)
    {
        var data = await sender.Send(new ObtenerRolesFilterQuery(loadOptions), ct);
        return Ok(data);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApplicationRole), 200)]
    [ProducesResponseType(typeof(ErrorMessage), 400)]
    [ProducesResponseType(401)]
    public async Task<ActionResult<ApplicationRole>> ObtenerPorId(string id, CancellationToken ct)
    {
        var data = await sender.Send(new ObtenerRolePorIdQuery(id), ct);
        return Ok(data);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApplicationRole), 200)]
    [ProducesResponseType(typeof(ErrorMessage), 400)]
    [ProducesResponseType(401)]
    public async Task<ActionResult<ApplicationRole>> Agregar([FromBody] RoleCrearCommand command, CancellationToken ct)
    {
        var data = await sender.Send(command, ct);
        return Ok(data);
    }

    [HttpPut("{roleId}")]
    [ProducesResponseType(typeof(ApplicationRole), 200)]
    [ProducesResponseType(typeof(ErrorMessage), 400)]
    [ProducesResponseType(401)]
    public async Task<ActionResult<ApplicationRole>> Modificar(int roleId, [FromBody] RoleModificarCommand command, CancellationToken ct)
    {
        var data = await sender.Send(command with { Id = roleId }, ct);
        return Ok(data);
    }

    [HttpDelete("{roleId}")]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(ErrorMessage), 400)]
    [ProducesResponseType(401)]
    public async Task<ActionResult> Eliminar(int roleId, CancellationToken ct)
    {
        await sender.Send(new RoleEliminarCommand(roleId), ct);
        return Ok(true);
    }
}
