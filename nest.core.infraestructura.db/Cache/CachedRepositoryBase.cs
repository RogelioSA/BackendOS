using AutoMapper;
using Microsoft.EntityFrameworkCore;
using nest.core.dominio;
using nest.core.dominio.Cache;
using nest.core.infraestructura.db.DbContext;
using nest.core.infraestructura.db.Utils;
using nest.core.infrastructura.utils.Excepciones;

namespace nest.core.infraestructura.db.Cache
{
    public abstract class CachedRepositoryBase<TEntity, TCreateDto, TKey> : CrudRepositoryBase<TEntity, TCreateDto, TKey> where TEntity : class, IEntity<TKey>
    {
        protected readonly ICacheRepository cache;
        private readonly string cacheKey;
        protected virtual TimeSpan CacheDuration => TimeSpan.FromHours(12);

        protected CachedRepositoryBase(NestDbContext context, IMapper mapper, ICacheRepository cache): base(context, mapper)
        {
            this.cache = cache;
            cacheKey = typeof(TEntity).Name;
            if (typeof(ITenantEntity).IsAssignableFrom(typeof(TEntity)))
                cacheKey = $"{typeof(TEntity).Name}_{context.EmpresaId}";
            else
                cacheKey = $"{typeof(TEntity).Name}";
        }

        protected async Task<List<TEntity>> GetCachedListAsync()
        {
            var cached = await cache.GetAsync<List<TEntity>>(cacheKey);
            if (cached is not null)
                return cached;

            var data = await Query().ToListAsync();
            await cache.SetAsync(cacheKey, data, CacheDuration);
            return data;
        }

        protected override async Task<TEntity?> GetByIdAsync(TKey id) => (await GetCachedListAsync()).FirstOrDefault(e => e.Id!.Equals(id));
        protected override async Task<List<TEntity>> GetAllAsync() => await GetCachedListAsync();
        protected override async Task<TEntity> AddAsync(TCreateDto dto)
        {
            var entity = mapper.Map<TEntity>(dto);
            context.Set<TEntity>().Add(entity);
            await context.SaveChangesAsync();
            await context.Entry(entity).ReloadAsync();
            await InvalidateCacheAsync();
            return entity;
        }
        public override async Task<TEntity> UpdateAsync(TKey id, TCreateDto dto)
        {
            var entity = await context.Set<TEntity>().FindAsync(id)
                         ?? throw new RegistroNoEncontradoException<TEntity>(id!.ToString()!);

            mapper.Map(dto, entity);
            await context.SaveChangesAsync();
            await context.Entry(entity).ReloadAsync();
            await InvalidateCacheAsync();
            return entity;
        }
        public override async Task DeleteAsync(TKey id)
        {
            var entity = await context.Set<TEntity>().FindAsync(id)
                         ?? throw new RegistroNoEncontradoException<TEntity>(id!.ToString()!);

            context.Set<TEntity>().Remove(entity);
            await context.SaveChangesAsync();
        }

        protected virtual Task InvalidateCacheAsync() => cache.RemoveAsync(cacheKey);
    }
}
