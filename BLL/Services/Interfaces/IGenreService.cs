using BLL.DTO;

namespace BLL.Services.Interfaces
{
    public interface IGenreService : IService
    {
        Task<int> CreateAsync(GenreDto.Base dto);
        Task<List<GenreDto.IdHasBase>> GetAll();
        Task UpdateAsync(GenreDto.IdHasBase dto);
        Task DeleteById(int id);
        Task<GenreDto.IdHasBase> GetById(int id);
    }
}
