using DAL.Models;

namespace Data.Repositories.RepositoryInterfaces
{
    public interface IUnitOfWork <TEntity, TKey>: IDisposable
        where TEntity : class
        where TKey : class
    {
        public IMovieRepository Movies { get; }
        IBaseRepository<TEntity, TKey> repository { get; }
        //public IBaseRepository<Genre, int> Genres { get; }
        //public IBaseRepository<Country, int> Countries { get; }
        //public IBaseRepository<Person, int> People { get; }
        Task SaveChangesAsync();
    }
}
