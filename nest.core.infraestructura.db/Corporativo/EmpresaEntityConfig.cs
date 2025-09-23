using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using nest.core.dominio.Corporativo.Empresa;
using nest.core.infraestructura.db.DbContext;

namespace nest.core.infraestructura.db.Corporativo
{
    public class EmpresaEntityConfig: IEntityTypeConfiguration<Empresa>
    {
        public void Configure(EntityTypeBuilder<Empresa> builder)
        {
            builder.ToTable("empresa", "organizacion");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .ValueGeneratedNever()
                .HasValueGenerator<EmpresaValueGenerator>();
            builder.Property(x => x.NombreCorto)
                .HasMaxLength(9)
                .IsRequired();
            builder.HasData(ObtenerInformacionInicial());
        }
        public List<Empresa> ObtenerInformacionInicial()
        {
            return new List<Empresa>()
            {
                new Empresa { Id = 1, Nombre = "Default", NombreCorto = "Def", Estado = true }
            };
        }
    }
    public class EmpresaValueGenerator : ValueGenerator<int>
    {
        public override bool GeneratesTemporaryValues => false;
        public override int Next(EntityEntry entry) => GeneradorCorrelativo.GetValue<int>(entry, object () => ((NestDbContext)entry.Context).Empresa.Max(x => x.Id));
        public override async ValueTask<int> NextAsync(EntityEntry entry, CancellationToken cancellationToken = default) => await GeneradorCorrelativo.GetValueAsync<int>(entry, object () => ((NestDbContext)entry.Context).Empresa.Max(x => x.Id), cancellationToken);
    }
}
