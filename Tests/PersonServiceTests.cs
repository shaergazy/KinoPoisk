using AutoMapper;
using BLL.DTO;
using BLL.DTO.Person;
using BLL.Services.Implementation;
using DAL.Enums;
using DAL.Models;
using Data.Models;
using Data.Repositories.RepositoryInterfaces;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

public class PersonServiceTests
{
    private readonly Mock<IUnitOfWork<Person, int>> _uowMock;
    private readonly IMapper _mapper;
    private readonly Mock<ILogger<PersonService>> _loggerMock;
    private readonly PersonService _personService;

    public PersonServiceTests()
    {
        _uowMock = new Mock<IUnitOfWork<Person, int>>();
        _loggerMock = new Mock<ILogger<PersonService>>();
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Person, ListPersonDto>();
        });
        _mapper = mapperConfig.CreateMapper();
        _personService = new PersonService(_mapper, _uowMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task ImportPeopleAsync_ShouldImportActorsAndDirector()
    {
        var actorNames = "John Doe, Jane Smith";
        var directorName = "Chris Nolan";
        var movie = new Movie { Title = "Inception", People = new List<MoviePerson>() };

        var actorJohn = new Person { FirstName = "John", LastName = "Doe" };
        var actorJane = new Person { FirstName = "Jane", LastName = "Smith" };
        var directorChris = new Person { FirstName = "Chris", LastName = "Nolan" };

        _uowMock.Setup(u => u.People.FirstOrDefaultAsync(It.IsAny<Expression<Func<Person, bool>>>()))
                       .ReturnsAsync((Expression<Func<Person, bool>> predicate) =>
                       {
                           if (predicate.Compile()(actorJohn)) return actorJohn;
                           if (predicate.Compile()(actorJane)) return actorJane;
                           return null;
                       });

        _uowMock.Setup(u => u.People.AddAsync(It.IsAny<Person>())).Returns((Person person) => Task.FromResult(person));

        await _personService.ImportPeopleAsync(actorNames, directorName, movie);

        _uowMock.Verify(u => u.People.AddAsync(It.IsAny<Person>()), Times.Once); 
        Assert.Equal(3, movie.People.Count); 
    }

    [Fact]
    public async Task GetActors_ShouldReturnActors()
    {
        var actors = new List<Person>
        {
            new Person { FirstName = "John", LastName = "Doe", Id = 1, BirthDate = DateTime.UtcNow, Movies = new List<MoviePerson>
            {
                new MoviePerson { PersonType = PersonType.Actor }
            }},
            new Person { FirstName = "Jane", LastName = "Smith", Id = 2, Movies = new List<MoviePerson>
            {
                new MoviePerson { PersonType = PersonType.Actor }
            }},
                    new Person { FirstName = "Lane", LastName = "Tina", Id = 3, Movies = new List<MoviePerson>
            {
                new MoviePerson { PersonType = PersonType.Director }
            }}
        };

        _uowMock.Setup(u => u.Repository.GetAll())
                .Returns(actors.AsQueryable());

        var result = _personService.GetActors();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetDirectors_ShouldReturnDirectors()
    {
        var people = new List<Person>
        {
            new Person { FirstName = "John", LastName = "Doe", Id = 1, BirthDate = DateTime.UtcNow, Movies = new List<MoviePerson>
            {
                new MoviePerson { PersonType = PersonType.Actor }
            }},
            new Person { FirstName = "Jane", LastName = "Smith", Id = 2, Movies = new List<MoviePerson>
            {
                new MoviePerson { PersonType = PersonType.Director }
            }},
                    new Person { FirstName = "Lane", LastName = "Tina", Id = 3, Movies = new List<MoviePerson>
            {
                new MoviePerson { PersonType = PersonType.Director }
            }}
        };

        _uowMock.Setup(u => u.Repository.GetAll())
                .Returns(people.AsQueryable());

        var result = _personService.GetDirectors();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public void FilterEntities_WithSearchTerm_ShouldFilterGenres()
    {
        var people = new List<Person>
        {
            new Person { FirstName = "John", LastName = "Doe" },
            new Person { FirstName = "Jane", LastName = "Smith" },
            new Person { FirstName = "Chris", LastName = "Nolan" }
        }.AsQueryable();

        var request = new DataTablesRequestDto { SearchTerm = "John" };
        _uowMock.Setup(uow => uow.Repository.GetAll()).Returns(people);

        var result = _personService.FilterEntities(request);

        Assert.Single(result);
        Assert.Equal("John", result.First().FirstName);
        _uowMock.Verify(uow => uow.Repository.GetAll(), Times.Once);
    }
}
