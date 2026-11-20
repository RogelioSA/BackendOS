using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using nest.core.aplicacion.rrhh.Mapper;
using nest.core.aplicacion.rrhh.RegistroAsistencias.Services;
using nest.core.aplicacion.rrhh.RegistroAsistencias.Services.Interface;
using nest.core.aplication.auth;
using nest.core.dominio.Mantto.OrdenTrabajoCabeceraEntities;
using nest.core.dominio.Mantto.OrdenTrabajoHorarioEntities;
using nest.core.dominio.RRHH.CargoEntities;
using nest.core.dominio.RRHH.GrupoTrabajoEntities;
using nest.core.dominio.RRHH.GrupoTrabajoPersonaEntities;
using nest.core.dominio.RRHH.HorarioCabeceraEntities;
using nest.core.dominio.RRHH.HorarioDetalleEntities;
using nest.core.dominio.RRHH.HorarioDetalleEventoEntities;
using nest.core.dominio.RRHH.PersonalCargoExternoEntities;
using nest.core.dominio.RRHH.PersonalEntities;
using nest.core.dominio.RRHH.PersonalEstadoEntities;
using nest.core.dominio.RRHH.RegistroAsistenciaAdjuntoEntities;
using nest.core.dominio.RRHH.RegistroAsistenciaAdjuntosEntities;
using nest.core.dominio.RRHH.RegistroAsistenciaEntities;
using nest.core.dominio.RRHH.RegistroAsistenciaOrdenTrabajoEntities;
using nest.core.dominio.RRHH.RegistroAsistenciaPoliticaEntities;
using nest.core.dominio.RRHH.TerminalBiometricoEntities;
using nest.core.dominio.Security.Tenant;
using nest.core.dominio.Transaccional;
using nest.core.infraestructura.db.Transaccional;
using nest.core.infraestructura.mantto;
using nest.core.infraestructura.rrhh;
using nest.core.infraestructura.rrhh.Extensiones;

namespace nest.core.aplicacion.rrhh
{
    public static class ConfigureServices
    {
        public static IServiceCollection ConfigureInfraestructura(this IServiceCollection services, IConfigurationManager configuration)
        {
            services.AddAutoMapper(cfg => cfg.AddProfile<AutoMapperProfiles>());
            services.AddTransient<IConnectionStringService>((serviceProvider) => AuthClaim.constructClaimsAuth(serviceProvider, configuration));
            services.AddTransient<IUnitOfWork, EfUnitOfWork>();
            services.AddTransient<ICargoRepository, CargoRepository>();
            services.AddTransient<IGrupoTrabajoRepository, GrupoTrabajoRepository>();
            services.AddTransient<IGrupoTrabajoPersonaRepository, GrupoTrabajoPersonaRepository>();
            services.AddTransient<IHorarioRepository, HorarioCabeceraRepository>();
            services.AddTransient<IHorarioDetalleRepository, HorarioDetalleRepository>();
            services.AddTransient<IHorarioDetalleEventoRepository, HorarioDetalleEventoRepository>();
            services.AddTransient<IPersonalRepository, PersonalRepository>();
            services.AddTransient<IPersonalCargoExternoRepository, PersonalCargoExternoRepository>();
            services.AddTransient<IPersonalEstadoRepository, PersonalEstadoRepository>();
            services.AddTransient<IRegistroAsistenciaAdjuntoRepository, RegistroAsistenciaAdjuntoRepository>();
            services.AddTransient<IRegistroAsistenciaAdjuntosRepository, RegistroAsistenciaAdjuntosRepository>();
            services.AddTransient<IRegistroAsistenciaRepository, RegistroAsistenciaRepository>();
            services.AddTransient<IRegistroAsistenciaOrdenTrabajoRepository, RegistroAsistenciaOrdenTrabajoRepository>();
            services.AddTransient<IRegistroAsistenciaPoliticaRepository, RegistroAsistenciaPoliticaRepository>();
            services.AddTransient<ITerminalBiometricoRepository, TerminalBiometricoRepository>();
            services.AddTransient<IOrdenTrabajoCabeceraRepository, OrdenTrabajoCabeceraRepository>();
            services.AddTransient<IRegistroAsistencia_OrdenTrabajoRepository, RegistroAsistencia_OrdenTrabajoRepository>();
            services.AddTransient<IOrdenTrabajoHorarioRepository, OrdenTrabajoHorarioRepository>();
            services.AddTransient<IMarcacionCalculoService, RegistroAsistenciaCalculoService>();
            return services;
        }
    }
}
