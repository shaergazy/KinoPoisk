using BLL.DTO.Movie;
using BLL.Services.Implementation;
using Microsoft.Extensions.Logging;
using Moq;
namespace Tests
{
    public class OMDBServiceTests
    {
        private readonly Mock<ILogger<OMDBService>> _loggerMock;
        private readonly OMDBService _service;
        private readonly HttpClient _httpClient;

        public OMDBServiceTests()
        {
            _loggerMock = new Mock<ILogger<OMDBService>>();

            _service = new OMDBService("fc8e73bf", _loggerMock.Object, false);
        }

        [Fact]
        public async Task GetItemByTitle_ShouldReturnMovie()
        {
            var title = "Inception";

            var result =  _service.GetItemByTitle(title);

            Assert.NotNull(result);
            Assert.Contains("Inception", result.Title);
        }

        [Fact]
        public async Task GetSearchList_ShouldReturnSearchList()
        {
            var query = "Inception";
            var jsonResponse = new SearchList { Response = "True", SearchResults = new List<SearchItem> { new SearchItem { Title = "Inception" } } };

            var result =  _service.GetSearchList(query);

            Assert.NotNull(result);
            Assert.True(result.Response == "True");
            Assert.Equal(10, result.SearchResults.Count);
            Assert.Equal("Inception", result.SearchResults[0].Title);
        }

        [Fact]
        public async Task GetOmdbDataAsync_ShouldReturnSerachList()
        {
            var query = "&s=Avengers&page=1&type=movie";

            var result = await _service.GetOmdbDataAsync<SearchList>(query);
            
            Assert.NotNull(result);
            Assert.True(0 < result.SearchResults.Count);
            Assert.Contains("Avengers", result.SearchResults[0].Title);
        }
    }

}
