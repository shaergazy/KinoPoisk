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
using static QuestPDF.Helpers.Colors;

namespace BLL.Services.Implementation
{
    public class PersonService : TranslatableService<ListPersonDto, AddPersonDto, EditPersonDto, GetPersonDto, Person, Guid, DataTablesRequestDto>,
        IPersonService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<Person, Guid> _uow;
        private readonly ILogger<PersonService> _logger;

        public PersonService(IMapper mapper, IUnitOfWork<Person, Guid> unitOfWork, ILogger<PersonService> logger)
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
                _logger.LogInformation("Importing people for movie: {MovieTitle}", movie.Translations.FirstOrDefault(x => x.FieldType == TranslatableFieldType.Title));
                await ImportActorsAsync(actorNames, movie);
                await ImportDirectorAsync(directorName, movie);
                _logger.LogInformation("Successfully imported people for movie: {MovieTitle}", movie.Translations.FirstOrDefault(x => x.FieldType == TranslatableFieldType.Title));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while importing people for movie: {MovieTitle}", movie.Translations.FirstOrDefault(x => x.FieldType == TranslatableFieldType.Title));
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
                    movie.People.Add(new MoviePerson
                    {
                        Person = await ImportPersonAsync(personName, movie),
                        Order = order,
                        PersonType = PersonType.Actor
                    });
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
                movie.People.Add(new MoviePerson 
                { 
                    Person = await ImportPersonAsync(directorName, movie),
                    PersonType = PersonType.Director 
                });
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
                    entities = entities.Where(m => (m.Translations.Any(t => t.Value.Contains(searchTerm))));
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
                                            .Include(p => p.Translations)
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
                                               .Include(p => p.Translations)
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

        public async Task<Person> ImportPersonAsync(string personName, Movie movie)
        {
            _logger.LogInformation("Importing person: {PersonName}", personName);

            var names = personName.Split(" ");
            var firstNameEn = names.First();
            var lastNameEn = names.Last();

            var person = await _uow.People.FirstOrDefaultAsync(p =>
                p.Translations.Any(t => t.FieldType == TranslatableFieldType.FirstName &&
                                        t.LanguageCode == LanguageCode.en &&
                                        t.Value == firstNameEn) &&
                p.Translations.Any(t => t.FieldType == TranslatableFieldType.LastName &&
                                        t.LanguageCode == LanguageCode.en &&
                                        t.Value == lastNameEn));

            if (person == null)
            {
                person = new Person();

                person.Translations.Add(new TranslatableEntityField
                {
                    LanguageCode = LanguageCode.en,
                    FieldType = TranslatableFieldType.FirstName,
                    Value = firstNameEn
                });
                person.Translations.Add(new TranslatableEntityField
                {
                    LanguageCode = LanguageCode.en,
                    FieldType = TranslatableFieldType.LastName,
                    Value = lastNameEn
                });

                person.Translations.Add(new TranslatableEntityField
                {
                    LanguageCode = LanguageCode.ru,
                    FieldType = TranslatableFieldType.FirstName,
                    Value = firstNameEn
                });
                person.Translations.Add(new TranslatableEntityField
                {
                    LanguageCode = LanguageCode.ru,
                    FieldType = TranslatableFieldType.LastName,
                    Value = lastNameEn
                });

                await _uow.People.AddAsync(person);
                _logger.LogInformation("Added new person: {PersonName}", personName);
            }
            else
            {
                _logger.LogInformation("person: {PersonName} already exists in database", personName);

                if (!person.Translations.Any(t => t.LanguageCode == LanguageCode.ru && t.FieldType == TranslatableFieldType.FirstName))
                {
                    person.Translations.Add(new TranslatableEntityField
                    {
                        LanguageCode = LanguageCode.ru,
                        FieldType = TranslatableFieldType.FirstName,
                        Value = firstNameEn
                    });
                }

                if (!person.Translations.Any(t => t.LanguageCode == LanguageCode.ru && t.FieldType == TranslatableFieldType.LastName))
                {
                    person.Translations.Add(new TranslatableEntityField
                    {
                        LanguageCode = LanguageCode.ru,
                        FieldType = TranslatableFieldType.LastName,
                        Value = lastNameEn
                    });
                }
            }
            return person;
        }
    }
}
