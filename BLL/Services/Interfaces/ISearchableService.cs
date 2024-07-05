using System.Linq.Expressions;

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
        Task<IEnumerable<TListDto>> SearchAsync(string searchTerm, params Expression<Func<TEntity, string>>[] properties);
    }
}
