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

        public async Task<IEnumerable<TListDto>> SearchAsync(string searchTerm, params Expression<Func<TEntity, string>>[] properties)
        {
            var query = _unitOfWork.Repository.GetAll();

            if (!string.IsNullOrWhiteSpace(searchTerm) && properties.Any())
            {
                var searchExpression = BuildSearchExpression(searchTerm, properties);
                query = query.Where(searchExpression);
            }

            var entities = await query.ToListAsync();
            return _mapper.Map<IEnumerable<TListDto>>(entities);
        }

        private static Expression<Func<TEntity, bool>> BuildSearchExpression(string searchTerm, params Expression<Func<TEntity, string>>[] properties)
        {
            var parameter = Expression.Parameter(typeof(TEntity), "t");
            var searchTermExpression = Expression.Constant(searchTerm, typeof(string));

            Expression searchExpression = null;
            foreach (var property in properties)
            {
                var propertyExpression = Expression.Invoke(property, parameter);
                var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                var containsExpression = Expression.Call(propertyExpression, containsMethod, searchTermExpression);

                if (searchExpression == null)
                {
                    searchExpression = containsExpression;
                }
                else
                {
                    searchExpression = Expression.OrElse(searchExpression, containsExpression);
                }
            }

            return Expression.Lambda<Func<TEntity, bool>>(searchExpression, parameter);
        }
    }
}
