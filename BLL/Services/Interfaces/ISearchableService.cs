using BLL.DTO;

namespace BLL.Services.Interfaces
{
    public interface ISearchableService<TListDto, TAddDto, TEditDto, TGetDto, TEntity, TKey, TDataTableRequest> 
        : IGenericService<TListDto, TAddDto, TEditDto, TGetDto, TEntity, TKey>
            where TAddDto : class
            where TEditDto : class
            where TListDto : class
            where TGetDto : class
            where TEntity : class
            where TDataTableRequest : class
    {
        Task<DataTablesResponse<TEntity>> SearchAsync(TDataTableRequest request);

        Task<IList<TEntity>> GetPagedData(TDataTableRequest request, IQueryable<TEntity> entities);

        public IQueryable<TEntity> OrderByColumn(IQueryable<TEntity> entities, TDataTableRequest request);

        public IQueryable<TEntity> FilterEntities(TDataTableRequest request, IQueryable<TEntity>? entities = null);
    }
}
