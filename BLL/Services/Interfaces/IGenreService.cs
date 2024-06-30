using Common.DTO;

namespace BLL.Services.Interfaces
{
    public interface IGenreService : IService
    {
        Task<int> Create(GenreDto.Base dto);
        Task<List<GenreDto.IdHasBase>> GetAll();
        Task EditById(GenreDto.IdHasBase dto);
        Task DeleteById(int id);
        Task<GenreDto.IdHasBase> GetById(int id);
    }
}
