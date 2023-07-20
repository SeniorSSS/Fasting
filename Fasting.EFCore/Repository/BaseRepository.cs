using AutoMapper;
using Fasting.Exceptions.Models;

namespace Fasting.EFCore.Repository
{
    public abstract class BaseRepository<TModel, TEntity, TContext> : IBaseRepository<TModel> where TEntity : class where TModel : class where TContext : BaseDbContext
    {
        protected readonly TContext DbContext;
        protected readonly IMapper _mapper;

        protected BaseRepository(TContext context, IMapper mapper)
        {
            DbContext = context;
            _mapper = mapper;
        }

        public async virtual Task<TModel> Create(TModel model)
        {
            var entity = _mapper.Map<TEntity>(model);
            await DbContext.Set<TEntity>().AddAsync(entity);
            await DbContext.SaveChangesAsync();

            return _mapper.Map(entity, model);
        }

        public async virtual Task<TModel> Update(TModel model)
        {
            var entity = await GetEntity(model);
            if (entity == null)
            {
                throw new BadRequestException("entity-not-found", $"Attempt to update enitity {typeof(TEntity)?.Name} failed, entity was not found");
            }
            return await Update(model, entity);
        }

        public async virtual Task Delete(TModel model)
        {
            var entity = await GetEntity(model);
            DbContext.Remove(entity);
            await DbContext.SaveChangesAsync();
        }

        private async Task<TModel> Update(TModel model, TEntity entity)
        {
            _mapper.Map(model, entity);
            await DbContext.SaveChangesAsync();

            return _mapper.Map(entity, model);
        }

        protected abstract Task<TEntity> GetEntity(TModel model);
    }
}
