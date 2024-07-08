using AutoMapper;
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

        public async Task<JsonResult> GetSortedAsync(DataTablesRequestDto request)
        {
            var entities = _uow.Repository.GetAll();

            var recordsTotal = entities.Count();

            var searchText = request.SearchTerm?.ToUpper();
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                entities = entities.Where(m => (m.FirstName + " " + m.LastName).Contains(searchText)
                                            || (m.LastName + " " + m.FirstName).Contains(searchText));
            }

            var recordsFiltered = entities.Count();

            var sortColumnName = request.Column;
            var sortDirection = request.Order;

            entities = entities.OrderBy($"{sortColumnName} {sortDirection}");

            var skip = request.Start;
            var take = request.Length;
            var data = await entities
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
