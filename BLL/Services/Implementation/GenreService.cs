using AutoMapper;
using BLL.DTO;
using BLL.DTO.Genre;
using BLL.Services.Interfaces;
using DAL.Enums;
using DAL.Models;
using Data.Repositories.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Dynamic.Core;

namespace BLL.Services.Implementation
{
    public class GenreService : TranslatableService<ListGenreDto, AddGenreDto, EditGenreDto, GetGenreDto, Genre, int, DataTablesRequestDto>,
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
        public override List<ListGenreDto> GetAll ()
        {
            return _mapper.Map<List<ListGenreDto>>(_uow.Genres.GetAll().Include(x => x.Translations));
        }
        public override IQueryable<Genre> FilterEntities(DataTablesRequestDto request, IQueryable<Genre>? entities = null)
        {
            var searchTerm = request.SearchTerm;
            if (entities == null)
            {
                entities = _uow.Repository.GetAll()
                              .Include(g => g.Translations);
                _logger.LogInformation("Retrieved all genres with translations from the repository.");
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                entities = entities.Where(s => s.Translations.Any(t =>
                    t.FieldType == TranslatableFieldType.Name &&
                    t.LanguageCode == request.LanguageCode &&
                    t.Value.ToUpper().Contains(searchTerm.ToUpper())));
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
                    var genre = await _uow.Genres
                        .Include(g => g.Translations)
                        .FirstOrDefaultAsync(g => g.Translations.Any(t =>
                            t.FieldType == TranslatableFieldType.Name &&
                            t.LanguageCode == LanguageCode.en &&
                            t.Value == genreName));

                    if (genre == null)
                    {
                        genre = new Genre();
                        genre.Translations.Add(new TranslatableEntityField
                        {
                            FieldType = TranslatableFieldType.Name,
                            LanguageCode = LanguageCode.en,
                            Value = genreName
                        });
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
