using DAL.Models;

namespace Data.Repositories.RepositoryInterfaces
{
    public interface IUnitOfWork <TEntity, TKey> : IDisposable
    {
        IGenericRepository<TEntity, TKey> Repository { get; }
        public IMovieRepository Movies { get; }
        //IGenericRepository<TEntity, TKey> repository { get; }
        //public IGenericRepository<Genre, int> Genres { get; }
        //public IGenericRepository<Country, int> Countries { get; }
        //public IGenericRepository<Person, int> People { get; }
        Task SaveChangesAsync();
    }
}
