using AutoMapper;
using BLL.DTO;
using BLL.DTO.Country;
using BLL.Services.Implementation;
using DAL.Models;
using Data.Repositories.RepositoryInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace Tests;
public class CountryServiceTests
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IUnitOfWork<Country, int>> _uowMock;
    private readonly Mock<ILogger<CountryService>> _loggerMock;
    private readonly CountryService _countryService;

    public CountryServiceTests()
    {
        _mapperMock = new Mock<IMapper>();
        _uowMock = new Mock<IUnitOfWork<Country, int>>();
        _loggerMock = new Mock<ILogger<CountryService>>();
        _countryService = new CountryService(_mapperMock.Object, _uowMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task ImportCountry_Should_ImportNewCountry()
    {
        var movie = new Movie { Title = "Test Movie" };
        var countryName = "TestCountry";
        _uowMock.Setup(u => u.Countries.FirstOrDefaultAsync(It.IsAny<Expression<Func<Country, bool>>>())).ReturnsAsync((Country)null);
        _uowMock.Setup(u => u.Countries.AddAsync(It.IsAny<Country>())).Returns((Country country) => Task.FromResult(country));

        await _countryService.ImportCountry(countryName, movie);

        _uowMock.Verify(u => u.Countries.AddAsync(It.Is<Country>(c => c.Name == countryName)), Times.Once);
    }

    [Fact]
    public void FilterEntities_Should_FilterCountries()
    {
        var searchTerm = "Test";
        var countries = new List<Country>
        {
            new Country { Name = "TestCountry1" },
            new Country { Name = "OtherCountry" },
            new Country { Name = "TestCountry2" }
        }.AsQueryable();
        var request = new DataTablesRequestDto { SearchTerm = searchTerm };

        var result = _countryService.FilterEntities(request, countries);

        Assert.Equal(2, result.Count());
        Assert.All(result, c => Assert.Contains(searchTerm, c.Name));
    }

    [Fact]
    public async Task BuildEntityForCreate_Should_ThrowException_WhenCountryExists()
    {
        var stream = new MemoryStream();
        var dto = new AddCountryDto { Name = "ExistingCountry", Flag = new FormFile(stream,21, 20, "file", "fileName") };
        _uowMock.Setup(u => u.Repository.Any(It.IsAny<Expression<Func<Country, bool>>>())).Returns(true);

        await Assert.ThrowsAsync<Exception>(() => _countryService.BuildEntityForCreate(dto));
    }

    [Fact]
    public async Task BuildEntityForDelete_Should_ThrowException_WhenCountryHasMovies()
    {
        int countryId = 1;
        _uowMock.Setup(u => u.Movies.Any(It.IsAny<Expression<Func<Movie, bool>>>())).Returns(true);

        await Assert.ThrowsAsync<Exception>(() => _countryService.BuildEntityForDelete(countryId));
    }

    [Fact]
    public async Task BuildEntityForDelete_Should_ThrowException_WhenCountryDoesNotExist()
    {
        int countryId = 1;
        _uowMock.Setup(u => u.Repository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Country)null);

        await Assert.ThrowsAsync<NullReferenceException>(() => _countryService.BuildEntityForDelete(countryId));
    }

    [Fact]
    public async Task BuildEntityForUpdate_Should_UpdateCountry()
    {
        var dto = new EditCountryDto { Id = 1, Name = "UpdatedCountry" };
        var existingCountry = new Country { Id = 1, Name = "OldCountry", IsOwnPicture = false };
        _uowMock.Setup(u => u.Repository.GetByIdAsync(dto.Id)).ReturnsAsync(existingCountry);
        _uowMock.Setup(u => u.Repository.Any(It.IsAny<Expression<Func<Country, bool>>>())).Returns(false);

        var result = await _countryService.BuildEntityForUpdate(dto);

        Assert.Equal(dto.Name, result.Name);
    }
}
