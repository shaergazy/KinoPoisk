using DAL;
using DAL.Entities;
using Data.Repositories.RepositoryInterfaces;

namespace Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _appDbContext;
        public IMovieRepository _movieRepository { get; set; }
        public IBaseRepository<Genre, int> _genresRepository;

        public UnitOfWork(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IMovieRepository Movies => _movieRepository ??= new MovieRepository(_appDbContext);
        public IBaseRepository<Genre, int> Genres => _genresRepository ??= new BaseRepository<Genre, int>(_appDbContext);

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
