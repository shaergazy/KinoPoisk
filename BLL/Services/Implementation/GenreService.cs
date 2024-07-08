using AutoMapper;
using BLL.DTO;
using BLL.DTO.Genre;
using BLL.Services.Interfaces;
using DAL.Models;
using Data.Repositories.RepositoryInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace BLL.Services.Implementation
{
    public class GenreService : SearchableService<ListGenreDto, AddGenreDto, EditGenreDto, GetGenreDto, Genre, int, DataTablesRequestDto>,
        IGenreService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<Genre, int> _uow;

        public GenreService(IMapper mapper, IUnitOfWork<Genre, int> unitOfWork) : base(mapper, unitOfWork)
        {
            _mapper = mapper;
            _uow = unitOfWork;
        }

        public async override Task<JsonResult> SearchAsync(DataTablesRequestDto request)
        {
            var entities = _uow.Repository.GetAll();

            var recordsTotal = entities.Count();

            entities = FilterEntities(entities, request.SearchTerm?.ToUpper());

            var recordsFiltered = entities.Count();

            entities = OrderByColumn(entities, request);

            var data = GetPagedData(request, entities);

            return new JsonResult(new
            {
                Draw = request.Draw,
                RecordsTotal = recordsTotal,
                RecordsFiltered = recordsFiltered,
                Data = data
            });
        }
        public async override Task<IList<Genre>> GetPagedData(DataTablesRequestDto request, IQueryable<Genre> entities)
        {
            var data = await entities
                .Skip(request.Start)
                .Take(request.Length)
                .ToListAsync();
            return data;
        }

        public override IQueryable<Genre> OrderByColumn(IQueryable<Genre> entities, DataTablesRequestDto request)
        {
            var sortColumnName = request.Column;
            var sortDirection = request.Order;

            entities = entities.OrderBy($"{sortColumnName} {sortDirection}");
            return entities;
        }

        public override IQueryable<Genre> FilterEntities(IQueryable<Genre> entities, DataTablesRequestDto searchTerm)
        {
            if (!string.IsNullOrWhiteSpace(searchTerm.SearchTerm))
            {
                entities = entities.Where(s =>
                    s.Name.ToUpper().Contains(searchTerm.SearchTerm)
                );
            }
            return entities;
        }
    }
}
