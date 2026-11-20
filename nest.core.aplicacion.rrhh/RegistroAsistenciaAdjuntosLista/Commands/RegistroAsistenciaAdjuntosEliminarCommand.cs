using MediatR;

namespace nest.core.aplicacion.rrhh.RegistroAsistenciaAdjuntosLista.Commands;

public record RegistroAsistenciaAdjuntosEliminarCommand(long Id) : IRequest<Unit>;
