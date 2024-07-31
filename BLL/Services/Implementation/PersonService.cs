using AutoMapper;
using BLL.DTO;
using BLL.DTO.Person;
using BLL.Services.Interfaces;
using DAL.Enums;
using DAL.Models;
using Data.Models;
using Data.Repositories.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using Repositories;
using System.Linq.Dynamic.Core;

namespace BLL.Services.Implementation
{
    public class PersonService : SearchableService<ListPersonDto, AddPersonDto, EditPersonDto, GetPersonDto, Person, int, DataTablesRequestDto>,
        IPersonService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<Person, int> _uow;

        public PersonService(IMapper mapper, IUnitOfWork<Person, int> unitOfWork) : base(mapper, unitOfWork)
        {
            _mapper = mapper;
            _uow = unitOfWork;
        }

        public async Task ImportPeopleAsync(string actorNames, string directorName, Movie movie)
        {
            await ImportActorsAsync(actorNames, movie);
            await ImportDirectorAsync(directorName, movie);
        }

        private async Task ImportActorsAsync(string actorNames, Movie movie)
        {
            var actors = actorNames.Split(", ");
            uint order = 1;
            foreach (var personName in actors)
            {
                var names = personName.Split(" ");
                var firstName = names.First();
                var lastName = names.Last();
                var person = await _uow.People.FirstOrDefaultAsync(p => p.FirstName == firstName && p.LastName == lastName);
                if (person == null)
                {
                    person = new Person { FirstName = firstName, LastName = lastName };
                    await _uow.People.AddAsync(person);
                }
                movie.People.Add(new MoviePerson { Person = person, Order = order, PersonType = PersonType.Actor });
                order++;
            }
        }

        private async Task ImportDirectorAsync(string directorName, Movie movie)
        {
            var names = directorName.Split(" ");
            var firstName = names.First();
            var lastName = names.Last();
            var director = await _uow.People.FirstOrDefaultAsync(p => p.FirstName == firstName && p.LastName == lastName);
            if (director == null)
            {
                director = new Person { FirstName = firstName, LastName = lastName };
                await _uow.People.AddAsync(director);
            }
            movie.People.Add(new MoviePerson { Person = director, PersonType = PersonType.Director });
        }

        public override IQueryable<Person> FilterEntities(DataTablesRequestDto request, IQueryable<Person>? entities = null)
        {
            var searchTerm = request.SearchTerm;
            if (entities == null)
                entities = _uow.Repository.GetAll();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                entities = entities.Where(m => (m.FirstName + " " + m.LastName).Contains(searchTerm)
                                            || (m.LastName + " " + m.FirstName).Contains(searchTerm));
            }
            return entities;
        }

        public IEnumerable<ListPersonDto> GetActors()
        {
            var actors = _uow.Repository.GetAll()
                                        .Include(p => p.Movies)
                                        .ThenInclude(mp => mp.Movie)
                                        .Where(p => p.Movies.Any(mp => mp.PersonType == DAL.Enums.PersonType.Actor))
                                        .ToList();
            return _mapper.Map<List<ListPersonDto>>(actors);
        }

        public IEnumerable<ListPersonDto> GetDirectors()
        {
            var directors = _uow.Repository.GetAll()
                                       .Include(p => p.Movies)
                                       .ThenInclude(mp => mp.Movie)
                                       .Where(p => p.Movies.Any(mp => mp.PersonType == DAL.Enums.PersonType.Director))
                                       .ToList();
            return _mapper.Map<List<ListPersonDto>>(directors);
        }
    }
}
