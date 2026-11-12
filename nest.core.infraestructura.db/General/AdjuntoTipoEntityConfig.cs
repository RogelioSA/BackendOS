using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using nest.core.dominio.General.AdjuntoTipoEntities;

namespace nest.core.infraestructura.db.General
{
    public class AdjuntoTipoEntityConfig : IEntityTypeConfiguration<AdjuntoTipo>
    {
        public void Configure(EntityTypeBuilder<AdjuntoTipo> builder)
        {
            builder.ToTable("adjunto_tipo", "dbo");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .ValueGeneratedNever();
            builder.HasData(GetData());
        }

        private List<AdjuntoTipo> GetData()
        {
            return new List<AdjuntoTipo>
            {
                new AdjuntoTipo { Id = AdjuntoTipoEnum.Foto, Nombre = "Fotografia", NombreCorto = "Foto", Activo = true },
                new AdjuntoTipo { Id = AdjuntoTipoEnum.DocumentoIdentidad, Nombre = "Dni", NombreCorto = "Dni", Activo = true },
                new AdjuntoTipo { Id = AdjuntoTipoEnum.LicenciaConducir, Nombre = "Licencia de conducir", NombreCorto = "LDC", Activo = true },
                new AdjuntoTipo { Id = AdjuntoTipoEnum.Cv, Nombre = "Hoja de vida", NombreCorto = "CV", Activo = true },
                new AdjuntoTipo { Id = AdjuntoTipoEnum.Contrato, Nombre = "Contrato", NombreCorto = "Contrato", Activo = true },
                new AdjuntoTipo { Id = AdjuntoTipoEnum.Habilitacion, Nombre = "Habilitación", NombreCorto = "HAB", Activo = true },
                new AdjuntoTipo { Id = AdjuntoTipoEnum.FirmaDigital, Nombre = "Firma digital", NombreCorto = "FIR", Activo = true },
                new AdjuntoTipo { Id = AdjuntoTipoEnum.Otro, Nombre = "Otros", NombreCorto = "Otros", Activo = true }
            };
        }
    }
}
