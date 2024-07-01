using BLL.DTO;

namespace BLL.Services.Interfaces
{
    public interface ICountryService : IService
    {
        Task<int> CreateAsync(CountryDto.Add dto);
        Task<List<CountryDto.Get>> GetAll();
        Task UpdateAsync(CountryDto.Edit dto);
        Task DeleteById(int id);
        Task<CountryDto.Get> GetById(int id);
    }
}
