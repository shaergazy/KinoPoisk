using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Data.Repositories.RepositoryInterfaces;


public interface IGenericRepository<TEntity, TKey>
{
    public IQueryable<TEntity> GetAll();

    public Task<TEntity> GetByIdAsync(TKey key);

    public Task<TEntity> AddAsync(TEntity model);

    public Task DeleteByIdAsync(TKey id);

    public Task Remove(TEntity model);

    public Task UpdateAsync(TEntity model);

    public Task<int> SaveChangesAsync();

    IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression = null);

    Task AddRangeAsync(ICollection<TEntity> entities);

    bool Any(Expression<Func<TEntity, bool>> expression = null);

    IIncludableQueryable<TEntity, TProperty> Include<TProperty>(Expression<Func<TEntity, TProperty>> expression);

    public IQueryable<TEntity> AsNoTracking();

    public void AddRange(IEnumerable<TEntity> entities);

    /// <summary>
    /// Return TEntity
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression = null);
}