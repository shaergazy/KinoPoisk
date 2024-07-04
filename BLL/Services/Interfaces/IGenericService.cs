using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IGenericService<TDto, TEntity, TKey>
    where TDto : class
    where TEntity : class
    where TKey : class
    {
        Task<IEnumerable<TDto>> GetAllAsync();
        Task<TDto> GetByIdAsync(TKey id);
        Task<TDto> CreateAsync(TDto dto);
        Task<TDto> UpdateAsync(TDto dto);
        Task DeleteAsync(TKey id);
    }
}
