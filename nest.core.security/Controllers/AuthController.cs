using nest.core.aplicacion.security.Login;
using Microsoft.AspNetCore.Mvc;
using nest.core.dominio.Security;
using nest.core.dominio.Security.Auth;
using nest.core.dominio;
using Microsoft.AspNetCore.Authorization;

namespace nest.core.security.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly LoginUseCase loginUseCase;
        private readonly ILogger<AuthController> logger;
        public AuthController(LoginUseCase loginUseCase, ILogger<AuthController> logger)
        {
            this.loginUseCase = loginUseCase;
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
        public async Task<ActionResult<CustomAccessTokenResponse>> Login([FromBody] LoginDto login)
        {
            try
            {
                CustomAccessTokenResponse token = await loginUseCase.Execute(login);
                return Ok(token);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Inicia sesión y genera un token de acceso.
        /// </summary>
        /// <param name="empresa">Email del usuario y la empresa a cambiar.</param>
        /// <returns>Token de acceso con información del usuario.</returns>
        /// <response code="200">Cambio exitoso</response>
        /// <response code="401">No autorizado.</response>
        [Authorize]
        [HttpPost("changetenant")]
        public async Task<ActionResult<CustomAccessTokenResponse>> Login([FromBody] CambiarEmpresaDto empresa)
        {
            try
            {
                CustomAccessTokenResponse token = await loginUseCase.CambiarEmpresa(empresa);
                return Ok(token);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
