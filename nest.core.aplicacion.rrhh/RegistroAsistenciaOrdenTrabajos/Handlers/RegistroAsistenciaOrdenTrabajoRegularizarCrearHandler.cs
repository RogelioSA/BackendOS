using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using nest.core.aplicacion.rrhh.RegistroAsistenciaOrdenTrabajos.Commands;
using nest.core.aplicacion.rrhh.RegistroAsistencias.Services.Interface;
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
    public class RegistroAsistenciaOrdenTrabajoRegularizarCrearHandler : IRequestHandler<RegistroAsistenciaOrdenTrabajoRegularizarCrearCommand, RegistroAsistencia>
    {
        private readonly IRegistroAsistencia_OrdenTrabajoRepository repository;
        private readonly IRegistroAsistenciaOrdenTrabajoRepository registroOrdenTrabajoRepository;
        private readonly IRegistroAsistenciaAdjuntosRepository registroAsistenciaAdjuntosRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IPersonalRepository personalRepository;
        private readonly IOrdenTrabajoHorarioRepository ordenTrabajoHorarioRepository;
        private readonly IConnectionStringService connectionStringService;
        private readonly IHorarioRepository horarioRepository;
        private readonly ILogger<RegistroAsistenciaOrdenTrabajoRegularizarCrearHandler> logger;

        public RegistroAsistenciaOrdenTrabajoRegularizarCrearHandler(
            IRegistroAsistencia_OrdenTrabajoRepository repository,
            IPersonalRepository personalRepository,
            IRegistroAsistenciaOrdenTrabajoRepository registroOrdenTrabajoRepository,
            IRegistroAsistenciaAdjuntosRepository registroAsistenciaAdjuntosRepository,
            IOrdenTrabajoHorarioRepository ordenTrabajoHorarioRepository,
            IMarcacionCalculoService calculoService,
            IConnectionStringService connectionStringService,
            IUnitOfWork unitOfWork,
            IHorarioRepository horarioRepository,
            IMapper mapper,
            ILogger<RegistroAsistenciaOrdenTrabajoRegularizarCrearHandler> logger)
        {
            this.repository = repository;
            this.registroOrdenTrabajoRepository = registroOrdenTrabajoRepository;
            this.personalRepository = personalRepository;
            this.registroAsistenciaAdjuntosRepository = registroAsistenciaAdjuntosRepository;
            this.connectionStringService = connectionStringService;
            this.horarioRepository = horarioRepository;
            this.ordenTrabajoHorarioRepository = ordenTrabajoHorarioRepository;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<RegistroAsistencia> Handle(RegistroAsistenciaOrdenTrabajoRegularizarCrearCommand request, CancellationToken cancellationToken)
        {
            await unitOfWork.BeginTransactionAsync();
            try
            {
                var registro = mapper.Map<RegistroAsistencia>(request);
                registro.Fecha = request.Fecha ?? DateTime.Now;

                Personal personal = null;

                if (request.TipoRegularizacion == RegistroAsistenciaTipoRegularizacionId.Automatico)
                {
                    personal = await personalRepository.ObtenerPorIdUsuario(connectionStringService.UserId);
                    registro.PersonalId = personal.Id;
                }
                else
                {
                    personal = await personalRepository.ObtenerPorId(registro.PersonalId);
                    registro.PersonalId = request.PersonalId;
                }

                var fechaBusqueda = registro.Fecha.AddHours(-22);
                var ultimaMarca = await repository.BuscarUltimaMarca(registro.PersonalId, fechaBusqueda, registro.Fecha);
                HorarioCabecera? horarioActual = null;

                if (request.EventoTipo == HorarioDetalleEventoTipoEnum.Entrada)
                {
                    if (!(ultimaMarca != null && ultimaMarca.TipoEvento == HorarioDetalleEventoTipoEnum.Entrada))
                    {
                        var otHorario = await ordenTrabajoHorarioRepository.ObtenerPorPersonalYFecha(registro.PersonalId, registro.Fecha);
                        horarioActual = otHorario == null ? personal.HorarioCabecera : otHorario.HorarioCabecera;
                        registro.FechaJornal = DateOnly.FromDateTime(registro.Fecha);
                    }
                    else throw new InvalidOperationException("No se puede registrar una entrada si ya existe una entrada previa.");
                }
                else
                {
                    if (ultimaMarca != null && ultimaMarca.TipoEvento == HorarioDetalleEventoTipoEnum.Entrada)
                    {
                        int? horarioCabeceraId = ultimaMarca.HorarioDetalleEvento?.HorarioDetalle?.HorarioCabeceraId;
                        if (horarioCabeceraId.HasValue)
                            horarioActual = await horarioRepository.ObtenerPorId(ultimaMarca.HorarioDetalleEvento.HorarioDetalle.HorarioCabeceraId);
                        else
                            horarioActual = personal.HorarioCabecera;
                        registro.FechaJornal = ultimaMarca.FechaJornal;
                    }
                    else throw new InvalidOperationException("No se puede registrar una salida u otro tipo de marca si no existe una entrada previa.");
                }

                var eventoDia = horarioActual.HorarioDetalles
                    .Where(x => x.DiaSemana == registro.FechaJornal.DayOfWeek)?.FirstOrDefault();

                if (eventoDia == null)
                    throw new InvalidOperationException($"No se encontró un horario válido para el día {registro.FechaJornal.DayOfWeek.ToString("dddd")}.");

                var evento = eventoDia.HorarioDetalleEventos
                    .Where(x => x.TipoEvento == request.EventoTipo)?.FirstOrDefault();

                if (evento == null)
                    throw new InvalidOperationException($"No se encontró un evento válido para el tipo {request.EventoTipo} para el dia {registro.FechaJornal.DayOfWeek.ToString("dddd")}.");

                registro.TipoEvento = request.EventoTipo;
                registro.DiferenciaMinutos = 0;
                registro.EsTardanza = false;
                registro.HorarioDetalleEventoId = evento?.Id;
                registro.RegistroAsistenciaPoliticaId = null;
                registro.Observacion = request.Observacion;

                registro = await repository.Agregar(registro);

                if (request.OrdenTrabajoId.HasValue)
                {
                    var relacion = new RegistroAsistenciaOrdenTrabajo
                    {
                        EmpresaId = registro.EmpresaId,
                        Id = registro.Id,
                        OrdenTrabajoCabeceraId = request.OrdenTrabajoId.Value
                    };
                    await registroOrdenTrabajoRepository.Agregar(relacion);
                }
                if (request.AdjuntoId > 0) 
                {
                    var adjunto = new dominio.RRHH.RegistroAsistenciaAdjuntosEntities.RegistroAsistenciaAdjuntos
                    {
                        EmpresaId = registro.EmpresaId,
                        RegistroAsistenciaId = registro.Id,
                        AdjuntoId = request.AdjuntoId
                    };
                    await registroAsistenciaAdjuntosRepository.Agregar(adjunto);
                }
                await unitOfWork.CommitAsync();
                return await repository.ObtenerPorId(registro.Id);
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync();
                logger.LogError(ex, "Error al registrar asistencia de orden de trabajo para el personal {PersonalId}", request.PersonalId);
                throw;
            }
            finally
            {
                await unitOfWork.DisposeAsync();
            }
        }
    }
}
