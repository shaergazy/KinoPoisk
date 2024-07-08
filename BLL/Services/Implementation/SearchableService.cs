using AutoMapper;
using BLL.Services.Interfaces;
using Data.Repositories.RepositoryInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services.Implementation
{
    public class SearchableService<TListDto, TAddDto, TEditDTo, TGetDto, TEntity, TKey, TRequestDto>
        : GenericService<TListDto, TAddDto, TEditDTo, TGetDto, TEntity, TKey> , 
        ISearchableService<TListDto, TAddDto, TEditDTo, TGetDto, TEntity, TKey, TRequestDto>
            where TAddDto : class
            where TEditDTo : class
            where TListDto : class
            where TGetDto : class
            where TEntity : class
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<TEntity, TKey> _unitOfWork;

        public SearchableService(IMapper mapper, IUnitOfWork<TEntity, TKey> unitOfWork) : base(mapper, unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public virtual async Task<JsonResult> SearchAsync(TRequestDto request)
        {
            var entities = _unitOfWork.Repository.GetAll();
            entities = FilterEntities(entities, request);
            entities = OrderByColumn(entities, request);
            var data = GetPagedData(request, entities);

            return new JsonResult(new { Data = data });
        }

        public virtual IQueryable<TEntity> FilterEntities(IQueryable<TEntity> entities, TRequestDto searchTerm)
        {
            return entities;
        }

        public IQueryable<TEntity> FilterEntities(IQueryable<TEntity> entities, string searchTerm)
        {
            throw new NotImplementedException();
        }

        public async virtual Task<IList<TEntity>> GetPagedData(TRequestDto request, IQueryable<TEntity> entities)
        {
            return await entities.ToListAsync();
        }

        public virtual IQueryable<TEntity> OrderByColumn(IQueryable<TEntity> entities, TRequestDto request)
        {
            return entities;
        }
    }
}
