namespace nest.iac.servicesinfra.Entities
{
    public class ServiceConfig
    {
        public string serviceName { get; set; } = null!;
        public string routepath { get; set; } = null!;
        public string contextDocker { get; set; } = null!;
        public string pathProject { get; set; } = null!;
        public int port { get; set; }
    }
}
