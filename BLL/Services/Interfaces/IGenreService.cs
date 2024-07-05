using BLL.DTO.Country;
using BLL.DTO.Genre;
using DAL.Models;

namespace BLL.Services.Interfaces
{
    public interface IGenreService : ISearchableService<ListGenreDto, AddGenreDto, EditGenreDto, GetGenreDto, Genre, int>, IService
    { }
}
