using BLL.DTO.GenreDTOs;

namespace BLL.Services.Interfaces
{
    public interface IGenreService : IService
    {
        Task<int> CreateAsync(AddGenreDto dto);
        Task<List<ListGenreDto>> GetAll();
        Task UpdateAsync(EditGenreDto dto);
        Task DeleteById(int id);
        Task<GetGenreDto> GetById(int id);
    }
}
