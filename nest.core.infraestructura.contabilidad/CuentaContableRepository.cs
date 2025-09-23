using AutoMapper;
using Microsoft.EntityFrameworkCore;
using nest.core.dominio.Cache;
using nest.core.dominio.Contabilidad.CuentaContableEntities;
using nest.core.infraestructura.db.Cache;
using nest.core.infraestructura.db.DbContext;

namespace nest.core.infraestructura.contabilidad
{
    public class CuentaContableRepository : CachedRepositoryBase<CuentaContable, CuentaContableCrearDto, long>, ICuentaContableRepository
    {
        public CuentaContableRepository(NestDbContext context, IMapper mapper, ICacheRepository cache) : base(context, mapper, cache) { }
        protected override IQueryable<CuentaContable> Query() => context.Set<CuentaContable>()
            .AsNoTracking()
            .Include(x => x.CuentaContableTipo)
            .Include(x => x.Children);
        public Task<CuentaContable> ObtenerPorId(int id) => GetByIdAsync(id);
        public Task<List<CuentaContable>> ObtenerTodos() => GetAllAsync();
        public async Task<List<CuentaContable>> ObtenerActivos() => (await GetAllAsync()).Where(x => x.Activo).ToList();
        public Task<CuentaContable> Agregar(CuentaContableCrearDto entry) => AddAsync(entry);
        public Task<CuentaContable> Modificar(int id, CuentaContableCrearDto entry) => UpdateAsync(id, entry);
        public Task Eliminar(int id) => DeleteAsync(id);
    }
}
