using AutoMapper;
using BLL.DataTables;
using BLL.DTO;
using BLL.DTO.Person;
using BLL.Services.Interfaces;
using DAL.Models;
using Data.Repositories.RepositoryInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace BLL.Services.Implementation
{
    public class PersonService : SearchableService<ListPersonDto, AddPersonDto, EditPersonDto, GetPersonDto, Person, int>,
        IPersonService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<Person, int> _uow;

        public PersonService(IMapper mapper, IUnitOfWork<Person, int> unitOfWork) : base(mapper, unitOfWork)
        {
            _mapper = mapper;
            _uow = unitOfWork;
        }

        public async Task<JsonResult> GetSortedAsync(DataTablesRequest request)
        {
            var persons = _uow.Repository.GetAll();

            var recordsTotal = persons.Count();

            var searchText = request.Search.Value?.ToUpper();
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                persons = persons.Where(m => m.FirstName.Contains(searchText) ||
                                              m.LastName.Contains(searchText));
            }

            var recordsFiltered = persons.Count();

            var sortColumnName = request.Columns.ElementAt(request.Order.ElementAt(0).Column).Name;
            var sortDirection = request.Order.ElementAt(0).Dir.ToLower();

            persons = persons.OrderBy($"{sortColumnName} {sortDirection}");

            var skip = request.Start;
            var take = request.Length;
            var data = await persons
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
