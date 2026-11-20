using AutoMapper;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.EntityFrameworkCore;
using nest.core.dominio.RRHH.RegistroAsistenciaAdjuntosEntities;
using nest.core.infraestructura.db.DbContext;
using nest.core.infraestructura.db.Utils;
using nest.core.infrastructura.utils.Excepciones;

namespace nest.core.infraestructura.rrhh
{
    public class RegistroAsistenciaAdjuntosRepository : CrudRepositoryBase<RegistroAsistenciaAdjuntos, long>, IRegistroAsistenciaAdjuntosRepository
    {
        public RegistroAsistenciaAdjuntosRepository(NestDbContext context, IMapper mapper) : base(context, mapper) { }

        protected override IQueryable<RegistroAsistenciaAdjuntos> Query() => context.RegistroAsistenciaAdjuntos
            .AsNoTracking()
            .Include(x => x.RegistroAsistencia)
            .Include(x => x.Adjunto);

        public async Task<RegistroAsistenciaAdjuntos> ObtenerPorId(long id) =>
            await GetByIdAsync(id) ?? throw new RegistroNoEncontradoException<RegistroAsistenciaAdjuntos>(id.ToString());
        public Task<List<RegistroAsistenciaAdjuntos>> ObtenerTodos() => GetAllAsync();
        public async Task<RegistroAsistenciaAdjuntos> Agregar(RegistroAsistenciaAdjuntos entry)
        {
            var registro = await AddAsync(entry);
            return await ObtenerPorId(registro.Id);
        }
        public async Task<RegistroAsistenciaAdjuntos> Modificar(RegistroAsistenciaAdjuntos entry)
        {
            await UpdateAsync(entry);
            return await ObtenerPorId(entry.Id);
        }
        public Task Eliminar(long id) => DeleteAsync(id);
        public Task<LoadResult> ObtenerFilter(DataSourceLoadOptionsBase options, CancellationToken cancellationToken) => DataSourceLoader.LoadAsync(Query(), options, cancellationToken);
        public Task<LoadResult> ObtenerFilterActivos(DataSourceLoadOptionsBase options, CancellationToken cancellationToken) => DataSourceLoader.LoadAsync(Query(), options, cancellationToken);
    }
}
