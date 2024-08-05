using BLL.DTO.Movie;
using BLL.Utilities;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Microsoft.Extensions.Logging;

namespace BLL.Services.Implementation
{
    public class OMDBService
    {
        private const string BaseUrl = "http://www.omdbapi.com/?";
        private readonly string _apikey;
        private readonly bool _rottenTomatoesRatings;
        private readonly ILogger<OMDBService> _logger;

        public OMDBService(string apikey, ILogger<OMDBService> logger, bool rottenTomatoesRatings = false)
        {
            _apikey = apikey;
            _rottenTomatoesRatings = rottenTomatoesRatings;
            _logger = logger;
        }

        public ExternalMovieDto GetItemByTitle(string title, bool fullPlot = false)
        {
            return GetItemByTitle(title, null, fullPlot);
        }

        public ExternalMovieDto GetItemByTitle(string title, int? year, bool fullPlot = false)
        {
            try
            {
                _logger.LogInformation("Requesting movie details by title: {Title} and year: {Year}", title, year);
                var query = QueryBuilder.GetItemByTitleQuery(title, year, fullPlot);

                var item = GetOmdbDataAsync<ExternalMovieDto>(query).Result;

                if (item.Response.Equals("False"))
                {
                    _logger.LogWarning("OMDB API returned an error: {Error}", item.Error);
                    throw new HttpRequestException(item.Error);
                }

                _logger.LogInformation("Successfully retrieved movie details for title: {Title}", title);
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting movie details by title: {Title}", title);
                throw;
            }
        }

        public ExternalMovieDto GetItemById(string id, bool fullPlot = true)
        {
            try
            {
                _logger.LogInformation("Requesting movie details by ID: {Id}", id);
                var query = QueryBuilder.GetItemByIdQuery(id, fullPlot);

                var item = GetOmdbDataAsync<ExternalMovieDto>(query).Result;

                if (item.Response.Equals("False"))
                {
                    _logger.LogWarning("OMDB API returned an error: {Error}", item.Error);
                    throw new HttpRequestException(item.Error);
                }

                _logger.LogInformation("Successfully retrieved movie details for ID: {Id}", id);
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting movie details by ID: {Id}", id);
                throw;
            }
        }

        public SearchList GetSearchList(string query, int page = 1)
        {
            return GetSearchList(null, query, page);
        }

        public SearchList GetSearchList(int? year, string query, int page = 1)
        {
            try
            {
                _logger.LogInformation("Searching for movies with query: {Query}, year: {Year}, page: {Page}", query, year, page);
                var editedQuery = QueryBuilder.GetSearchListQuery(year, query, page);

                var searchList = GetOmdbDataAsync<SearchList>(editedQuery).Result;

                if (searchList.Response.Equals("False"))
                {
                    _logger.LogWarning("OMDB API returned an error during search: {Error}", searchList.Error);
                    throw new HttpRequestException(searchList.Error);
                }

                _logger.LogInformation("Successfully retrieved search results for query: {Query}", query);
                return searchList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while searching for movies with query: {Query}", query);
                throw;
            }
        }

        public async Task<T> GetOmdbDataAsync<T>(string query)
        {
            try
            {
                _logger.LogInformation("Sending request to OMDB API with query: {Query}", query);

                using (var client = new HttpClient { BaseAddress = new Uri(BaseUrl) })
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await client
                        .GetAsync($"{BaseUrl}apikey={_apikey}{query}&tomatoes={_rottenTomatoesRatings}")
                        .ConfigureAwait(false);

                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogWarning("OMDB API request failed with status code: {StatusCode}", response.StatusCode);
                        return default;
                    }

                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    _logger.LogInformation("Successfully retrieved response from OMDB API for query: {Query}", query);
                    
                    return JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings
                    {
                        MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
                        DateParseHandling = DateParseHandling.None,
                        Error = delegate (object? sender, Newtonsoft.Json.Serialization.ErrorEventArgs args)
                        {
                            _logger.LogError("Error deserializing response from OMDB API: {ErrorMessage}", args.ErrorContext.Error.Message);
                            args.ErrorContext.Handled = true;
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while sending request to OMDB API with query: {Query}", query);
                throw;
            }
        }
    }
}
