using DAL.Entities;

namespace Data.Repositories.RepositoryInterfaces
{
    public interface IUnitOfWork : IDisposable
    {
        public IMovieRepository Movies { get; }
        public IBaseRepository<Genre, int> Genres { get; }
        public IBaseRepository<Country, int> Countries { get; }
        public IBaseRepository<Person, int> People { get; }
        Task SaveChangesAsync();
    }
}
