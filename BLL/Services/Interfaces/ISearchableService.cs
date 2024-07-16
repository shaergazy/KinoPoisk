using BLL.DTO;

namespace BLL.Services.Interfaces
{
    public interface ISearchableService<TListDto, TAddDto, TEditDto, TGetDto, TEntity, TKey> 
        : IGenericService<TListDto, TAddDto, TEditDto, TGetDto, TEntity, TKey>
            where TAddDto : class
            where TEditDto : class
            where TListDto : class
            where TGetDto : class
            where TEntity : class
    {
        Task<DataTablesResponse<TEntity>> SearchAsync(DataTablesRequestDto request);

        Task<IList<TEntity>> GetPagedData(DataTablesRequestDto request, IQueryable<TEntity> entities);

        public IQueryable<TEntity> OrderByColumn(IQueryable<TEntity> entities, DataTablesRequestDto request);

        public IQueryable<TEntity> FilterEntities(string searchTerm, IQueryable<TEntity>? entities = null);
    }
}
