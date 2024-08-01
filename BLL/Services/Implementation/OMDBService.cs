using BLL.DTO.Movie;
using BLL.Services.Interfaces;
using BLL.Utilities;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace BLL.Services.Implementation
{
    public class OMDBService
    {
        private const string BaseUrl = "http://www.omdbapi.com/?";
        private readonly string _apikey;
        private readonly bool _rottenTomatoesRatings;

        public OMDBService(string apikey, bool rottenTomatoesRatings = false)
        {
            _apikey = apikey;
            _rottenTomatoesRatings = rottenTomatoesRatings;
        }

        public ExternalMovieDto GetItemByTitle(string title, bool fullPlot = false)
        {
            return GetItemByTitle(title, null, fullPlot);
        }

        public ExternalMovieDto GetItemByTitle(string title, int? year, bool fullPlot = false)
        {
            var query = QueryBuilder.GetItemByTitleQuery(title, year, fullPlot);

            var item = GetOmdbDataAsync<ExternalMovieDto>(query).Result;

            if (item.Response.Equals("False"))
            {
                throw new HttpRequestException(item.Error);
            }

            return item;
        }

        public ExternalMovieDto GetItemById(string id, bool fullPlot = false)
        {
            var query = QueryBuilder.GetItemByIdQuery(id, fullPlot);

            var item = GetOmdbDataAsync<ExternalMovieDto>(query).Result;

            if (item.Response.Equals("False"))
            {
                throw new HttpRequestException(item.Error);
            }
            return item;
        }

        public SearchList GetSearchList(string query, int page = 1)
        {
            return GetSearchList(null, query, page);
        }

        public SearchList GetSearchList(int? year, string query, int page = 1)
        {
            var editedQuery = QueryBuilder.GetSearchListQuery(year, query, page);

            var searchList = GetOmdbDataAsync<SearchList>(editedQuery).Result;

            if (searchList.Response.Equals("False"))
            {
                throw new HttpRequestException(searchList.Error);
            }

            return searchList;
        }

        public async Task<T> GetOmdbDataAsync<T>(string query)
        {
            using (var client = new HttpClient { BaseAddress = new Uri(BaseUrl) })
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client
                    .GetAsync($"{BaseUrl}apikey={_apikey}{query}&tomatoes={_rottenTomatoesRatings}")
                    .ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    return default;
                }

                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings
                {
                    MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
                    DateParseHandling = DateParseHandling.None,
                    Error = delegate (object? sender, Newtonsoft.Json.Serialization.ErrorEventArgs args)
                    {
                        string message = args.ErrorContext.Error.Message;
                        var currentError = message;
                        args.ErrorContext.Handled = true;
                    }
                });
            }
        }
    }
}
