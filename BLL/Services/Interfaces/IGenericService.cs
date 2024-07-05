namespace BLL.Services.Interfaces
{
    public interface IGenericService<TListDto, TAddDto, TEditDto, TGetDto, TEntity, TKey> : IService
        where TAddDto : class
        where TEditDto : class
        where TListDto : class
        where TGetDto : class
        where TEntity : class
    {
        IEnumerable<TListDto> GetAll();
        Task<TGetDto> GetByIdAsync(TKey id);
        Task<TEntity> CreateAsync(TAddDto dto);
        Task UpdateAsync(TEditDto dto);
        Task DeleteAsync(TKey id);
    }
}
