using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using nest.core.dominio.RRHH.HorarioDetalleEventoEntities;
using nest.core.dominio.RRHH.PersonalEntities;

namespace nest.core.dominio.RRHH.RegistroAsistenciaEntities
{
    public interface IRegistroAsistenciaRepository
    {
        Task<RegistroAsistencia> ObtenerPorId(long id);
        Task<List<RegistroAsistencia>> ObtenerTodos();
        Task<List<RegistroAsistencia>> BuscarPorRangoFecha(int personalId, DateTime fechaInicio, DateTime fechaFin);
        Task<List<RegistroAsistencia>> BuscarPorPersonalYFechaJornal(int personalId, DateOnly fechaJornal);
        Task<RegistroAsistencia> BuscarPorRangoFecha(int personalId, DateTime fechaInicio, DateTime fechaFin, HorarioDetalleEventoTipoEnum tipoMarca);
        Task<List<RegistroAsistenciaQueryView>> BuscarPorRangoFecha(DateTime fechaInicio, DateTime fechaFin);
        Task<List<RegistroAsistencia>> ObtenerPorIdUsuarioYRangoFecha(string UsuarioId, DateTime fechaInicio, DateTime fechaFin);
        Task<List<Personal>> BuscarPersonalAsistenciasRangoFechas(DateTime fechaInicio, DateTime fechaFin);
        Task<RegistroAsistencia> BuscarUltimaMarca(int personalId);
        Task<RegistroAsistencia> BuscarUltimaMarca(int personalId, DateTime fechaDesde);
        Task<RegistroAsistencia> BuscarUltimaMarca(int personalId, DateTime fechaDesde, DateTime fechaHasta);
        Task<RegistroAsistencia> Agregar(RegistroAsistencia entidad);
        Task<RegistroAsistencia> Modificar(RegistroAsistencia entidad);
        Task Eliminar(long id);
        Task<LoadResult> ObtenerFilter(DataSourceLoadOptionsBase options, CancellationToken cancellationToken);
        Task<LoadResult> ObtenerFilterActivos(DataSourceLoadOptionsBase options, CancellationToken cancellationToken);
    }
}
