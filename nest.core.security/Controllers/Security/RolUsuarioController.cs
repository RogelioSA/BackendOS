using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nest.core.aplicacion.security.RoleUsuarios.Commands;
using nest.core.dominio;
using nest.core.security.Extensions;
using System.Collections.Generic;

namespace nest.core.security.Controllers.Security;

[Authorize]
[ApiController]
[ApiExplorerSettings(GroupName = ConfigureEndpoints.EndpointsEnum.SECURITY)]
[Route($"{ConfigureEndpoints.EndpointsEnum.SECURITY}/[controller]")]
public class RolUsuarioController : Controller
{
    private readonly ISender sender;

    public RolUsuarioController(ISender sender)
    {
        this.sender = sender;
    }

    [HttpPost("{roleId}")]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(ErrorMessage), 400)]
    [ProducesResponseType(401)]
    public async Task<ActionResult<bool>> Merge(string roleId, [FromBody] List<string> usersId, CancellationToken ct)
    {
        await sender.Send(new RoleUsuarioMergeCommand(roleId, usersId), ct);
        return Ok(true);
    }
}
