using AutoMapper;
using BLL.DTO;
using BLL.Services.Interfaces;
using Data.Repositories.RepositoryInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace BLL.Services.Implementation
{
    public class SearchableService<TListDto, TAddDto, TEditDTo, TGetDto, TEntity, TKey>
        : GenericService<TListDto, TAddDto, TEditDTo, TGetDto, TEntity, TKey> , 
        ISearchableService<TListDto, TAddDto, TEditDTo, TGetDto, TEntity, TKey>
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

        public virtual async Task<DataTablesResponse<TEntity>> SearchAsync(DataTablesRequestDto request)
        {
            var entities = _unitOfWork.Repository.GetAll();

            var recordsTotal = entities.Count();

            entities = FilterEntities(entities, request.SearchTerm?.ToUpper());

            var recordsFiltered = entities.Count();

            entities = OrderByColumn(entities, request);

            var data = await GetPagedData(request, entities);

            var s = new DataTablesResponse<TEntity>()
            {
                Draw = request.Draw,
                RecordsTotal = recordsTotal,
                RecordsFiltered = recordsFiltered,
                Data = data
            };
            return s;
        }

        public virtual IQueryable<TEntity> FilterEntities(IQueryable<TEntity> entities, string searchTerm)
        {
            return entities;
        }

        public async virtual Task<IList<TEntity>> GetPagedData(DataTablesRequestDto request, IQueryable<TEntity> entities)
        {
            var data = await entities
                .Skip(request.Start)
                .Take(request.Length)
                .ToListAsync();

            return data;
        }

        public virtual IQueryable<TEntity> OrderByColumn(IQueryable<TEntity> entities, DataTablesRequestDto request)
        {
            var sortColumnName = request.SortColumn;
            var sortDirection = request.SortDirection;

            entities = entities.OrderBy($"{sortColumnName} {sortDirection}");
            return entities;
        }
    }
}
