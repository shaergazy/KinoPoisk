using DAL;
using DAL.Models;
using Data.Repositories.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class UnitOfWork <TEntity, TKey>  : IUnitOfWork <TEntity, TKey>
        where TEntity : class
    {
        private readonly AppDbContext _appDbContext;
        public IMovieRepository _movieRepository { get; set; }
        //public IGenericRepository<TEntity, TKey> repository { get; set; }
        public IGenericRepository<Genre, int> _genresRepository;
        public IGenericRepository<Country, int> _countriesRepository;
        public IGenericRepository<Person, int> _peopleRepository;
        private IGenericRepository<TEntity, TKey> _repository;

        public UnitOfWork(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IMovieRepository Movies => _movieRepository ??= new MovieRepository(_appDbContext);
        //public IGenericRepository<TEntity, TKey> Genres => repository ??= new GenericRepository<TEntity, TKey>(_appDbContext);
        public IGenericRepository<Genre, int> Genres => _genresRepository ??= new GenericRepository<Genre, int>(_appDbContext);
        public IGenericRepository<Country, int> Countries => _countriesRepository ??= new GenericRepository<Country, int>(_appDbContext);
        public IGenericRepository<Person, int> People => _peopleRepository ??= new GenericRepository<Person, int>(_appDbContext);

        public IGenericRepository<TEntity, TKey> Repository => _repository ??= new GenericRepository<TEntity, TKey>(_appDbContext);

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
