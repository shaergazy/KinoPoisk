using AutoMapper;
using BLL.DTO;
using BLL.Services.Implementation;
using BLL.Services.Interfaces;
using Data.Repositories.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

public class SearchableService<TListDto, TAddDto, TEditDTo, TGetDto, TEntity, TKey, TDataTableRequest>
    : GenericService<TListDto, TAddDto, TEditDTo, TGetDto, TEntity, TKey>,
    ISearchableService<TListDto, TAddDto, TEditDTo, TGetDto, TEntity, TKey, TDataTableRequest>
    where TAddDto : class
    where TEditDTo : class
    where TListDto : class
    where TGetDto : class
    where TEntity : class
    where TDataTableRequest : DataTablesRequestDto
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork<TEntity, TKey> _unitOfWork;

    public SearchableService(IMapper mapper, IUnitOfWork<TEntity, TKey> unitOfWork) : base(mapper, unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public virtual async Task<DataTablesResponse<TEntity>> SearchAsync(TDataTableRequest request)
    {
        var entities = _unitOfWork.Repository.GetAll();

        var recordsTotal = entities.Count();

        entities = FilterEntities(request, entities);

        var recordsFiltered = entities.Count();
        entities = OrderByColumn(entities, request);
        var data = await GetPagedData(request, entities);

        var response = new DataTablesResponse<TEntity>
        {
            Draw = request.Draw,
            RecordsTotal = recordsTotal,
            RecordsFiltered = recordsFiltered,
            Data = data
        };
        return response;
    }

    public async virtual Task<IList<TEntity>> GetPagedData(TDataTableRequest request, IQueryable<TEntity> entities)
    {
        var data = await entities
            .Skip(request.Start)
            .Take(request.Length)
            .ToListAsync();

        return data;
    }

    public virtual IQueryable<TEntity> OrderByColumn(IQueryable<TEntity> entities, TDataTableRequest request)
    {
        var sortColumnName = request.SortColumn;
        var sortDirection = request.SortDirection;

        entities = entities.OrderBy($"{sortColumnName} {sortDirection}");
        return entities;
    }

    public virtual IQueryable<TEntity> FilterEntities(TDataTableRequest request, IQueryable<TEntity> entities = null)
    {
        if (entities == null)
        {
            entities = _unitOfWork.Repository.GetAll();
        }

        return entities;
    }
}
