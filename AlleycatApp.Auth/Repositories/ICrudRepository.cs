namespace AlleycatApp.Auth.Repositories
{
    public interface ICrudRepository<TEntity, in TId> where TEntity : class
    {
        IQueryable<TEntity> Entities { get; }

        Task<TEntity?> FindByIdAsync(TId id);
        Task<TEntity> AddAsync(TEntity entity);
        Task UpdateAsync(TId id, TEntity entity);
        Task DeleteAsync(TId id); 
    }
}
