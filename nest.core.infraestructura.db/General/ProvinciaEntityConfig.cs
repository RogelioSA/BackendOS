﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using nest.core.dominio.General;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace nest.core.infraestructura.db.General
{
    public class ProvinciaEntityConfig : IEntityTypeConfiguration<Provincia>
    {
        public void Configure(EntityTypeBuilder<Provincia> builder)
        {
            builder.ToTable("provincia", "dbo");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .ValueGeneratedNever()
                .HasValueGenerator<ProvinciaValueGenerator>();
            builder.HasMany(p => p.Distritos)
                .WithOne(d => d.Provincia)
                .HasForeignKey(d => d.ProvinciaId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasData(GetData());
        }

        private List<Provincia> GetData()
        {
            return new List<Provincia>
            {
                new Provincia { Id = 1, Nombre = "Arequipa", DepartamentoId = 4 },
                new Provincia { Id = 2, Nombre = "Camaná", DepartamentoId = 4 },
                new Provincia { Id = 3, Nombre = "Caravelí", DepartamentoId = 4 },
                new Provincia { Id = 4, Nombre = "Castilla", DepartamentoId = 4 },
                new Provincia { Id = 5, Nombre = "Caylloma", DepartamentoId = 4 },
                new Provincia { Id = 6, Nombre = "Condesuyos", DepartamentoId = 4 },
                new Provincia { Id = 7, Nombre = "Islay", DepartamentoId = 4 },
                new Provincia { Id = 8, Nombre = "La Unión", DepartamentoId = 4 }
            };
        }
    }

    public class ProvinciaValueGenerator : ValueGenerator<int>
    {
        public override bool GeneratesTemporaryValues => false;
        public override int Next(EntityEntry entry) =>
            (entry.Context.Set<Provincia>().Max(g => (int?)g.Id) ?? 0) + 1;
        public override async ValueTask<int> NextAsync(EntityEntry entry, CancellationToken cancellationToken = default) =>
            (await entry.Context.Set<Provincia>().MaxAsync(g => (byte?)g.Id, cancellationToken) ?? 0) + 1;
    }
}
