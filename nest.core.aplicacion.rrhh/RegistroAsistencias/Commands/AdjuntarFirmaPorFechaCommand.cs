using MediatR;

namespace nest.core.aplicacion.rrhh.RegistroAsistencias.Commands;

public record AdjuntarFirmaPorFechaItem(DateTime Fecha, long AdjuntoId);

public record AdjuntarFirmaPorFechaCommand(List<AdjuntarFirmaPorFechaItem> Items) : IRequest<int>;
