using AutoMapper;
using BLL.DTO.Genre;
using BLL.Services.Interfaces;
using DAL.Models;
using Data.Repositories.RepositoryInterfaces;
using System.Linq.Dynamic.Core;

namespace BLL.Services.Implementation
{
    public class GenreService : SearchableService<ListGenreDto, AddGenreDto, EditGenreDto, GetGenreDto, Genre, int>,
        IGenreService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<Genre, int> _uow;

        public GenreService(IMapper mapper, IUnitOfWork<Genre, int> unitOfWork) : base(mapper, unitOfWork)
        {
            _mapper = mapper;
            _uow = unitOfWork;
        }

        public override IQueryable<Genre> FilterEntities(string searchTerm, IQueryable<Genre>? entities = null)
        {
            if (entities == null)
                entities = _uow.Repository.GetAll();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                entities = entities.Where(s =>
                    s.Name.ToUpper().Contains(searchTerm));
            }
            return entities;
        }
    }
}
