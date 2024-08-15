using AutoMapper;
using BLL.DTO;
using BLL.Services.Implementation;
using DAL.Models;
using Data.Repositories.RepositoryInterfaces;
using Microsoft.Extensions.Logging;
using Moq;

public class GenreServiceTests
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IUnitOfWork<Genre, int>> _unitOfWorkMock;
    private readonly Mock<ILogger<GenreService>> _loggerMock;
    private readonly GenreService _genreService;

    public GenreServiceTests()
    {
        _mapperMock = new Mock<IMapper>();
        _unitOfWorkMock = new Mock<IUnitOfWork<Genre, int>>();
        _loggerMock = new Mock<ILogger<GenreService>>();

        _genreService = new GenreService(_mapperMock.Object, _unitOfWorkMock.Object, _loggerMock.Object);
    }

    [Fact]
    public void FilterEntities_WithSearchTerm_ShouldFilterGenres()
    {
        var genres = new List<Genre>
        {
            new Genre { Name = "Action" },
            new Genre { Name = "Comedy" },
            new Genre { Name = "Drama" }
        }.AsQueryable();

        var request = new DataTablesRequestDto { SearchTerm = "Action" };
        _unitOfWorkMock.Setup(uow => uow.Repository.GetAll()).Returns(genres);

        var result = _genreService.FilterEntities(request);

        Assert.Single(result);
        Assert.Equal("Action", result.First().Name);
        _unitOfWorkMock.Verify(uow => uow.Repository.GetAll(), Times.Once);
    }

    [Fact]
    public async Task ImportGenres_ShouldAddGenresToMovie()
    {
        var movie = new Movie { Title = "Sample Movie", Genres = new List<MovieGenre>() };
        var genreName = "Hay";
        var genre = new Genre { Name = genreName };

        _unitOfWorkMock.Setup(uow => uow.Genres.FirstOrDefaultAsync(g => g.Name == genreName)).ReturnsAsync((Genre)null);
        _unitOfWorkMock.Setup(uow => uow.Genres.AddAsync(It.IsAny<Genre>())).Returns((Genre genre) => Task.FromResult(genre));

        await _genreService.ImportGenres(genreName, movie);

        Assert.Single(movie.Genres);
        Assert.Equal(genreName, movie.Genres.First().Genre.Name);
        _unitOfWorkMock.Verify(uow => uow.Genres.AddAsync(It.IsAny<Genre>()), Times.Once);
    }

}
