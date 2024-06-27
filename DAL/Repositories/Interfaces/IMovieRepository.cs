using DAL.Entities;

namespace Data.Repositories.RepositoryInterfaces
{
    public interface IMovieRepository
    {
        IEnumerable<Movie> GetTopHighRatingMovie(int count);
        IEnumerable<Movie> GetTopNewestMovie(int count);
    }
}
