using AutoMapper;
using BLL.Services.Interfaces;
using Data.Repositories.RepositoryInterfaces;

namespace BLL.Services.Implementation
{
    public class GenericService<TDto, TEntity, TKey> : IGenericService<TDto, TEntity, TKey>
    where TDto : class
    where TEntity : class
    where TKey : class
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<TEntity, TKey> _unitOfWork;

        public GenericService(IMapper mapper, IUnitOfWork <TEntity, TKey> unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<TDto>> GetAllAsync()
        {
            var entities =  _unitOfWork.repository.GetAll();
            return _mapper.Map<IEnumerable<TDto>>(entities);
        }

        public async Task<TDto> GetByIdAsync(TKey id)
        {
            var entity = await _unitOfWork.repository.GetByIdAsync(id);
            return _mapper.Map<TDto>(entity);
        }

        public async Task<TDto> CreateAsync(TDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto);
            await _unitOfWork.repository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<TDto>(entity);
        }

        public async Task<TDto> UpdateAsync(TDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto);
            await _unitOfWork.repository.UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<TDto>(entity);
        }

        public async Task DeleteAsync(TKey id)
        {
            var entity = await _unitOfWork.repository.GetByIdAsync(id);
            if (entity != null)
            {
                await _unitOfWork.repository.Remove(entity);
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}
