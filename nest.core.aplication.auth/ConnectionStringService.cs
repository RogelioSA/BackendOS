using Microsoft.Extensions.Configuration;
using nest.core.dominio.Security;
using nest.core.dominio.Security.Tenant;
using System.Security.Claims;

namespace nest.core.aplication.auth
{
    public class ConnectionStringService : IConnectionStringService
    {
        public string Engine { get; set; }
        public string Usuario { get; set; }
        public int? EmpresaId { get; set; }
        public RequestParameters Request { get; set; }
        public IConfigurationManager Configuration { get; set; }
        public string ConnectionString { get { return Configuration.GetConnectionString("DefaultConnection"); } }

        private readonly List<Claim> Claims;
        public ConnectionStringService(List<Claim> claims, RequestParameters request, IConfigurationManager Configuration)
        {
            this.Request = request;
            this.Configuration = Configuration;
            this.Claims = claims;
            try
            {
                Claim EmpresaClaim = this.Claims.SingleOrDefault(x => x.Type == ClaimTypesCustom.EMPRESAID);
                this.Usuario = this.Claims.SingleOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
                this.EmpresaId = EmpresaClaim == null ? null : string.IsNullOrWhiteSpace(EmpresaClaim.Value) ? null : int.Parse(EmpresaClaim.Value);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading ConnectionService", ex.Message);
            }
        }
    }
}
