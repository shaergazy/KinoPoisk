using AutoMapper;
using AutoMapper.QueryableExtensions;
using BLL.Services.Interfaces;
using DAL.Models;
using Data.Repositories.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using BLL.DTO.Genre;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using BLL.DTO;
using Newtonsoft.Json.Linq;
using System.Buffers;
using System.Drawing.Printing;

namespace BLL.Services.Implementation
{
    public class GenreService : SearchableService<ListGenreDto, AddGenreDto, EditGenreDto, GetGenreDto, Genre, int>, IGenreService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<Genre, int> _uow;

        public GenreService(IMapper mapper, IUnitOfWork<Genre, int> unitOfWork) : base(mapper, unitOfWork)
        {
            _mapper = mapper;
            _uow = unitOfWork;
        }

        public  object GetAll(DataTableModel dataTableMode)
        {
            var Genres = _uow.Repository.GetAll();
            if (!(string.IsNullOrEmpty(dataTableMode.sortColumn) && string.IsNullOrEmpty(dataTableMode.sortColumnDirection)))
            {
                Genres = Genres.OrderBy(dataTableMode.sortColumn + " " + dataTableMode.sortColumnDirection);
            }
            if (!string.IsNullOrEmpty(dataTableMode.searchValue))
            {
                Genres = Genres.Where(m => m.Name.Contains(dataTableMode.searchValue));
            }
            dataTableMode.recordsTotal = Genres.Count();
            var data = Genres.Skip(dataTableMode.skip).Take(dataTableMode.pageSize).ToList();
            var jsonData = new { draw = dataTableMode.draw, recordsFiltered = dataTableMode.recordsTotal, recordsTotal = dataTableMode.recordsTotal, data = data };
            return jsonData;
        }
    }
}
