using BLL.DTO;
using BLL.DTO.Movie;
using DAL.Models;

namespace BLL.Services.Interfaces
{
    public interface IMovieService : ITranslatableService<ListMovieDto, AddMovieDto, EditMovieDto, GetMovieDto, Movie, Guid, MovieDataTablesRequestDto>, IService
    {
        Task<byte[]> GeneratePdfAsync(MovieDataTablesRequestDto dto, string languageCode = "en");
        Task<byte[]> GenerateExcelAsync(MovieDataTablesRequestDto dto, string languageCode = "en");
        Task AddRatingAsync(AddMovieRating dto);
        Task AddCommentAsync(AddCommentDto dto);
        public Task ImportMovieAsync(ExternalMovieDto dto);
        Task<GetMovieDto> GetByIdAsync(Guid id, string languageCode = "en");

        Task<DataTablesResponse<GetCommentDto>> GetCommentsAsync(Guid id, DataTablesRequestDto request);
        Task DeleteCommentAsync(int commentId);

        public IQueryable<Movie> SortByParametrs(IQueryable<Movie> entities, MovieDataTablesRequestDto request);

        //Task<IEnumerable<Movie>> GetMoviesFromExternalSourceAsync(string titleOrIMDBId);
        //Task ImportMovieFromExternalSourceAsync(Movie movie);

        Task<IEnumerable<ListMovieDto>> GetTopRatedMoviesAsync(int count, string language = "en");
        Task<IEnumerable<ListMovieDto>> GetNewestMoviesAsync(int count, string language = "en");
        Task UpdateImdbRatings();
    }
}
