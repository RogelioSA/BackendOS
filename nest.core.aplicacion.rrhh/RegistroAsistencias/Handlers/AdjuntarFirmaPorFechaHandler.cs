using MediatR;
using nest.core.aplicacion.rrhh.RegistroAsistencias.Commands;
using nest.core.dominio.RRHH.PersonalEntities;
using nest.core.dominio.RRHH.RegistroAsistenciaAdjuntosEntities;
using nest.core.dominio.RRHH.RegistroAsistenciaEntities;
using nest.core.dominio.Security.Tenant;
using nest.core.dominio.Transaccional;

namespace nest.core.aplicacion.rrhh.RegistroAsistencias.Handlers;

public sealed class AdjuntarFirmaPorFechaHandler(
    IRegistroAsistenciaRepository registroRepository,
    IRegistroAsistenciaAdjuntosRepository adjuntosRepository,
    IPersonalRepository personalRepository,
    IConnectionStringService connectionStringService,
    IUnitOfWork unitOfWork) : IRequestHandler<AdjuntarFirmaPorFechaCommand, int>
{
    public async Task<int> Handle(AdjuntarFirmaPorFechaCommand request, CancellationToken cancellationToken)
    {
        var personal = await personalRepository.ObtenerPorIdUsuario(connectionStringService.UserId)
            ?? throw new InvalidOperationException("El usuario autenticado no tiene un personal asociado.");
        var empresaId = connectionStringService.EmpresaId
            ?? throw new InvalidOperationException("Usuario no autenticado.");

        await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var cantidad = 0;
            foreach (var item in request.Items)
            {
                if (item.AdjuntoId <= 0)
                    throw new ArgumentException("El AdjuntoId debe ser mayor que cero.");

                var asistencias = await registroRepository.BuscarPorPersonalYFechaJornal(personal.Id, DateOnly.FromDateTime(item.Fecha));
                foreach (var asistencia in asistencias)
                {
                    await adjuntosRepository.Agregar(new dominio.RRHH.RegistroAsistenciaAdjuntosEntities.RegistroAsistenciaAdjuntos
                    {
                        EmpresaId = empresaId,
                        RegistroAsistenciaId = asistencia.Id,
                        AdjuntoId = item.AdjuntoId,
                        Tipo = RegistroAsistenciaAdjuntoTipoEnum.FirmaDigital
                    });
                    cantidad++;
                }
            }

            await unitOfWork.CommitAsync(cancellationToken);
            return cantidad;
        }
        catch
        {
            await unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
        finally
        {
            await unitOfWork.DisposeAsync();
        }
    }
}
