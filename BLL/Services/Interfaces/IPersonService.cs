using BLL.DTO.PersonDTOs;

namespace BLL.Services.Interfaces
{
    public interface IPersonService : IService
    {
        Task<int> CreateAsync(AddPersonDto dto);
        Task<List<ListPersonDto>> GetAll();
        Task UpdateAsync(EditPersonDto dto);
        Task DeleteById(int id);
        Task<GetPersonDto> GetById(int id);
    }
}
