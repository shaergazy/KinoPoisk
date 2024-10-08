﻿using AutoMapper;
using BLL.DTO;
using DAL.Models;
using Data.Repositories.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Dynamic.Core;

namespace BLL.Services.Implementation
{
    public class TranslatableService<TListDto, TAddDto, TEditDTo, TGetDto, TEntity, TKey, TDataTableRequest> : GenericService
        <TListDto, TAddDto, TEditDTo, TGetDto, TEntity, TKey>
    where TAddDto : class
    where TEditDTo : class
    where TListDto : class
    where TGetDto : class
    where TEntity : TranslatableEntity
    where TDataTableRequest : DataTablesRequestDto
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<TEntity, TKey> _uow;
        private readonly ILogger<GenericService<TListDto, TAddDto, TEditDTo, TGetDto, TEntity, TKey>> _logger;

        public TranslatableService(IMapper mapper, IUnitOfWork<TEntity, TKey> unitOfWork, ILogger<GenericService<TListDto, TAddDto, TEditDTo, TGetDto, TEntity, TKey>> logger)
            : base(mapper, unitOfWork, logger)
        {
            _mapper = mapper;
            _uow = unitOfWork;
            _logger = logger;
        }

        public virtual async Task<DataTablesResponse<TEntity>> SearchAsync(TDataTableRequest request)
        {
            _logger.LogDebug("Starting search with request: {Request}", request);

            var entities = GetAllWithTranslations();

            var recordsTotal = await entities.CountAsync();

            entities = FilterEntities(request, entities);

            var recordsFiltered = await entities.CountAsync();
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



        public virtual async Task<IList<TEntity>> GetPagedData(TDataTableRequest request, IQueryable<TEntity> entities)
        {
            _logger.LogDebug("Getting paged data for request: {Request}", request);

            var data = await entities
                .Skip(request.Start)
                .Take(request.Length)
                .ToListAsync();

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
                entities = GetAllWithTranslations();
            }

            _logger.LogDebug("Filtering entities for request: {Request}", request);

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                var searchValue = request.SearchTerm.ToLower();
                entities = entities.Where(e => e.Translations.Any(t => t.Value.ToLower().Contains(searchValue)));
            }

            return entities;
        }

        public virtual IQueryable<TEntity> GetAllWithTranslations()
        {
            return _uow.Repository
                .GetAll().Include("Translations");
        }

        public override async Task<TGetDto> GetByIdAsync(TKey id)
        {
            try
            {
                var entity = await GetWithTranslationsByIdAsync(id);
                return _mapper.Map<TGetDto>(entity);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public virtual async Task<TEntity> GetWithTranslationsByIdAsync(TKey id)
        {
            var entity = await _uow.Repository.FindAsync(id);
            await _uow.Repository
              .Entry(entity)
              .Collection(e => e.Translations)
              .LoadAsync();

            return entity;
        }

        public override async Task<TEntity> BuildEntityForDelete(TKey id)
        {
            var entity = await GetWithTranslationsByIdAsync(id);

            return entity;
        }
    }
}
