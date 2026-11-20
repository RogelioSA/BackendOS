using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;

namespace nest.core.dominio.RRHH.RegistroAsistenciaAdjuntosEntities
{
    public interface IRegistroAsistenciaAdjuntosRepository
    {
        Task<RegistroAsistenciaAdjuntos> ObtenerPorId(long id);
        Task<List<RegistroAsistenciaAdjuntos>> ObtenerTodos();
        Task<RegistroAsistenciaAdjuntos> Agregar(RegistroAsistenciaAdjuntos entidad);
        Task<RegistroAsistenciaAdjuntos> Modificar(RegistroAsistenciaAdjuntos entidad);
        Task Eliminar(long id);
        Task<LoadResult> ObtenerFilter(DataSourceLoadOptionsBase options, CancellationToken cancellationToken);
        Task<LoadResult> ObtenerFilterActivos(DataSourceLoadOptionsBase options, CancellationToken cancellationToken);
    }
}
