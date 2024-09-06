using AutoMapper;
using BLL.DTO.Genre;
using DAL.Interfaces;
using DAL.Models;
using Data.Repositories.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repositories;

namespace BLL.Services.Implementation
{
    public class TranslatableGenericService<TListDto, TAddDto, TEditDto, TGetDto, TEntity, TKey> : GenericService<TListDto, TAddDto, TEditDto, TGetDto, TEntity, TKey>
    where TAddDto : class
    where TEditDto : class
    where TListDto : class
    where TGetDto : class
    where TEntity : TranslatableEntity
    {
        private readonly IUnitOfWork<TEntity, TKey> _uow;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        public TranslatableGenericService(IMapper mapper, IUnitOfWork<TEntity, TKey> unitOfWork, ILogger<GenericService<TListDto, TAddDto, TEditDto, TGetDto, TEntity, TKey>> logger) : base(mapper, unitOfWork, logger)
        {
            _uow = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        protected virtual IQueryable<TEntity> GetAllWithTranslations()
        {
            return _uow.Repository
                .GetAll().Include("Translations");
        }

        public override async Task<TGetDto> GetByIdAsync(TKey id)
        {
            var entity = await GetWithTranslationsByIdAsync(id);

            return _mapper.Map<TGetDto>(entity);
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
