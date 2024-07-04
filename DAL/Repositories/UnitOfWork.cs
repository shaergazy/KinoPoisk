using DAL;
using DAL.Entities;
using DAL.Models;
using Data.Repositories.RepositoryInterfaces;

namespace Repositories
{
    public class UnitOfWork <TEntity, TKey> : IUnitOfWork <TEntity, TKey>
        where TEntity : class
        where TKey : class
    {
        private readonly AppDbContext _appDbContext;
        public IMovieRepository _movieRepository { get; set; }
        public IBaseRepository<TEntity, TKey> repository { get; set; }
        //public IBaseRepository<Genre, int> _genresRepository;
        //public IBaseRepository<Country, int> _countriesRepository;
        //public IBaseRepository<Person, int> _peopleRepository;

        public UnitOfWork(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IMovieRepository Movies => _movieRepository ??= new MovieRepository(_appDbContext);
        public IBaseRepository<TEntity, TKey> Genres => repository ??= new BaseRepository<TEntity, TKey>(_appDbContext);
        //public IBaseRepository<Genre, int> Genres => _genresRepository ??= new BaseRepository<Genre, int>(_appDbContext);
        //public IBaseRepository<Country, int> Countries => _countriesRepository ??= new BaseRepository<Country, int>(_appDbContext);
        //public IBaseRepository<Person, int> People => _peopleRepository ??= new BaseRepository<Person, int>(_appDbContext);


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
