using AlleycatApp.Auth.Models;
using AlleycatApp.Auth.Models.Validation;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AlleycatApp.Auth.Repositories
{
    public abstract class CrudDbRepository<TEntity, TId>(DbContext context, IMapper mapper) : ICrudRepository<TEntity, TId> where TEntity : class, IModel<TId>
    {
        public abstract IQueryable<TEntity> Entities { get; }
        public abstract Task DeleteAsync(TId id);

        public virtual async Task<TEntity?> FindByIdAsync(TId id)
            => await Entities.SingleOrDefaultAsync(x => x.Id != null && x.Id.Equals(id));

        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            ModelValidator.Validate(entity);

            var entry = await context.AddAsync(entity);
            await context.SaveChangesAsync();

            return entry.Entity;
        }

        public virtual async Task UpdateAsync(TId id, TEntity entity)
        {
            var entityToEdit = await FindByIdStrictAsync(id);
            mapper.Map(entity, entityToEdit);
            ModelValidator.Validate(entity);
            await context.SaveChangesAsync();
        }

        protected async Task<TEntity> FindByIdStrictAsync(TId id) =>
            await Entities.SingleAsync(x => x.Id != null && x.Id.Equals(id));
    }
}
