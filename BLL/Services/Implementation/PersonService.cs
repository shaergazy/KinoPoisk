using AutoMapper;
using BLL.DTO;
using BLL.DTO.Person;
using BLL.Services.Interfaces;
using DAL.Enums;
using DAL.Models;
using Data.Models;
using Data.Repositories.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Dynamic.Core;

namespace BLL.Services.Implementation
{
    public class PersonService : SearchableService<ListPersonDto, AddPersonDto, EditPersonDto, GetPersonDto, Person, int, DataTablesRequestDto>,
        IPersonService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<Person, int> _uow;
        private readonly ILogger<PersonService> _logger;

        public PersonService(IMapper mapper, IUnitOfWork<Person, int> unitOfWork, ILogger<PersonService> logger)
            : base(mapper, unitOfWork, logger)
        {
            _mapper = mapper;
            _uow = unitOfWork;
            _logger = logger;
        }

        public async Task ImportPeopleAsync(string actorNames, string directorName, Movie movie)
        {
            try
            {
                _logger.LogInformation("Importing people for movie: {MovieTitle}", movie.Title);
                await ImportActorsAsync(actorNames, movie);
                await ImportDirectorAsync(directorName, movie);
                _logger.LogInformation("Successfully imported people for movie: {MovieTitle}", movie.Title);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while importing people for movie: {MovieTitle}", movie.Title);
                throw;
            }
        }

        private async Task ImportActorsAsync(string actorNames, Movie movie)
        {
            var actors = actorNames.Split(", ");
            uint order = 1;
            foreach (var personName in actors)
            {
                try
                {
                    _logger.LogInformation("Importing actor: {PersonName}", personName);
                    var names = personName.Split(" ");
                    var firstName = names.First();
                    var lastName = names.Last();
                    var person = await _uow.People.FirstOrDefaultAsync(p => p.FirstName == firstName && p.LastName == lastName);
                    if (person == null)
                    {
                        person = new Person { FirstName = firstName, LastName = lastName };
                        await _uow.People.AddAsync(person);
                        _logger.LogInformation("Added new actor: {PersonName}", personName);
                    }
                    else
                    {
                        _logger.LogInformation("Actor: {PersonName} already exist in database", personName);
                    }
                    movie.People.Add(new MoviePerson { Person = person, Order = order, PersonType = PersonType.Actor });
                    order++;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while importing actor: {PersonName}", personName);
                    throw;
                }
            }
        }

        private async Task ImportDirectorAsync(string directorName, Movie movie)
        {
            try
            {
                _logger.LogInformation("Importing director: {DirectorName}", directorName);
                var names = directorName.Split(" ");
                var firstName = names.First();
                var lastName = names.Last();
                var director = await _uow.People.FirstOrDefaultAsync(p => p.FirstName == firstName && p.LastName == lastName);
                if (director == null)
                {
                    director = new Person { FirstName = firstName, LastName = lastName };
                    await _uow.People.AddAsync(director);
                    _logger.LogInformation("Added new director: {DirectorName}", directorName);
                }
                else
                {
                    _logger.LogInformation("Director: {PersonName} already exist in database", directorName);
                }
                movie.People.Add(new MoviePerson { Person = director, PersonType = PersonType.Director });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while importing director: {DirectorName}", directorName);
                throw;
            }
        }

        public override IQueryable<Person> FilterEntities(DataTablesRequestDto request, IQueryable<Person>? entities = null)
        {
            try
            {
                _logger.LogInformation("Filtering entities with search term: {SearchTerm}", request.SearchTerm);
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while filtering entities with search term: {SearchTerm}", request.SearchTerm);
                throw;
            }
        }

        public IEnumerable<ListPersonDto> GetActors()
        {
            try
            {
                _logger.LogInformation("Fetching all actors");
                var actors = _uow.Repository.GetAll()
                                            .Include(p => p.Movies)
                                            .ThenInclude(mp => mp.Movie)
                                            .Where(p => p.Movies.Any(mp => mp.PersonType == PersonType.Actor))
                                            .ToList();
                return _mapper.Map<List<ListPersonDto>>(actors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching actors");
                throw;
            }
        }

        public IEnumerable<ListPersonDto> GetDirectors()
        {
            try
            {
                _logger.LogInformation("Fetching all directors");
                var directors = _uow.Repository.GetAll()
                                               .Include(p => p.Movies)
                                               .ThenInclude(mp => mp.Movie)
                                               .Where(p => p.Movies.Any(mp => mp.PersonType == PersonType.Director))
                                               .ToList();
                return _mapper.Map<List<ListPersonDto>>(directors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching directors");
                throw;
            }
        }
    }
}
