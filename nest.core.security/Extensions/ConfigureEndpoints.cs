using nest.core.security.Models;

namespace nest.core.security.Extensions
{
    public static class ConfigureEndpoints
    {
        public static class EndpointsEnum
        {
            public const string SECURITY = "security";
            public const string RRHH = "rrhh";
            public const string LOGISTICA = "logistica";
            public const string CORPORATIVO = "corporativo";
            public const string LEGAL = "legal";
            public const string GENERAL = "general";
            public const string COSTOS = "costos";
            public const string FINANZAS = "finanzas";
            public const string CONTABILIDAD = "contabilidad";
            public const string MANTTO = "mantto";
            public const string PATRIMONIAL = "patrimonial";
            public const string DATASOURCE = "datasource";
            public const string ICLOCK = "iclock";
        }

        public static Dictionary<string, SwaggerModelEndpoint> Endpoints = new Dictionary<string, SwaggerModelEndpoint>
        {
            { EndpointsEnum.SECURITY, new SwaggerModelEndpoint { Title = "Security API", Version = "v1", MainPath = "security" } },
            { EndpointsEnum.RRHH, new SwaggerModelEndpoint { Title = "RRHH API", Version = "v1", MainPath = "rrhh" } },
            { EndpointsEnum.LOGISTICA, new SwaggerModelEndpoint { Title = "Logística API", Version = "v1", MainPath = "logistica" } },
            { EndpointsEnum.CORPORATIVO, new SwaggerModelEndpoint { Title = "Corporativo API", Version = "v1", MainPath = "corporativo" } },
            { EndpointsEnum.LEGAL, new SwaggerModelEndpoint { Title = "Legal API", Version = "v1", MainPath = "legal" } },
            { EndpointsEnum.GENERAL, new SwaggerModelEndpoint { Title = "General API", Version = "v1", MainPath = "general" } },
            { EndpointsEnum.COSTOS, new SwaggerModelEndpoint { Title = "Costos API", Version = "v1", MainPath = "costos" } },
            { EndpointsEnum.FINANZAS, new SwaggerModelEndpoint { Title = "Finanzas API", Version = "v1", MainPath = "finanzas" } },
            { EndpointsEnum.CONTABILIDAD, new SwaggerModelEndpoint { Title = "Contabilidad API", Version = "v1", MainPath = "contabilidad" } },
            { EndpointsEnum.MANTTO, new SwaggerModelEndpoint { Title = "Mantenimiento API", Version = "v1", MainPath = "mantto" } },
            { EndpointsEnum.PATRIMONIAL, new SwaggerModelEndpoint { Title = "Patrimonial API", Version = "v1", MainPath = "patrimonial" } },
            //{ EndpointsEnum.DATASOURCE, new SwaggerModelEndpoint { Title = "Datasource API", Version = "v1", MainPath = "datasource" } },
            { EndpointsEnum.ICLOCK, new SwaggerModelEndpoint { Title = "iClock API", Version = "v1", MainPath = "iclock" } }

        };

        
    }
}
