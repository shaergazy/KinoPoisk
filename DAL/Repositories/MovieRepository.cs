using DAL;
using DAL.Models;
using Data.Repositories.RepositoryInterfaces;

namespace Repositories
{
    public class MovieRepository : GenericRepository<Movie, Guid>, IMovieRepository
    {
        private readonly AppDbContext _appDbContext;
        public MovieRepository(AppDbContext context) : base(context)
        {
            _appDbContext = context;
        }

        public IEnumerable<Movie> GetTopHighRatingMovie(int count)
        {
            return _appDbContext.Movies.OrderBy(x => x.Ratings).Take(count).ToList();
        }

        public IEnumerable<Movie> GetTopNewestMovie(int count)
        {
            return _appDbContext.Movies.OrderBy(x => x.RealesedDate).Take(count).ToList();
        }
    }
}
