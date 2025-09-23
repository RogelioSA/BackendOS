using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using nest.core.dominio.Security;
using nest.core.infraestructura.db.DbContext;
using nest.core.infraestructura.db.DbContext.Convention;
using nest.core.infraestructura.db.DbContext.Provider;

namespace nest.core.aplication.auth
{
    public static class DbContextSelector
    {
        public static void SelectProvider(WebApplicationBuilder builder, bool IsRun)
        {
            string connection = IsRun ? Environment.GetEnvironmentVariable("ENGINE") : MigrationService.MigrationConnection();
            Console.WriteLine($"Resolviendo proveedor de base de datos para migraciones: {connection}");
            Console.WriteLine($"Conexion actual: {connection}");
            switch (connection)
            {
                case "SqlServer":
                    builder.Services.AddDbContext<NestDbContext, DbContextSqlServer>((sp) => {
                        sp.AddInterceptors(new TenantGuardSaveChangesInterceptor());
                    });
                    builder.Services
                        .AddIdentity<ApplicationUser, ApplicationRole>()
                        .AddEntityFrameworkStores<DbContextSqlServer>();
                    builder.Services.AddHealthChecks()
                        .AddDbContextCheck<DbContextSqlServer>("Users check", customTestQuery: (db, token) => db.Users.AnyAsync(token));
                    break;
                case "Npgsql":
                    builder.Services.AddDbContext<NestDbContext, DbContextPsSql>((sp) => {
                        sp.AddInterceptors(new TenantGuardSaveChangesInterceptor());
                    });
                    builder.Services
                        .AddIdentity<ApplicationUser, ApplicationRole>()
                        .AddEntityFrameworkStores<DbContextPsSql>();
                    builder.Services.AddHealthChecks()
                        .AddDbContextCheck<DbContextPsSql>("Users check", customTestQuery: (db, token) => db.Users.AnyAsync(token));
                    break;
                case "MySql":
                    builder.Services.AddDbContext<NestDbContext, DbContextMySql>((sp) => {
                        sp.AddInterceptors(new TenantGuardSaveChangesInterceptor());
                    });
                    builder.Services
                        .AddIdentity<ApplicationUser, ApplicationRole>()
                        .AddEntityFrameworkStores<DbContextMySql>();
                    builder.Services.AddHealthChecks()
                        .AddDbContextCheck<DbContextMySql>("Users check", customTestQuery: (db, token) => db.Users.AnyAsync(token));
                    break;
                default: throw new Exception("Engine no soportado para migraciones");
            }
        }
    }
}
