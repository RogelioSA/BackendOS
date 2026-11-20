using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using MediatR;
using nest.core.dominio.RRHH.RegistroAsistenciaAdjuntosEntities;

namespace nest.core.aplicacion.rrhh.RegistroAsistenciaAdjuntosLista.Queries;

public record RegistroAsistenciaAdjuntosObtenerPorIdQuery(long Id) : IRequest<dominio.RRHH.RegistroAsistenciaAdjuntosEntities.RegistroAsistenciaAdjuntos>;
public record RegistroAsistenciaAdjuntosObtenerTodosQuery() : IRequest<List<dominio.RRHH.RegistroAsistenciaAdjuntosEntities.RegistroAsistenciaAdjuntos>>;
public record RegistroAsistenciaAdjuntosObtenerFiltroQuery(DataSourceLoadOptionsBase Options) : IRequest<LoadResult>;

public sealed class RegistroAsistenciaAdjuntosObtenerPorIdHandler(IRegistroAsistenciaAdjuntosRepository repository) : IRequestHandler<RegistroAsistenciaAdjuntosObtenerPorIdQuery, dominio.RRHH.RegistroAsistenciaAdjuntosEntities.RegistroAsistenciaAdjuntos>
{
    public Task<dominio.RRHH.RegistroAsistenciaAdjuntosEntities.RegistroAsistenciaAdjuntos> Handle(RegistroAsistenciaAdjuntosObtenerPorIdQuery request, CancellationToken cancellationToken) => repository.ObtenerPorId(request.Id);
}
public sealed class RegistroAsistenciaAdjuntosObtenerTodosHandler(IRegistroAsistenciaAdjuntosRepository repository) : IRequestHandler<RegistroAsistenciaAdjuntosObtenerTodosQuery, List<dominio.RRHH.RegistroAsistenciaAdjuntosEntities.RegistroAsistenciaAdjuntos>>
{
    public Task<List<dominio.RRHH.RegistroAsistenciaAdjuntosEntities.RegistroAsistenciaAdjuntos>> Handle(RegistroAsistenciaAdjuntosObtenerTodosQuery request, CancellationToken cancellationToken) => repository.ObtenerTodos();
}
public sealed class RegistroAsistenciaAdjuntosObtenerFiltroHandler(IRegistroAsistenciaAdjuntosRepository repository) : IRequestHandler<RegistroAsistenciaAdjuntosObtenerFiltroQuery, LoadResult>
{
    public Task<LoadResult> Handle(RegistroAsistenciaAdjuntosObtenerFiltroQuery request, CancellationToken cancellationToken) => repository.ObtenerFilter(request.Options, cancellationToken);
}
