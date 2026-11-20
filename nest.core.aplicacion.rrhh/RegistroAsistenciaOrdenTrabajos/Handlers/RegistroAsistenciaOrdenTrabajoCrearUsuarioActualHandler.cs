using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using nest.core.aplicacion.rrhh.RegistroAsistenciaOrdenTrabajos.Commands;
using nest.core.aplicacion.rrhh.RegistroAsistencias.Services.Interface;
using nest.core.dominio.Mantto.OrdenTrabajoCabeceraEntities;
using nest.core.dominio.Mantto.OrdenTrabajoHorarioEntities;
using nest.core.dominio.RRHH.HorarioCabeceraEntities;
using nest.core.dominio.RRHH.HorarioDetalleEventoEntities;
using nest.core.dominio.RRHH.PersonalEntities;
using nest.core.dominio.RRHH.RegistroAsistenciaAdjuntosEntities;
using nest.core.dominio.RRHH.RegistroAsistenciaEntities;
using nest.core.dominio.RRHH.RegistroAsistenciaOrdenTrabajoEntities;
using nest.core.dominio.Security.Tenant;
using nest.core.dominio.Transaccional;

namespace nest.core.aplicacion.rrhh.RegistroAsistenciaOrdenTrabajos.Handlers
{
    public class RegistroAsistenciaOrdenTrabajoCrearUsuarioActualHandler : IRequestHandler<RegistroAsistenciaOrdenTrabajoCrearUsuarioActualCommand, RegistroAsistencia>
    {
        private readonly IRegistroAsistencia_OrdenTrabajoRepository repository;
        private readonly IPersonalRepository personalRepository;
        private readonly IRegistroAsistenciaOrdenTrabajoRepository registroOrdenTrabajoRepository;
        private readonly IRegistroAsistenciaAdjuntosRepository registroAsistenciaAdjuntosRepository;
        private readonly IOrdenTrabajoHorarioRepository ordenTrabajoHorarioRepository;
        private readonly IConnectionStringService connectionStringService;
        private readonly IMarcacionCalculoService calculoService;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IHorarioRepository horarioRepository;
        private readonly ILogger<RegistroAsistenciaOrdenTrabajoCrearUsuarioActualHandler> logger;

        public RegistroAsistenciaOrdenTrabajoCrearUsuarioActualHandler(
            IRegistroAsistencia_OrdenTrabajoRepository repository,
            IPersonalRepository personalRepository,
            IRegistroAsistenciaOrdenTrabajoRepository registroOrdenTrabajoRepository,
            IRegistroAsistenciaAdjuntosRepository registroAsistenciaAdjuntosRepository,
            IOrdenTrabajoHorarioRepository ordenTrabajoHorarioRepository,
            IConnectionStringService connectionStringService,
            IMarcacionCalculoService calculoService,
            IHorarioRepository horarioRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<RegistroAsistenciaOrdenTrabajoCrearUsuarioActualHandler> logger)
        {
            this.repository = repository;
            this.personalRepository = personalRepository;
            this.registroOrdenTrabajoRepository = registroOrdenTrabajoRepository;
            this.registroAsistenciaAdjuntosRepository = registroAsistenciaAdjuntosRepository;
            this.ordenTrabajoHorarioRepository = ordenTrabajoHorarioRepository;
            this.connectionStringService = connectionStringService;
            this.calculoService = calculoService;
            this.unitOfWork = unitOfWork;
            this.horarioRepository = horarioRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<RegistroAsistencia> Handle(RegistroAsistenciaOrdenTrabajoCrearUsuarioActualCommand request, CancellationToken cancellationToken)
        {
            await unitOfWork.BeginTransactionAsync();
            try
            {
                var registro = mapper.Map<RegistroAsistencia>(request);
                registro.Fecha = DateTime.Now;
                registro.EmpresaId = connectionStringService.EmpresaId ?? throw new Exception("Usuario no autenticado");
                var personal = await personalRepository.ObtenerPorIdUsuario(connectionStringService.UserId) ?? throw new Exception("El usuario debe tener el atributo IdUsuario");
                registro.PersonalId = personal.Id;
                if(!personal.Persona.Estado) throw new Exception("El personal se encuentra inactivo");
                var fechaBusqueda = registro.Fecha.AddHours(-22);

                var ultimaMarca = await repository.BuscarUltimaMarca(registro.PersonalId, fechaBusqueda);
                HorarioCabecera? horarioActual = null;
                OrdenTrabajoCabecera? ordenTrabajoCabecera = null;
                if (ultimaMarca != null && ultimaMarca.TipoEvento == HorarioDetalleEventoTipoEnum.Entrada)
                {
                    horarioActual = await horarioRepository.ObtenerPorId(ultimaMarca.HorarioDetalleEvento.HorarioDetalle.HorarioCabeceraId);
                    ordenTrabajoCabecera = ultimaMarca.RegistroAsistenciaOrdenTrabajo?.OrdenTrabajoCabecera;
                }
                else
                {
                    var otHorario = await ordenTrabajoHorarioRepository.ObtenerPorPersonalYFecha(registro.PersonalId, registro.Fecha);
                    horarioActual = otHorario == null ? personal.HorarioCabecera : otHorario.HorarioCabecera;
                    ordenTrabajoCabecera = otHorario?.OrdenTrabajoCabecera;
                }
                registro = await calculoService.PrepararRegistroAsync(registro, horarioActual);
                registro = await repository.Agregar(registro);

                if (ordenTrabajoCabecera != null)
                {
                    var relacion = new RegistroAsistenciaOrdenTrabajo
                    {
                        EmpresaId = registro.EmpresaId,
                        Id = registro.Id,
                        OrdenTrabajoCabeceraId = ordenTrabajoCabecera.Id
                    };
                    await registroOrdenTrabajoRepository.Agregar(relacion);
                }
                if (request.AdjuntoId > 0)
                {
                    var adjunto = new dominio.RRHH.RegistroAsistenciaAdjuntosEntities.RegistroAsistenciaAdjuntos
                    {
                        EmpresaId = registro.EmpresaId,
                        RegistroAsistenciaId = registro.Id,
                        AdjuntoId = request.AdjuntoId,
                        Tipo = RegistroAsistenciaAdjuntoTipoEnum.Foto
                    };
                    await registroAsistenciaAdjuntosRepository.Agregar(adjunto);
                }
                await unitOfWork.CommitAsync();
                return await repository.ObtenerPorId(registro.Id);
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync();
                logger.LogError(ex, "Error al registrar asistencia de orden de trabajo para el usuario actual");
                throw;
            }
            finally
            {
                await unitOfWork.DisposeAsync();
            }
        }
    }
}
