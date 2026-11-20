using MediatR;
using nest.core.dominio.RRHH.RegistroAsistenciaAdjuntosEntities;

namespace nest.core.aplicacion.rrhh.RegistroAsistenciaAdjuntosLista.Commands;

public record RegistroAsistenciaAdjuntosModificarCommand(long Id, long RegistroAsistenciaId, int EmpresaId, long AdjuntoId, RegistroAsistenciaAdjuntoTipoEnum Tipo) : IRequest<dominio.RRHH.RegistroAsistenciaAdjuntosEntities.RegistroAsistenciaAdjuntos>;
