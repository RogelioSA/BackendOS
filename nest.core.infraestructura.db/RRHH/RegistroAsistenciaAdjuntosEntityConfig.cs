using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using nest.core.dominio.RRHH.RegistroAsistenciaAdjuntosEntities;

namespace nest.core.infraestructura.db.RRHH
{
    public class RegistroAsistenciaAdjuntosEntityConfig : IEntityTypeConfiguration<RegistroAsistenciaAdjuntos>
    {
        public void Configure(EntityTypeBuilder<RegistroAsistenciaAdjuntos> builder)
        {
            builder.ToTable("registro_asistencia_adjuntos", "rrhh");
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.EmpresaId);
            builder.Property(x => x.Id)
                .ValueGeneratedNever()
                .HasValueGenerator<GenericValueGenerator<long>>();
            builder.HasOne(x => x.RegistroAsistencia)
                .WithMany(x => x.RegistroAsistenciaAdjuntos)
                .HasForeignKey(x => x.RegistroAsistenciaId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Adjunto)
                .WithMany()
                .HasForeignKey(x => x.AdjuntoId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
