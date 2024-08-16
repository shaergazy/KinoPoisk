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

namespace Tests;
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
            cfg.CreateMap<Person, GetPersonDto>().ReverseMap();
            cfg.CreateMap<Person, AddPersonDto>().ReverseMap();
            cfg.CreateMap<Person, EditPersonDto>().ReverseMap();
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
    public void FilterEntities_WithSearchTerm_ShouldFilterPeople()
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

    [Fact]
    public async Task SearchAsync_ShouldReturnFilteredAndPagedData()
    {
        var people = new List<Person>
        {
            new Person { FirstName = "John", LastName = "Doe", Id = 1 },
            new Person { FirstName = "Jane", LastName = "Smith", Id = 2 },
            new Person { FirstName = "Chris", LastName = "Nolan", Id = 3 }
        }.AsQueryable();

        var request = new DataTablesRequestDto { Start = 0, Length = 2, SearchTerm = "John" };

        _uowMock.Setup(uow => uow.Repository.GetAll()).Returns(people);

        var result = await _personService.SearchAsync(request);

        Assert.NotNull(result);
        Assert.Equal(1, result.RecordsFiltered);
        Assert.Single(result.Data);
        Assert.Equal("John", result.Data.First().FirstName);
        _uowMock.Verify(uow => uow.Repository.GetAll(), Times.Once);
    }

    [Fact]
    public async Task GetPagedData_ShouldReturnCorrectPage()
    {
        var people = new List<Person>
        {
            new Person { FirstName = "John", LastName = "Doe" },
            new Person { FirstName = "Jane", LastName = "Smith" },
            new Person { FirstName = "Chris", LastName = "Nolan" }
        }.AsQueryable();

        var request = new DataTablesRequestDto { Start = 1, Length = 2 };

        _uowMock.Setup(uow => uow.Repository.GetAll()).Returns(people);

        var result = await _personService.GetPagedData(request, people);

        Assert.Equal(2, result.Count);
        Assert.Equal("Jane", result[0].FirstName);
        Assert.Equal("Chris", result[1].FirstName);
    }

    [Fact]
    public void OrderByColumn_ShouldOrderByGivenColumn()
    {
        var people = new List<Person>
        {
            new Person { FirstName = "John", LastName = "Doe" },
            new Person { FirstName = "Jane", LastName = "Smith" },
            new Person { FirstName = "Chris", LastName = "Nolan" }
        }.AsQueryable();

        var request = new DataTablesRequestDto { SortColumn = "FirstName", SortDirection = "asc" };

        var result = _personService.OrderByColumn(people, request);

        Assert.Equal("Chris", result.First().FirstName);
        Assert.Equal("John", result.Last().FirstName);
    }

    [Fact]
    public void FilterEntities_ShouldFilterBySearchTerm()
    {
        var people = new List<Person>
        {
            new Person { FirstName = "John", LastName = "Doe" },
            new Person { FirstName = "Jane", LastName = "Smith" }
        }.AsQueryable();

        var request = new DataTablesRequestDto { SearchTerm = "John" };

        var result = _personService.FilterEntities(request, people);

        Assert.Single(result);
        Assert.Equal("John", result.First().FirstName);
    }

    [Fact]
    public void GetAll_ShouldReturnMappedListOfPeople()
    {
        var people = new List<Person>
        {
            new Person { FirstName = "John", LastName = "Doe" },
            new Person { FirstName = "Jane", LastName = "Smith" }
        };

        _uowMock.Setup(uow => uow.Repository.GetAll()).Returns(people.AsQueryable());

        var result = _personService.GetAll();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _uowMock.Verify(uow => uow.Repository.GetAll(), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnMappedPerson()
    {
        var person = new Person { FirstName = "John", LastName = "Doe", Id = 1 };

        _uowMock.Setup(uow => uow.Repository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(person);

        var result = await _personService.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal("John", result.FirstName);
        _uowMock.Verify(uow => uow.Repository.GetByIdAsync(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdatePerson()
    {
        var editDto = new EditPersonDto { Id = 1, FirstName = "John", LastName = "UpdatedDoe" };
        var person = new Person { FirstName = "John", LastName = "Doe", Id = 1 };

        _uowMock.Setup(uow => uow.Repository.GetByIdAsync(1)).ReturnsAsync(person);
        _uowMock.Setup(uow => uow.Repository.UpdateAsync(It.IsAny<Person>()));
        _uowMock.Setup(uow => uow.SaveChangesAsync()).Returns(Task.FromResult(1));

        await _personService.UpdateAsync(editDto);

        _uowMock.Verify(uow => uow.Repository.UpdateAsync(It.IsAny<Person>()), Times.Once);
        _uowMock.Verify(uow => uow.SaveChangesAsync(), Times.Once);
    }

}
