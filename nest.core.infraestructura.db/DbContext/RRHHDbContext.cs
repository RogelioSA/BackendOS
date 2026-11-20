using Microsoft.EntityFrameworkCore;
using nest.core.dominio.General.SexoEntities;
using nest.core.dominio.RRHH.CargoEntities;
using nest.core.dominio.RRHH.GrupoTrabajoEntities;
using nest.core.dominio.RRHH.GrupoTrabajoPersonaEntities;
using nest.core.dominio.RRHH.HorarioCabeceraEntities;
using nest.core.dominio.RRHH.HorarioDetalleEntities;
using nest.core.dominio.RRHH.HorarioDetalleEventoEntities;
using nest.core.dominio.RRHH.PersonalEntities;
using nest.core.dominio.RRHH.PersonalCargoExternoEntities;
using nest.core.dominio.RRHH.PersonalEstadoEntities;
using nest.core.dominio.RRHH.RegistroAsistenciaAdjuntoEntities;
using nest.core.dominio.RRHH.RegistroAsistenciaAdjuntosEntities;
using nest.core.dominio.RRHH.RegistroAsistenciaEntities;
using nest.core.dominio.RRHH.RegistroAsistenciaOrdenTrabajoEntities;
using nest.core.dominio.RRHH.RegistroAsistenciaPoliticaEntities;
using nest.core.dominio.RRHH.TerminalBiometricoEntities;

namespace nest.core.infraestructura.db.DbContext
{
    public partial class NestDbContext
    {
        public DbSet<Cargo> Cargos { get; set; }
        public DbSet<GrupoTrabajo> GrupoTrabajo { get; set; }
        public DbSet<GrupoTrabajoPersona> GrupoTrabajoPersona { get; set; }
        public DbSet<HorarioCabecera> HorarioCabeceras { get; set; }
        public DbSet<HorarioDetalle> HorarioDetalles { get; set; }
        public DbSet<HorarioDetalleEvento> HorarioDetalleEventos { get; set; }
        public DbSet<Personal> Personales { get; set; }
        public DbSet<PersonalCargoExterno> PersonalCargoExterno { get; set; }
        public DbSet<PersonalEstado> PersonalEstado { get; set; }
        public DbSet<RegistroAsistencia> RegistroAsistencia { get; set; }
        public DbSet<RegistroAsistenciaPolitica> RegistroAsistenciaPolitica { get; set; }
        public DbSet<RegistroAsistenciaOrdenTrabajo> RegistroAsistenciaOrdenTrabajo { get; set; }
        public DbSet<RegistroAsistenciaAdjunto> RegistroAsistenciaAdjunto { get; set; }
        public DbSet<RegistroAsistenciaAdjuntos> RegistroAsistenciaAdjuntos { get; set; }
        public DbSet<TerminalBiometrico> TerminalBiometrico { get; set; }
        public void OnModelCreatingRRHH(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GrupoTrabajo>().HasQueryFilter(x => x.EmpresaId == this.EmpresaId);
            modelBuilder.Entity<GrupoTrabajoPersona>().HasQueryFilter(x => x.EmpresaId == this.EmpresaId);
            modelBuilder.Entity<HorarioCabecera>().HasQueryFilter(x => x.EmpresaId == this.EmpresaId);
            modelBuilder.Entity<HorarioDetalle>().HasQueryFilter(x => x.EmpresaId == this.EmpresaId);
            modelBuilder.Entity<HorarioDetalleEvento>().HasQueryFilter(x => x.EmpresaId == this.EmpresaId);
            modelBuilder.Entity<Personal>().HasQueryFilter(x => x.EmpresaId == this.EmpresaId);
            modelBuilder.Entity<PersonalCargoExterno>().HasQueryFilter(x => x.EmpresaId == this.EmpresaId);
            modelBuilder.Entity<RegistroAsistencia>().HasQueryFilter(x => x.EmpresaId == this.EmpresaId);
            modelBuilder.Entity<RegistroAsistenciaPolitica>().HasQueryFilter(x => x.EmpresaId == this.EmpresaId);
            modelBuilder.Entity<RegistroAsistenciaOrdenTrabajo>().HasQueryFilter(x => x.EmpresaId == this.EmpresaId);
            modelBuilder.Entity<RegistroAsistenciaAdjunto>().HasQueryFilter(x => x.EmpresaId == this.EmpresaId);
            modelBuilder.Entity<RegistroAsistenciaAdjuntos>().HasQueryFilter(x => x.EmpresaId == this.EmpresaId);
            modelBuilder.Entity<TerminalBiometrico>().HasQueryFilter(x => x.EmpresaId == this.EmpresaId);
        }
    }
}
