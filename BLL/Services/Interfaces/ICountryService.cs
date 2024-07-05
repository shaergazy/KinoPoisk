using BLL.DTO.Country;
using DAL.Models;

namespace BLL.Services.Interfaces
{
    public interface ICountryService : ISearchableService<ListCountryDto, AddCountryDto, EditCountryDto, GetCountryDto, Country, int>, IService
    {
    }
}
