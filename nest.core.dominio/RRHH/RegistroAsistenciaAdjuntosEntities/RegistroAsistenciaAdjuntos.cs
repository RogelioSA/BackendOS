using nest.core.dominio.General.AdjuntoEntities;
using nest.core.dominio.RRHH.RegistroAsistenciaEntities;
using nest.core.dominio.Security.Audit;

namespace nest.core.dominio.RRHH.RegistroAsistenciaAdjuntosEntities
{
    public class RegistroAsistenciaAdjuntos : IEntity<long>, IAuditable, ITenantEntity
    {
        public long Id { get; set; }
        public int EmpresaId { get; set; }
        public long RegistroAsistenciaId { get; set; }
        public long AdjuntoId { get; set; }
        public RegistroAsistenciaAdjuntoTipoEnum Tipo { get; set; }
        public RegistroAsistencia RegistroAsistencia { get; set; }
        public Adjunto Adjunto { get; set; }
    }
}
