using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using nest.core.dominio.Security.UsuarioEmpresa;
using nest.core.infraestructura.db.DbContext;

namespace nest.core.infraestructura.db.Security
{
    public class UsuarioEmpresaEntityConfig : IEntityTypeConfiguration<UsuarioEmpresa>
    {
        public void Configure(EntityTypeBuilder<UsuarioEmpresa> builder)
        {
            builder.ToTable("usuario_empresa", "security");
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.EmpresaId);
            builder.HasIndex(x => new { x.EmpresaId, x.UsuarioId })
                .IsUnique();
            builder.Property(x => x.Id)
                .ValueGeneratedNever()
                .HasValueGenerator<UsuarioEmpresaGenerator>();
            builder.HasOne(x => x.Empresa)
                .WithMany()
                .HasForeignKey(x => x.EmpresaId);
            builder.HasOne(x => x.Usuario)
                .WithMany()
                .HasForeignKey(x => x.UsuarioId);
            builder.HasData(GetData());
        }

        private List<UsuarioEmpresa> GetData()
        {
            return new List<UsuarioEmpresa> {
                new UsuarioEmpresa { Id = 1, EmpresaId = 1, UsuarioId = "1", Actual = true },
                new UsuarioEmpresa { Id = 2, EmpresaId = 1, UsuarioId = "2", Actual = true }
            };
        }
    }
    public class UsuarioEmpresaGenerator : ValueGenerator<long>
    {
        public override bool GeneratesTemporaryValues => false;
        public override long Next(EntityEntry entry) => GeneradorCorrelativo.GetValue<long>(entry, object () => ((NestDbContext)entry.Context).UsuarioEmpresa.Max(x => x.Id));
        public override async ValueTask<long> NextAsync(EntityEntry entry, CancellationToken cancellationToken = default) => await GeneradorCorrelativo.GetValueAsync<long>(entry, object () => ((NestDbContext)entry.Context).UsuarioEmpresa.Max(x => x.Id), cancellationToken);
    }
}
