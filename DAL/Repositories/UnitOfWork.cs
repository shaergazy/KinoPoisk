using DAL;
using Data.Repositories.RepositoryInterfaces;

namespace Repositories
{
    public class UnitOfWork : IUnitOdWork
    {
        private readonly AppDbContext _appDbContext;
        public IMovieRepository MovieRepository { get; set; }

        public UnitOfWork(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            MovieRepository = new MovieRepository(_appDbContext);
        }

        public async Task CommitAsync()
        {
            await _appDbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _appDbContext.Dispose();
        }
    }
}
