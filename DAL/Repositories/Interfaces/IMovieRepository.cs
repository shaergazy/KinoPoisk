using DAL.Models;

namespace Data.Repositories.RepositoryInterfaces
{
    public interface IMovieRepository : IBaseRepository<Movie, Guid>
    {
        IEnumerable<Movie> GetTopHighRatingMovie(int count);
        IEnumerable<Movie> GetTopNewestMovie(int count);
    }
}
