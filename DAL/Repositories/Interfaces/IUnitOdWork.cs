using DAL.Models;
using DAL.Models.Users;
using Data.Models;

namespace Data.Repositories.RepositoryInterfaces
{
    public interface IUnitOfWork <TEntity, TKey> : IDisposable
    {
        public IGenericRepository<Movie, Guid> Movies { get; }
        public IGenericRepository<TEntity, TKey> Repository { get; }
        public IGenericRepository<Genre, int> Genres { get; }
        public IGenericRepository<Country, int> Countries { get; }
        public IGenericRepository<Person, int> People { get; }
        public IGenericRepository<MoviePerson, int> MoviePerson { get; }
        public IGenericRepository<Comment, int> Comments { get; }
        public IGenericRepository<MovieRating, int> Ratings { get; }
        public IGenericRepository<MovieGenre, int> MovieGenres { get; }
        public IGenericRepository<User, Guid> Users { get; }
        Task SaveChangesAsync();
    }
}
