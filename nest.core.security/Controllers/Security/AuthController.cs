using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nest.core.aplicacion.security.Login.Commands;
using nest.core.dominio.Security;
using nest.core.security.Extensions;
namespace nest.core.security.Controllers.Security;

[ApiController]
[ApiExplorerSettings(GroupName = ConfigureEndpoints.EndpointsEnum.SECURITY)]
[Route($"{ConfigureEndpoints.EndpointsEnum.SECURITY}/[controller]")]
public class AuthController : Controller
{
    private readonly ISender sender;
    private readonly ILogger<AuthController> logger;

    public AuthController(ISender sender, ILogger<AuthController> logger)
    {
        this.sender = sender;
        this.logger = logger;
    }

    /// <summary>
    /// Inicia sesión y genera un token de acceso.
    /// </summary>
    /// <param name="login">Credenciales del usuario.</param>
    /// <returns>Token de acceso con información del usuario.</returns>
    /// <response code="200">Login exitoso</response>
    /// <response code="400">Error en la solicitud o credenciales incorrectas</response>
    [HttpPost("login")]
    public async Task<ActionResult<CustomAccessTokenResponse>> Login([FromBody] LoginCommand login, CancellationToken ct)
    {
        try
        {
            CustomAccessTokenResponse token = await sender.Send(login, ct);
            return Ok(token);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            throw;
        }
    }

    /// <summary>
    /// Cambia la empresa seleccionada del usuario autenticado.
    /// </summary>
    /// <param name="command">Email del usuario y la empresa a cambiar.</param>
    /// <returns>Token de acceso con información del usuario.</returns>
    /// <response code="200">Cambio exitoso</response>
    /// <response code="401">No autorizado.</response>
    [Authorize]
    [HttpPost("changetenant")]
    public async Task<ActionResult<CustomAccessTokenResponse>> CambiarEmpresa([FromBody] CambiarEmpresaCommand command, CancellationToken ct)
    {
        try
        {
            CustomAccessTokenResponse token = await sender.Send(command, ct);
            return Ok(token);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            throw;
        }
    }

    [HttpPost("login_document")]
    public async Task<ActionResult<CustomAccessTokenResponse>> LoginPorDocumentoIdentidad([FromBody] LoginDocumentoIdentidadCommand login, CancellationToken ct)
    {
        try
        {
            CustomAccessTokenResponse token = await sender.Send(login, ct);
            return Ok(token);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            throw;
        }
    }
}
