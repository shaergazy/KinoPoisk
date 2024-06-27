using DAL.Entities;

namespace Repositories.Interfaces
{
    public interface IMovieRepository
    {
        IEnumerable<Movie> GetTopHighRatingMovie(int count);
        IEnumerable<Movie> GetTopNewestMovie(int count);
    }
}
