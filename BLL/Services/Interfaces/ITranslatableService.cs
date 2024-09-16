namespace BLL.Services.Interfaces
{
    public interface ITranslatableService<TListDto, TAddDto, TEditDto, TGetDto, TEntity, TKey, TDataTableRequest> : ISearchableService
            <TListDto, TAddDto, TEditDto, TGetDto, TEntity, TKey, TDataTableRequest>
            where TAddDto : class
            where TEditDto : class
            where TListDto : class
            where TGetDto : class
            where TEntity : class
            where TDataTableRequest : class
    {
        IQueryable<TEntity> GetAllWithTranslations();
        Task<TEntity> GetWithTranslationsByIdAsync(TKey id);
    }
}
