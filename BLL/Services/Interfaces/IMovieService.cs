using BLL.DTO.Country;
using BLL.DTO.Genre;
using BLL.DTO.Movie;
using BLL.DTO.Person;
using DAL.Models;

namespace BLL.Services.Interfaces
{
    public interface IMovieService : ISearchableService<ListMovieDto, AddMovieDto, EditMovieDto, GetMovieDto, Movie, Guid>, IService
    {
        Task<int> AddRatingAsync(AddMovieRating dto);
        Task<int> AddCommentAsync(AddCommentDo dto);

        Task<IEnumerable<Comment>> GetCommentsAsync(Guid id);
        Task DeleteCommentAsync(int commentId);

        //Task<IEnumerable<Movie>> GetMoviesFromExternalSourceAsync(string titleOrIMDBId);
        //Task ImportMovieFromExternalSourceAsync(Movie movie);

        Task<IEnumerable<ListMovieDto>> GetTopRatedMoviesAsync(int count);
        Task<IEnumerable<ListMovieDto>> GetNewestMoviesAsync(int count);
        IEnumerable<ListCountryDto> GetCountries();
        IEnumerable<ListGenreDto> GetGenres();
        IEnumerable<ListPersonDto> GetPeople();
    }
}
