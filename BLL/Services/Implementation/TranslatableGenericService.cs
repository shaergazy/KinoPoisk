using AutoMapper;
using Data.Repositories.RepositoryInterfaces;
using Microsoft.Extensions.Logging;

namespace BLL.Services.Implementation
{
    public class TranslatableGenericService<TListDto, TAddDto, TEditDto, TGetDto, TEntity, TKey> : GenericService<TListDto, TAddDto, TEditDto, TGetDto, TEntity, TKey>
        where TAddDto : class
    where TEditDto : class
    where TListDto : class
    where TGetDto : class
    where TEntity : class
    {
        public TranslatableGenericService(IMapper mapper, IUnitOfWork<TEntity, TKey> unitOfWork, ILogger<GenericService<TListDto, TAddDto, TEditDto, TGetDto, TEntity, TKey>> logger) : base(mapper, unitOfWork, logger)
        {
        }
    }
}
