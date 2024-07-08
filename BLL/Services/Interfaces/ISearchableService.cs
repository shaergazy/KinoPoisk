using BLL.DTO;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace BLL.Services.Interfaces
{
    public interface ISearchableService<TListDto, TAddDto, TEditDto, TGetDto, TEntity, TKey, TRequestDto> 
        : IGenericService<TListDto, TAddDto, TEditDto, TGetDto, TEntity, TKey>
            where TAddDto : class
            where TEditDto : class
            where TListDto : class
            where TGetDto : class
            where TEntity : class
    {
        Task<JsonResult> SearchAsync(TRequestDto request);

        Task<IList<TEntity>> GetPagedData(TRequestDto request, IQueryable<TEntity> entities);

        public IQueryable<TEntity> OrderByColumn(IQueryable<TEntity> entities, TRequestDto request);

        public IQueryable<TEntity> FilterEntities(IQueryable<TEntity> entities, string searchTerm);
    }
}
