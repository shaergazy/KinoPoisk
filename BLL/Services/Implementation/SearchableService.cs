using AutoMapper;
using BLL.Services.Interfaces;
using Data.Repositories.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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

        public async Task<IQueryable<TEntity>> SearchAsync(string searchTerm, params Expression<Func<TEntity, object>>[] properties)
        {
            var query = _unitOfWork.Repository.GetAll();

            if (!string.IsNullOrWhiteSpace(searchTerm) && properties != null && properties.Length > 0)
            {
                var parameter = Expression.Parameter(typeof(TEntity), "x");

                var searchExpressions = properties
                    .Select(property =>
                    {
                        var propertyExpression = Expression.Convert(Expression.Invoke(property, parameter), typeof(string));
                        var toUpper = Expression.Call(propertyExpression, "ToUpper", Type.EmptyTypes);
                        var contains = Expression.Call(toUpper, "Contains", null, Expression.Constant(searchTerm.ToUpper()));
                        return contains;
                    })
                    .Aggregate<Expression>((current, next) => Expression.OrElse(current, next));

                var lambda = Expression.Lambda<Func<TEntity, bool>>(searchExpressions, parameter);
                query = query.Where(lambda);
            }

            return query;
        }

        public async Task<IEnumerable<TListDto>> SearchByConditionAsync(Expression<Func<TEntity, bool>> condition)
        {
            var entities = await _unitOfWork.Repository.Where(condition).ToListAsync();
            return _mapper.Map<IEnumerable<TListDto>>(entities);
        }
    }
}
