using BLL.DTO;
using BLL.DTO.Country;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace BLL.Services.Interfaces
{
    public interface ICountryService : ISearchableService<ListCountryDto, AddCountryDto, EditCountryDto, GetCountryDto, Country, int>, IService
    {
        public Task<JsonResult> GetSortedAsync(DataTablesRequestDto model);
    }
}
