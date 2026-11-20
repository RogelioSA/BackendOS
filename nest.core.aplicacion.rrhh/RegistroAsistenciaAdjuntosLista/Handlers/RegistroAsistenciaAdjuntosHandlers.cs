using MediatR;
using nest.core.aplicacion.rrhh.RegistroAsistenciaAdjuntosLista.Commands;
using nest.core.dominio.RRHH.RegistroAsistenciaAdjuntosEntities;

namespace nest.core.aplicacion.rrhh.RegistroAsistenciaAdjuntosLista.Handlers;

public sealed class RegistroAsistenciaAdjuntosCrearHandler(IRegistroAsistenciaAdjuntosRepository repository) : IRequestHandler<RegistroAsistenciaAdjuntosCrearCommand, dominio.RRHH.RegistroAsistenciaAdjuntosEntities.RegistroAsistenciaAdjuntos>
{
    public Task<dominio.RRHH.RegistroAsistenciaAdjuntosEntities.RegistroAsistenciaAdjuntos> Handle(RegistroAsistenciaAdjuntosCrearCommand request, CancellationToken cancellationToken) =>
        repository.Agregar(new dominio.RRHH.RegistroAsistenciaAdjuntosEntities.RegistroAsistenciaAdjuntos { RegistroAsistenciaId = request.RegistroAsistenciaId, EmpresaId = request.EmpresaId, AdjuntoId = request.AdjuntoId, Tipo = request.Tipo });
}

public sealed class RegistroAsistenciaAdjuntosModificarHandler(IRegistroAsistenciaAdjuntosRepository repository) : IRequestHandler<RegistroAsistenciaAdjuntosModificarCommand, dominio.RRHH.RegistroAsistenciaAdjuntosEntities.RegistroAsistenciaAdjuntos>
{
    public Task<dominio.RRHH.RegistroAsistenciaAdjuntosEntities.RegistroAsistenciaAdjuntos> Handle(RegistroAsistenciaAdjuntosModificarCommand request, CancellationToken cancellationToken) =>
        repository.Modificar(new dominio.RRHH.RegistroAsistenciaAdjuntosEntities.RegistroAsistenciaAdjuntos { Id = request.Id, RegistroAsistenciaId = request.RegistroAsistenciaId, EmpresaId = request.EmpresaId, AdjuntoId = request.AdjuntoId, Tipo = request.Tipo });
}

public sealed class RegistroAsistenciaAdjuntosEliminarHandler(IRegistroAsistenciaAdjuntosRepository repository) : IRequestHandler<RegistroAsistenciaAdjuntosEliminarCommand, Unit>
{
    public async Task<Unit> Handle(RegistroAsistenciaAdjuntosEliminarCommand request, CancellationToken cancellationToken)
    {
        await repository.Eliminar(request.Id);
        return Unit.Value;
    }
}
