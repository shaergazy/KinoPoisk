using DAL;
using Data.Repositories.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class BaseRepository<T, K> : IBaseRepository <T> 
    where T : class
    {
        protected readonly DbSet<T> _dbSet;
        protected readonly AppDbContext _context;
        public BaseRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
            
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public async Task<T> GetById(T id)
        {
           return await _dbSet.FindAsync(id);
        }
            
        public async Task<T> Create(T model, bool commitTransaction = true)
        {
            var entity = (await _context.AddAsync(model)).Entity;
            if(commitTransaction) 
                await SaveChangesAsync();
            return entity;
        }

        public async Task<T> Delete(T model)
        {
           var entity =  _context.Remove(model).Entity;
           await SaveChangesAsync();
           return entity;
        }

        public async Task<T> Update(T model)
        {
            var entity = _dbSet.Update(model).Entity;
            await SaveChangesAsync();
            return entity;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
