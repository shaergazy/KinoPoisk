using BLL.DTO;
using BLL.DTO.Genre;
using DAL.Models;

namespace BLL.Services.Interfaces
{
    public interface IGenreService : ITranslatableService<ListGenreDto, AddGenreDto, EditGenreDto, GetGenreDto, Genre, Guid, DataTablesRequestDto>, IService
    {
        Task ImportGenres(string genreNames, Movie movie);
    }
}
