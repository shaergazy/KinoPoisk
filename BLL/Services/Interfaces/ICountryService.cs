using BLL.DTO.Country;

namespace BLL.Services.Interfaces
{
    public interface ICountryService : IService
    {
        Task<int> CreateAsync(AddCountryDto dto);
        Task<List<ListCountryDto>> GetAll();
        Task UpdateAsync(EditCountryDto dto);
        Task DeleteById(int id);
        Task<GetCountryDto> GetById(int id);
    }
}
