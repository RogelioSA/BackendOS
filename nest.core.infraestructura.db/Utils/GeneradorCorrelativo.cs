using nest.core.dominio.Security.Audit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace nest.core.infraestructura
{
    public class GeneradorCorrelativo
    {
        private static int PRIMER_NUMERO_CONTADOR = 0;
        private static int OFFSET_CONTADOR = 1;
        public static T GetValue<T>(EntityEntry entry, Func<object> maxId = null)
        {
            string esquema = entry.Metadata.GetSchema() ?? "dbo";
            string tabla = entry.Metadata.GetTableName();
            DbContext ctx = entry.Context;

            var correl = ctx.ChangeTracker
                            .Entries<CorrelativoMaestro>()
                            .FirstOrDefault(e =>
                                   e.Entity.Schema == esquema &&
                                   e.Entity.Table == tabla)?
                            .Entity;
            if (correl == null)
            {
                correl = ctx.Set<CorrelativoMaestro>()
                    .FirstOrDefault(c => c.Schema == esquema && c.Table == tabla);
                if (correl == null)
                {
                    correl = new CorrelativoMaestro
                    {
                        Schema = esquema,
                        Table = tabla,
                        LastValue = maxId == null ? PRIMER_NUMERO_CONTADOR : (long)maxId()
                    };
                    correl = ctx.Set<CorrelativoMaestro>().Add(correl).Entity;
                }
            }
            correl.LastValue = correl.LastValue + OFFSET_CONTADOR;
            return (T)Convert.ChangeType(correl.LastValue, typeof(T));
        }
        public static async Task<T> GetValueAsync<T>(EntityEntry entry, Func<object> maxId = null, CancellationToken cancellationToken = default)
        {
            string esquema = entry.Metadata.GetSchema() ?? "dbo";
            string tabla = entry.Metadata.GetTableName();
            var ctx = entry.Context;
            var correl = ctx.ChangeTracker
                            .Entries<CorrelativoMaestro>()
                            .FirstOrDefault(e =>
                                   e.Entity.Schema == esquema &&
                                   e.Entity.Table == tabla)?
                            .Entity;
            if (correl == null)
            {
                correl = await ctx.Set<CorrelativoMaestro>()
                                  .FirstOrDefaultAsync(c => c.Schema == esquema && c.Table == tabla);
                if (correl == null)
                {
                    correl = new CorrelativoMaestro
                    {
                        Schema = esquema,
                        Table = tabla,
                        LastValue = maxId == null ? PRIMER_NUMERO_CONTADOR : (long)maxId()
                    };
                    correl = ctx.Set<CorrelativoMaestro>().Add(correl).Entity;
                }
            }
            correl.LastValue = correl.LastValue + OFFSET_CONTADOR;
            return (T)Convert.ChangeType(correl.LastValue, typeof(T));
        }
    }
}
