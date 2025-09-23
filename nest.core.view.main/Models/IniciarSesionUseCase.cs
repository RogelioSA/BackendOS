using Microsoft.AspNetCore.Identity.Data;
using nest.core.dominio.Security.Auth;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace nest.core.view.main.Models
{
    public class IniciarSesionUseCase
    {
        private readonly RequestRestApi restApi;
        public IniciarSesionUseCase(RequestRestApi restApi)
        {
            this.restApi = restApi;
        }

        public async Task<AuthResponse> execute(LoginDto input)
        {
            Dictionary<string, string> keys = new Dictionary<string, string>();
            return await restApi.executeRequest<AuthResponse, LoginDto>("/security/Auth/login", input, keys);
        }
    }
}
