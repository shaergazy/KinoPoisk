using AutoMapper;
using BLL.Services.Interfaces;
using Data.Repositories.RepositoryInterfaces;

namespace BLL.Services.Implementation
{
    public class GenericService<TListDto, TAddDto, TEditDto, TGetDto, TEntity, TKey> : IGenericService<TListDto, TAddDto, TEditDto, TGetDto, TEntity, TKey>
    where TAddDto : class
    where TEditDto : class
    where TListDto : class
    where TGetDto : class
    where TEntity : class
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<TEntity, TKey> _unitOfWork;

        public GenericService(IMapper mapper, IUnitOfWork <TEntity, TKey> unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public virtual IEnumerable<TListDto> GetAll()
        {
            var entities =  _unitOfWork.Repository.GetAll();
            return _mapper.Map<IEnumerable<TListDto>>(entities);
        }

        public async Task<TGetDto> GetByIdAsync(TKey id)
        {
            var entity = await _unitOfWork.Repository.GetByIdAsync(id);
            return _mapper.Map<TGetDto>(entity);
        }

        public async Task<TEntity> CreateAsync(TAddDto dto)
        {
            var entity = await BuildEntityForCreate(dto);
            await _unitOfWork.Repository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(TEditDto dto)
        {
            var entity = await BuildEntityForUpdate(dto);
            await _unitOfWork.Repository.UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(TKey id)
        {
            var entity = await BuildEntityForDelete(id);
            await _unitOfWork.Repository.Remove(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public virtual async Task<TEntity> BuildEntityForDelete(TKey id)
        {
            var entity = await _unitOfWork.Repository.GetByIdAsync(id);
            return entity;
        }
        public virtual async Task<TEntity> BuildEntityForCreate(TAddDto dto)
        {
            return _mapper.Map<TEntity>(dto);
        }

        public virtual async Task<TEntity> BuildEntityForUpdate(TEditDto dto)
        {
            return _mapper.Map<TEntity>(dto);
        }
    }
}
