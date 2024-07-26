using BLL.DTO;
using BLL.DTO.Movie;
using DAL.Models;

namespace BLL.Services.Interfaces
{
    public interface IMovieService : ISearchableService<ListMovieDto, AddMovieDto, EditMovieDto, GetMovieDto, Movie, Guid, MovieDataTablesRequestDto>, IService
    {
        Task AddRatingAsync(AddMovieRating dto);
        Task AddCommentAsync(AddCommentDo dto);
        public Task ImportMovieAsync(Item dto);

        Task<IEnumerable<GetCommentDto>> GetCommentsAsync(Guid movieId, int start, int length);
        Task DeleteCommentAsync(int commentId);

        public IQueryable<Movie> SortByParametrs(IQueryable<Movie> entities, MovieDataTablesRequestDto request);

        //Task<IEnumerable<Movie>> GetMoviesFromExternalSourceAsync(string titleOrIMDBId);
        //Task ImportMovieFromExternalSourceAsync(Movie movie);

        Task<IEnumerable<ListMovieDto>> GetTopRatedMoviesAsync(int count);
        Task<IEnumerable<ListMovieDto>> GetNewestMoviesAsync(int count);
    }
}
