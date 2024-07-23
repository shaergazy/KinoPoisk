﻿using Newtonsoft.Json;

namespace BLL.DTO.Movie
{
    public class SearchItem
    {
        [JsonProperty("Title")]
        public string Title { get; set; }

        [JsonProperty("Year")]
        public string Year { get; set; }

        [JsonProperty("imdbID")]
        public string ImdbId { get; set; }

        [JsonProperty("Type")]
        public string Type { get; set; }

        [JsonProperty("Poster")]
        public string Poster { get; set; }
    }

    public class SearchList
    {
        [JsonProperty("Search")]
        public List<SearchItem> SearchResults { get; set; }
        
        [JsonProperty("totalResults")]
        public string TotalResults { get; set; }
        
        [JsonProperty("Response")]
        public string Response { get; set; }
        
        [JsonProperty("Error")]
        public string Error { get; set; }
    }
}
