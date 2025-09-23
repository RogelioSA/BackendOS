using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using nest.core.dominio;

namespace nest.core.infraestructura.db.DbContext.Convention
{
    public class TenantGuardSaveChangesInterceptor : SaveChangesInterceptor
    {
        private static readonly string ErrorMsg =
            "No puedes realizar acciones a empresas diferentes a la de tu sesion";

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            Validate((NestDbContext)eventData.Context);
            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            Validate((NestDbContext)eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void Validate(NestDbContext ctx)
        {
            if (ctx == null) return;
            var empresaId = ctx.EmpresaId;

            IEnumerable<EntityEntry> entries = ctx.ChangeTracker.Entries()
                .Where(e => e.Entity is ITenantEntity &&
                           (e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted));

            foreach (var e in entries)
            {
                var ten = (ITenantEntity)e.Entity;
                if (ten.EmpresaId != empresaId.Value)
                    throw new InvalidOperationException(ErrorMsg);
            }
        }
    }
}
