using AutoMapper;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Connections;
using Microsoft.EntityFrameworkCore;
using nest.core.dominio;
using nest.core.dominio.RRHH.HorarioDetalleEventoEntities;
using nest.core.dominio.RRHH.PersonalEntities;
using nest.core.dominio.RRHH.RegistroAsistenciaEntities;
using nest.core.infraestructura.db.DbContext;
using nest.core.infraestructura.db.Utils;
using nest.core.infrastructura.utils.Excepciones;

namespace nest.core.infraestructura.rrhh
{
    public class RegistroAsistenciaRepository : CrudRepositoryBase<RegistroAsistencia, long>, IRegistroAsistenciaRepository
    {
        public RegistroAsistenciaRepository(NestDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        protected override IQueryable<RegistroAsistencia> Query()
        {
            return base.Query()
                .Include(x => x.Personal).ThenInclude(x => x.Persona).ThenInclude(x => x.DocumentoIdentidadTipo)
                .Include(x => x.Personal).ThenInclude(x => x.Persona).ThenInclude(x => x.LicenciaConducir)
                .Include(x => x.Personal).ThenInclude(x => x.Persona).ThenInclude(x => x.Sexo)
                .Include(x => x.RegistroAsistenciaPolitica)
                .Include(x => x.HorarioDetalleEvento).ThenInclude(x => x.HorarioDetalle).ThenInclude(x => x.HorarioCabecera)
                .AsNoTracking()
                .AsSplitQuery();
        }

        protected IQueryable<RegistroAsistenciaQueryView> QueryOt()
        {
            var resultado = base.Query()
                .Include(x => x.Personal).ThenInclude(x => x.Persona)
                .Include(x => x.RegistroAsistenciaAdjunto)
                .Include(x => x.RegistroAsistenciaAdjuntos)
                .Include(x => x.RegistroAsistenciaOrdenTrabajo).ThenInclude(x => x.OrdenTrabajoCabecera).ThenInclude(x => x.OrdenServicioCabecera)
                .Include(x => x.Personal).ThenInclude(x => x.PersonalCargoExterno)
                .AsNoTracking()
                .AsSplitQuery()
                .Select(x => new RegistroAsistenciaQueryView {
                    EmpresaId = x.EmpresaId,
                    Id = x.Id,
                    PersonalId = x.PersonalId,
                    Fecha = x.Fecha,
                    FechaJornal = x.FechaJornal,
                    TipoEvento = x.TipoEvento,
                    EsTardanza = x.EsTardanza,
                    DiferenciaMinutos = x.DiferenciaMinutos,
                    Latitud = x.Latitud,
                    Longitud = x.Longitud,
                    AdjuntoId = x.RegistroAsistenciaAdjunto.AdjuntoId,
                    AdjuntoUrl = $"{ConfigVariables.GeneralService}/Adjunto/download/{x.RegistroAsistenciaAdjunto.AdjuntoId}",
                    MinutosDescanso = x.HorarioDetalleEvento.HorarioDetalle.HorarioCabecera.MinutosDescanso,
                    MinutosTraslado = x.HorarioDetalleEvento.HorarioDetalle.HorarioCabecera.MinutosTraslado,
                    PersonalCargoExterno = x.Personal.PersonalCargoExterno,
                    Personal = new PersonalQueryView {
                    //    Id = x.Personal.Id,
                    //    MarcaAsistencia = x.Personal.MarcaAsistencia,
                    //    ContratoCabeceraId = x.Personal.ContratoCabeceraId,
                        HorarioCabeceraId = x.Personal.HorarioCabeceraId,
                    //    RegistroAsistenciaPoliticaId = x.Personal.RegistroAsistenciaPoliticaId
                    },
                    Persona = new PersonaQueryView {
                    //    Id = x.Personal.Persona.Id,
                    //    Nombres = x.Personal.Persona.Nombres,
                    //    ApellidoPaterno = x.Personal.Persona.ApellidoPaterno,
                    //    ApellidoMaterno = x.Personal.Persona.ApellidoMaterno,
                    //    FechaNacimiento = x.Personal.Persona.FechaNacimiento,
                        DocumentoIdentidad = x.Personal.Persona.DocumentoIdentidad,
                        NombreCompleto = $"{x.Personal.Persona.ApellidoPaterno} {x.Personal.Persona.ApellidoMaterno}, {x.Personal.Persona.Nombres}",
                        //    Correo = x.Personal.Persona.Correo,
                        //    Celular = x.Personal.Persona.Celular,
                        //    Direccion = x.Personal.Persona.Direccion
                    },
                    OrdenTrabajo = new OrdenTrabajoQueryView {
                        Id = x.RegistroAsistenciaOrdenTrabajo.OrdenTrabajoCabecera.Id,
                        Nombre = x.RegistroAsistenciaOrdenTrabajo.OrdenTrabajoCabecera.Nombre,
                        Descripcion = x.RegistroAsistenciaOrdenTrabajo.OrdenTrabajoCabecera.Descripcion,
                        //FechaInicio = x.RegistroAsistenciaOrdenTrabajo.OrdenTrabajoCabecera.FechaInicio,
                        //FechaCompromiso = x.RegistroAsistenciaOrdenTrabajo.OrdenTrabajoCabecera.FechaCompromiso,
                        //FechaFin = x.RegistroAsistenciaOrdenTrabajo.OrdenTrabajoCabecera.FechaFin
                    },
                    OrdenServicio = new OrdenServicioQueryView
                    {
                        Id = x.RegistroAsistenciaOrdenTrabajo.OrdenTrabajoCabecera.OrdenServicioCabecera.Id,
                        //OrdenServicioTipoId = x.RegistroAsistenciaOrdenTrabajo.OrdenTrabajoCabecera.OrdenServicioCabecera.OrdenServicioTipoId,
                        CodigoOrdenInterna = x.RegistroAsistenciaOrdenTrabajo.OrdenTrabajoCabecera.OrdenServicioCabecera.CodigoOrdenInterna,
                        CodigoReferencial = x.RegistroAsistenciaOrdenTrabajo.OrdenTrabajoCabecera.OrdenServicioCabecera.CodigoReferencial,
                        Descripcion = x.RegistroAsistenciaOrdenTrabajo.OrdenTrabajoCabecera.OrdenServicioCabecera.Descripcion,
                        //FechaInicial = x.RegistroAsistenciaOrdenTrabajo.OrdenTrabajoCabecera.OrdenServicioCabecera.FechaInicial,
                        //FechaFinal = x.RegistroAsistenciaOrdenTrabajo.OrdenTrabajoCabecera.OrdenServicioCabecera.FechaFinal
                    },
                    Adjuntos = x.RegistroAsistenciaAdjuntos.Select(a => new AdjuntosQueryView
                    {
                        Id = a.Id,
                        AdjuntoId = a.AdjuntoId,
                        Tipo = a.Tipo,
                        AdjuntoUrl = $"{ConfigVariables.GeneralService}/Adjunto/download/{a.AdjuntoId}"
                    }).ToList()
                });
            return resultado;
        }

        public async Task<RegistroAsistencia> ObtenerPorId(long id) =>
            await GetByIdAsync(id) ?? throw new RegistroNoEncontradoException<RegistroAsistencia>(id.ToString());
        public Task<List<RegistroAsistencia>> ObtenerTodos() => GetAllAsync();

        public Task<List<RegistroAsistencia>> BuscarPorRangoFecha(int personalId, DateTime fechaInicio, DateTime fechaFin)
        {
            if (fechaFin < fechaInicio)
            {
                (fechaInicio, fechaFin) = (fechaFin, fechaInicio);
            }

            return Query()
                .Where(x => x.PersonalId == personalId && x.Fecha >= fechaInicio && x.Fecha <= fechaFin)
                .OrderBy(x => x.Fecha)
                .ToListAsync();
        }
        public Task<RegistroAsistencia> BuscarPorRangoFecha(int personalId, DateTime fechaInicio, DateTime fechaFin, HorarioDetalleEventoTipoEnum tipoMarca)
        {
            if (fechaFin < fechaInicio)
                (fechaInicio, fechaFin) = (fechaFin, fechaInicio);

            return Query()
                .Where(x => x.PersonalId == personalId && x.Fecha >= fechaInicio && x.Fecha <= fechaFin && x.TipoEvento == tipoMarca)
                .OrderByDescending(x => x.Fecha)
                .FirstOrDefaultAsync();
        }
        public Task<List<Personal>> BuscarPersonalAsistenciasRangoFechas(DateTime fechaInicio, DateTime fechaFin)
        {
            if (fechaFin < fechaInicio)
                (fechaInicio, fechaFin) = (fechaFin, fechaInicio);

            return context.Personales
                .AsNoTracking()
                .Include(x => x.Persona)
                .Include(x => x.RegistroAsistencias.Where(x => x.Fecha >= fechaInicio && x.Fecha <= fechaFin))
                    .ThenInclude(x => x.RegistroAsistenciaOrdenTrabajo)
                        .ThenInclude(x => x.OrdenTrabajoCabecera)
                        .ThenInclude(x => x.OrdenServicioCabecera)
                .ToListAsync();
        }
        public Task<List<RegistroAsistencia>> ObtenerPorIdUsuarioYRangoFecha(string UsuarioId, DateTime fechaInicio, DateTime fechaFin)
        {
            if (fechaFin < fechaInicio)
                (fechaInicio, fechaFin) = (fechaFin, fechaInicio);

            return Query()
                .Where(x => x.Personal.UsuarioId == UsuarioId)
                .Where(x => x.Fecha >= fechaInicio && x.Fecha <= fechaFin)
                .OrderByDescending(x => x.Fecha)
                .ToListAsync();
        }
        public Task<List<RegistroAsistenciaQueryView>> BuscarPorRangoFecha(DateTime fechaInicio, DateTime fechaFin)
        {
            if (fechaFin < fechaInicio)
                (fechaInicio, fechaFin) = (fechaFin, fechaInicio);

            return this.QueryOt()
                .Where(x => x.Fecha >= fechaInicio && x.Fecha <= fechaFin)
                .ToListAsync();
        }
        public async Task<RegistroAsistencia> BuscarUltimaMarca(int personalId)
        {
            return await Query()
                .Where(x => x.PersonalId == personalId)
                .OrderByDescending(x => x.Fecha)
                .FirstOrDefaultAsync();
        }

        public async Task<RegistroAsistencia> BuscarUltimaMarca(int personalId, DateTime fechaDesde)
        {
            return await Query()
                .Where(x => x.PersonalId == personalId && (x.TipoEvento == HorarioDetalleEventoTipoEnum.Entrada || x.TipoEvento == HorarioDetalleEventoTipoEnum.Salida) && x.Fecha >= fechaDesde)
                .OrderByDescending(x => x.Fecha)
                .FirstOrDefaultAsync();
        }

        public async Task<RegistroAsistencia> BuscarUltimaMarca(int personalId, DateTime fechaDesde, DateTime fechaHasta)
        {
            return await Query()
                .Where(x => x.PersonalId == personalId && (x.TipoEvento == HorarioDetalleEventoTipoEnum.Entrada || x.TipoEvento == HorarioDetalleEventoTipoEnum.Salida) && x.Fecha >= fechaDesde && x.Fecha <= fechaHasta)
                .OrderByDescending(x => x.Fecha)
                .FirstOrDefaultAsync();
        }

        public async Task<RegistroAsistencia> Agregar(RegistroAsistencia entry)
        {
            var registro = await AddAsync(entry);
            return await ObtenerPorId(registro.Id);
        }

        public async Task<RegistroAsistencia> Modificar(RegistroAsistencia entry)
        {
            await UpdateAsync(entry);
            return await ObtenerPorId(entry.Id);
        }

        public Task Eliminar(long id) => DeleteAsync(id);
        public async Task<LoadResult> ObtenerFilter(DataSourceLoadOptionsBase options, CancellationToken cancellationToken) => await DataSourceLoader.LoadAsync(Query(), options, cancellationToken);
        public async Task<LoadResult> ObtenerFilterActivos(DataSourceLoadOptionsBase options, CancellationToken cancellationToken) => await DataSourceLoader.LoadAsync(Query(), options, cancellationToken);
    }
}
