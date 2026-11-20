using nest.core.dominio.RRHH.HorarioDetalleEventoEntities;
using nest.core.dominio.RRHH.PersonalEntities;
using nest.core.dominio.RRHH.RegistroAsistenciaAdjuntoEntities;
using nest.core.dominio.RRHH.RegistroAsistenciaAdjuntosEntities;
using nest.core.dominio.RRHH.RegistroAsistenciaOrdenTrabajoEntities;
using nest.core.dominio.RRHH.RegistroAsistenciaPoliticaEntities;
using nest.core.dominio.RRHH.TerminalBiometricoEntities;
using nest.core.dominio.Security.Audit;

namespace nest.core.dominio.RRHH.RegistroAsistenciaEntities
{
    public enum RegistroAsistenciaTipoRegularizacionId : byte
    {
        Manual = 0,
        Automatico = 1
    }
    public class RegistroAsistencia: IEntity<long>, ITenantEntity, IAuditable
    {
        public int EmpresaId { get; set; }
        public long Id { get; set; }
        public int PersonalId { get; set; }
        public DateTime Fecha { get; set; }
        public DateOnly FechaJornal { get; set; }
        public HorarioDetalleEventoTipoEnum TipoEvento { get; set; }
        public bool EsTardanza { get; set; }
        public int DiferenciaMinutos { get; set; }
        public decimal? Latitud { get; set; }
        public decimal? Longitud { get; set; }
        public long? HorarioDetalleEventoId { get; set; }
        public long? RegistroAsistenciaPoliticaId { get; set; }
        public int? TerminalBiometricoId { get; set; }
        public string? Observacion { get; set; }
        public TerminalBiometrico TerminalBiometrico { get; set; }
        public RegistroAsistenciaPolitica RegistroAsistenciaPolitica { get; set; }
        public Personal Personal { get; set; }
        public HorarioDetalleEvento HorarioDetalleEvento { get; set; }
        public RegistroAsistenciaOrdenTrabajo RegistroAsistenciaOrdenTrabajo { get; set; }
        public RegistroAsistenciaAdjunto RegistroAsistenciaAdjunto { get; set; }
        public List<RegistroAsistenciaAdjuntos> RegistroAsistenciaAdjuntos { get; set; }
    }
}
