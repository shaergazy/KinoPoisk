using AutoMapper;
using BLL.DataTables;
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

        public async Task<JsonResult> GetSortedAsync(DataTablesRequest request)
        {
            var genres = _uow.Repository.GetAll();

            var recordsTotal = genres.Count();

            var searchText = request.Search.Value?.ToUpper();
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                genres = genres.Where(s =>
                    s.Name.ToUpper().Contains(searchText)
                );
            }

            var recordsFiltered = genres.Count();

            var sortColumnName = request.Columns.ElementAt(request.Order.ElementAt(0).Column).Name;
            var sortDirection = request.Order.ElementAt(0).Dir.ToLower();

            genres = genres.OrderBy($"{sortColumnName} {sortDirection}");

            var skip = request.Start;
            var take = request.Length;
            var data = await genres
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            return new JsonResult(new
            {
                Draw = request.Draw,
                RecordsTotal = recordsTotal,
                RecordsFiltered = recordsFiltered,
                Data = data
            });
        }
    }
}
