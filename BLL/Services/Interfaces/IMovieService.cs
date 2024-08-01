using BLL.DTO;
using BLL.DTO.Movie;
using DAL.Models;

namespace BLL.Services.Interfaces
{
    public interface IMovieService : ISearchableService<ListMovieDto, AddMovieDto, EditMovieDto, GetMovieDto, Movie, Guid, MovieDataTablesRequestDto>, IService
    {
        Task<byte[]> GeneratePdfAsync(MovieDataTablesRequestDto dto);
        Task<byte[]> GenerateExcelAsync(MovieDataTablesRequestDto dto);
        Task AddRatingAsync(AddMovieRating dto);
        Task AddCommentAsync(AddCommentDto dto);
        public Task ImportMovieAsync(ExternalMovieDto dto);

        Task<DataTablesResponse<GetCommentDto>> GetCommentsAsync(Guid id, DataTablesRequestDto request);
        Task DeleteCommentAsync(int commentId);

        public IQueryable<Movie> SortByParametrs(IQueryable<Movie> entities, MovieDataTablesRequestDto request);

        //Task<IEnumerable<Movie>> GetMoviesFromExternalSourceAsync(string titleOrIMDBId);
        //Task ImportMovieFromExternalSourceAsync(Movie movie);

        Task<IEnumerable<ListMovieDto>> GetTopRatedMoviesAsync(int count);
        Task<IEnumerable<ListMovieDto>> GetNewestMoviesAsync(int count);
    }
}
