using AutoMapper;
using BLL.DTO;
using BLL.DTO.Genre;
using BLL.Services.Interfaces;
using DAL.Models;
using Data.Repositories.RepositoryInterfaces;
using Microsoft.Extensions.Logging;
using System.Linq.Dynamic.Core;

namespace BLL.Services.Implementation
{
    public class GenreService : SearchableService<ListGenreDto, AddGenreDto, EditGenreDto, GetGenreDto, Genre, int, DataTablesRequestDto>,
        IGenreService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<Genre, int> _uow;
        private readonly ILogger<GenreService> _logger;

        public GenreService(IMapper mapper, IUnitOfWork<Genre, int> unitOfWork, ILogger<GenreService> logger) : base(mapper, unitOfWork, logger)
        {
            _mapper = mapper;
            _uow = unitOfWork;
            _logger = logger;
        }

        public override IQueryable<Genre> FilterEntities(DataTablesRequestDto request, IQueryable<Genre>? entities = null)
        {
            var searchTerm = request.SearchTerm;
            if (entities == null)
            {
                entities = _uow.Repository.GetAll();
                _logger.LogInformation("Retrieved all genres from the repository.");
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                entities = entities.Where(s =>
                    s.Name.ToUpper().Contains(searchTerm.ToUpper()));
                _logger.LogInformation("Filtered genres by search term: {SearchTerm}", searchTerm);
            }

            return entities;
        }

        public async Task ImportGenres(string genreNames, Movie movie)
        {
            var genres = genreNames.Split(", ");
            foreach (var genreName in genres)
            {
                try
                {
                    var genre = await _uow.Genres.FirstOrDefaultAsync(g => g.Name == genreName);
                    if (genre == null)
                    {
                        genre = new Genre { Name = genreName };
                        await _uow.Genres.AddAsync(genre);
                        _logger.LogInformation("Added new genre: {GenreName}", genreName);
                    }
                    movie.Genres.Add(new MovieGenre { Genre = genre });
                    _logger.LogInformation("Added genre {GenreName} to movie {MovieTitle}", genreName, movie.Title);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error importing genre {GenreName} for movie {MovieTitle}", genreName, movie.Title);
                }
            }
        }
    }
}
