using BLL.DataTables;
using BLL.DTO;
using BLL.DTO.Genre;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace BLL.Services.Interfaces
{
    public interface IGenreService : ISearchableService<ListGenreDto, AddGenreDto, EditGenreDto, GetGenreDto, Genre, int>, IService
    {
        public Task<JsonResult> GetSortedAsync(DataTablesRequest model);
    }
}
