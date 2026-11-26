using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using nest.core.aplicacion.rrhh.RegistroAsistenciaOrdenTrabajos.Commands;
using nest.core.aplicacion.rrhh.RegistroAsistencias.Services.Interface;
using nest.core.dominio.Mantto.OrdenTrabajoHorarioEntities;
using nest.core.dominio.RRHH.HorarioCabeceraEntities;
using nest.core.dominio.RRHH.PersonalEntities;
using nest.core.dominio.RRHH.RegistroAsistenciaAdjuntosEntities;
using nest.core.dominio.RRHH.RegistroAsistenciaEntities;
using nest.core.dominio.RRHH.RegistroAsistenciaOrdenTrabajoEntities;
using nest.core.dominio.Transaccional;

namespace nest.core.aplicacion.rrhh.RegistroAsistenciaOrdenTrabajos.Handlers
{
    public class RegistroAsistenciaOrdenTrabajoCrearHandler : IRequestHandler<RegistroAsistenciaOrdenTrabajoCrearCommand, RegistroAsistencia>
    {
        private readonly IRegistroAsistencia_OrdenTrabajoRepository repository;
        private readonly IPersonalRepository personalRepository;
        private readonly IRegistroAsistenciaOrdenTrabajoRepository registroOrdenTrabajoRepository;
        private readonly IRegistroAsistenciaAdjuntosRepository registroAsistenciaAdjuntosRepository;
        private readonly IOrdenTrabajoHorarioRepository ordenTrabajoHorarioRepository;
        private readonly IMarcacionCalculoService calculoService;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ILogger<RegistroAsistenciaOrdenTrabajoCrearHandler> logger;

        public RegistroAsistenciaOrdenTrabajoCrearHandler(
            IRegistroAsistencia_OrdenTrabajoRepository repository,
            IPersonalRepository personalRepository,
            IRegistroAsistenciaOrdenTrabajoRepository registroOrdenTrabajoRepository,
            IRegistroAsistenciaAdjuntosRepository registroAsistenciaAdjuntosRepository,
            IOrdenTrabajoHorarioRepository ordenTrabajoHorarioRepository,
            IMarcacionCalculoService calculoService,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<RegistroAsistenciaOrdenTrabajoCrearHandler> logger)
        {
            this.repository = repository;
            this.personalRepository = personalRepository;
            this.registroOrdenTrabajoRepository = registroOrdenTrabajoRepository;
            this.registroAsistenciaAdjuntosRepository = registroAsistenciaAdjuntosRepository;
            this.ordenTrabajoHorarioRepository = ordenTrabajoHorarioRepository;
            this.calculoService = calculoService;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<RegistroAsistencia> Handle(RegistroAsistenciaOrdenTrabajoCrearCommand request, CancellationToken cancellationToken)
        {
            await unitOfWork.BeginTransactionAsync();
            try
            {
                var registro = mapper.Map<RegistroAsistencia>(request);
                var personal = await personalRepository.ObtenerPorId(registro.PersonalId);
                var otHorario = await ordenTrabajoHorarioRepository.ObtenerPorPersonalYFecha(registro.PersonalId, registro.Fecha);
                if(!personal.Persona.Estado) throw new Exception("El personal se encuentra inactivo");
                HorarioCabecera horarioActual = otHorario == null ? personal.HorarioCabecera : otHorario.HorarioCabecera;
                registro = await calculoService.PrepararRegistroAsync(registro, horarioActual);
                registro = await repository.Agregar(registro);

                if (otHorario != null)
                {
                    var relacion = new RegistroAsistenciaOrdenTrabajo
                    {
                        EmpresaId = registro.EmpresaId,
                        Id = registro.Id,
                        OrdenTrabajoCabeceraId = otHorario.OrdenTrabajoCabeceraId
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
