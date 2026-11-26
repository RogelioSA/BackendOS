using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nest.core.aplicacion.security.RoleClaims.Commands;
using nest.core.dominio;
using nest.core.dominio.Security.Dto;
using nest.core.security.Extensions;

namespace nest.core.security.Controllers.Security;

[Authorize]
[ApiController]
[ApiExplorerSettings(GroupName = ConfigureEndpoints.EndpointsEnum.SECURITY)]
[Route($"{ConfigureEndpoints.EndpointsEnum.SECURITY}/[controller]")]
public class RolClaimController : Controller
{
    private readonly ISender sender;

    public RolClaimController(ISender sender)
    {
        this.sender = sender;
    }

    [HttpPost("{roleId}")]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(ErrorMessage), 400)]
    [ProducesResponseType(401)]
    public async Task<ActionResult<bool>> Merge(string roleId, [FromBody] List<ClaimDto> claims, CancellationToken ct)
    {
        await sender.Send(new RoleClaimsMergeCommand(roleId, claims), ct);
        return Ok(true);
    }
}
