using AutoMapper;
using BLL.DTO;
using BLL.Services.Implementation;
using BLL.Services.Interfaces;
using Data.Repositories.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
    private readonly ILogger<SearchableService<TListDto, TAddDto, TEditDTo, TGetDto, TEntity, TKey, TDataTableRequest>> _logger;

    public SearchableService(IMapper mapper, IUnitOfWork<TEntity, TKey> unitOfWork, ILogger<SearchableService<TListDto, TAddDto, TEditDTo, TGetDto, TEntity, TKey, TDataTableRequest>> logger) : base(mapper, unitOfWork, logger)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public virtual async Task<DataTablesResponse<TEntity>> SearchAsync(TDataTableRequest request)
    {
        _logger.LogDebug("Starting search with request: {Request}", request);

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

        _logger.LogDebug("Search completed. Response: {Response}", response);

        return response;
    }

    public async virtual Task<IList<TEntity>> GetPagedData(TDataTableRequest request, IQueryable<TEntity> entities)
    {
        _logger.LogDebug("Getting paged data for request: {Request}", request);

        var data = entities
            .Skip(request.Start)
            .Take(request.Length)
            .ToList();

        _logger.LogDebug("Paged data retrieval completed. Data count: {Count}", data.Count);

        return data;
    }

    public virtual IQueryable<TEntity> OrderByColumn(IQueryable<TEntity> entities, TDataTableRequest request)
    {
        var sortColumnName = request.SortColumn;
        var sortDirection = request.SortDirection;

        _logger.LogDebug("Ordering by column {Column} {Direction}", sortColumnName, sortDirection);

        entities = entities.OrderBy($"{sortColumnName} {sortDirection}");
        return entities;
    }

    public virtual IQueryable<TEntity> FilterEntities(TDataTableRequest request, IQueryable<TEntity> entities = null)
    {
        if (entities == null)
        {
            _logger.LogWarning("Entities were null in FilterEntities. Fetching all entities.");
            entities = _unitOfWork.Repository.GetAll();
        }

        _logger.LogDebug("Filtering entities for request: {Request}", request);

        return entities;
    }
}
