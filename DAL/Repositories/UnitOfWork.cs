using DAL;
using DAL.Models;
using Data.Models;
using Data.Repositories.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class UnitOfWork <TEntity, TKey>  : IUnitOfWork <TEntity, TKey>
        where TEntity : class
    {
        private readonly AppDbContext _appDbContext;
        public IGenericRepository<Genre, int> _genresRepository;
        public IGenericRepository<Country, int> _countriesRepository;
        public IGenericRepository<Person, int> _peopleRepository;
        public IGenericRepository<Movie, Guid> _movieRepository;
        public IGenericRepository<Comment, int> _commentRepository;
        public IGenericRepository<MoviePerson, int> _moviePersonRepository;
        public IGenericRepository<MovieRating, int> _ratingRepository;
        public IGenericRepository<MovieGenre, int> _movieGenreRepository;

        private IGenericRepository<TEntity, TKey> _repository;

        public UnitOfWork(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IGenericRepository<Genre, int> Genres => _genresRepository ??= new GenericRepository<Genre, int>(_appDbContext);
        public IGenericRepository<Country, int> Countries => _countriesRepository ??= new GenericRepository<Country, int>(_appDbContext);
        public IGenericRepository<Person, int> People => _peopleRepository ??= new GenericRepository<Person, int>(_appDbContext);


        public IGenericRepository<TEntity, TKey> Repository => _repository ??= new GenericRepository<TEntity, TKey>(_appDbContext);

        public IGenericRepository<Movie, Guid> Movies => _movieRepository ??= new GenericRepository<Movie, Guid>(_appDbContext);

        public IGenericRepository<MoviePerson, int> MoviePerson => _moviePersonRepository ??= new GenericRepository<MoviePerson, int>(_appDbContext);

        public IGenericRepository<Comment, int> Comments => _commentRepository ??= new GenericRepository<Comment, int>(_appDbContext);

        public IGenericRepository<MovieRating, int> Ratings => _ratingRepository ??= new GenericRepository<MovieRating, int>(_appDbContext);
        public IGenericRepository<MovieGenre, int> MovieGenres => _movieGenreRepository ??= new GenericRepository<MovieGenre, int>(_appDbContext);

        public async Task SaveChangesAsync()
        {
            await _appDbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _appDbContext.Dispose();
        }
    }
}
