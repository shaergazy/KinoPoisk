using AutoMapper;
using BLL.DTO.Person;
using BLL.Services.Interfaces;
using DAL.Models;
using Data.Repositories.RepositoryInterfaces;
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

        public override IQueryable<Person> FilterEntities(IQueryable<Person> entities, string searchTerm)
        {
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                entities = entities.Where(m => (m.FirstName + " " + m.LastName).Contains(searchTerm)
                                            || (m.LastName + " " + m.FirstName).Contains(searchTerm));
            }
            return entities;
        }
    }
}
