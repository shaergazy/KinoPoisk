using AutoMapper;
using BLL.Services.Interfaces;
using Data.Repositories.RepositoryInterfaces;

namespace BLL.Services.Implementation
{
    public class GenericService<TListDto, TAddDto, TEditDTo, TGetDto, TEntity, TKey> : IGenericService<TListDto, TAddDto, TEditDTo, TGetDto, TEntity, TKey>
    where TAddDto : class
    where TEditDTo : class
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

        public async Task<IEnumerable<TListDto>> GetAllAsync()
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
            var entity = _mapper.Map<TEntity>(dto);
            await _unitOfWork.Repository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(TEditDTo dto)
        {
            var entity = _mapper.Map<TEntity>(dto);
            await _unitOfWork.Repository.UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(TKey id)
        {
            var entity = await _unitOfWork.Repository.GetByIdAsync(id);
            if (entity != null)
            {
                await _unitOfWork.Repository.Remove(entity);
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}
