using AutoMapper;
using BLL.DTO;
using BLL.DTO.Movie;
using BLL.Services.Interfaces;
using DAL.Enums;
using DAL.Models;
using Data.Models;
using Data.Repositories.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using QuestPDF.Fluent;
using System.Globalization;

namespace BLL.Services.Implementation
{
    public class MovieService : SearchableService<ListMovieDto, AddMovieDto, EditMovieDto, GetMovieDto, Movie, Guid, MovieDataTablesRequestDto>, IMovieService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<Movie, Guid> _uow;
        private readonly ICountryService _countryService;
        private readonly IGenreService _genreService;
        private readonly IPersonService _personService;
        private readonly ILogger<MovieService> _logger;
        private readonly OMDBService _omdbService;

        public MovieService(IMapper mapper, ILogger<MovieService> logger, IUnitOfWork<Movie, Guid> unitOfWork,
            ICountryService countryService, IGenreService genreService, IPersonService personService, OMDBService omdbService)
            : base(mapper, unitOfWork, logger)
        {
            _mapper = mapper;
            _uow = unitOfWork;
            _countryService = countryService;
            _genreService = genreService;
            _personService = personService;
            _logger = logger;
            _omdbService = omdbService;
        }

        public async override Task DeleteAsync(Guid id)
        {
            _logger.LogInformation($"Attempting to delete movie with ID: {id}");

            try
            {
                var movie = await _uow.Movies
                    .Include(m => m.Genres)
                    .Include(m => m.Comments)
                    .Include(m => m.People)
                    .Include(m => m.Ratings)
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (movie == null)
                {
                    _logger.LogWarning($"Movie with ID: {id} not found.");
                    return;
                }

                _logger.LogInformation($"Deleting related entities for movie with ID: {id}");

                _uow.MovieGenres.RemoveRange(movie.Genres);
                _uow.Comments.RemoveRange(movie.Comments);
                _uow.MoviePerson.RemoveRange(movie.People);
                _uow.Ratings.RemoveRange(movie.Ratings);

                _logger.LogInformation($"Deleting movie with ID: {id}");
                await _uow.Movies.Remove(movie);

                await _uow.SaveChangesAsync();

                _logger.LogInformation($"Successfully deleted movie with ID: {id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting movie with ID: {id}");
                throw;
            }
        }

        public async Task<byte[]> GeneratePdfAsync(MovieDataTablesRequestDto dto)
        {
            var response = await SearchAsync(dto);
            var movies = response.Data;

            var document = new ListMoviePdfDocument(movies.ToList());
            using (var ms = new MemoryStream())
            {
                document.GeneratePdf(ms);
                return ms.ToArray();
            }
        }

        public async Task<byte[]> GenerateExcelAsync(MovieDataTablesRequestDto dto)
        {
            var response = await SearchAsync(dto);
            var movies = response.Data;

            using (var ms = new MemoryStream())
            {
                using (var package = new ExcelPackage(ms))
                {
                    var worksheet = package.Workbook.Worksheets.Add("Movies");

                    // Headers
                    worksheet.Cells[1, 1].Value = "Title";
                    worksheet.Cells[1, 2].Value = "Description";
                    worksheet.Cells[1, 3].Value = "Released Date";
                    worksheet.Cells[1, 4].Value = "Duration";
                    worksheet.Cells[1, 5].Value = "IMDB Rating";
                    worksheet.Cells[1, 6].Value = "Rating";

                    // Content
                    for (int i = 0; i < movies.Count; i++)
                    {
                        var movie = movies[i];
                        worksheet.Cells[i + 2, 1].Value = movie.Title;
                        worksheet.Cells[i + 2, 2].Value = movie.Description;
                        worksheet.Cells[i + 2, 3].Value = movie.ReleasedDate.ToString("dd-MM-yyyy");
                        worksheet.Cells[i + 2, 4].Value = movie.Duration;
                        worksheet.Cells[i + 2, 5].Value = movie.IMDBRating;
                        worksheet.Cells[i + 2, 6].Value = movie.Rating;
                    }
                    package.Save();
                }
                return ms.ToArray();
            }
        }

        public async Task AddCommentAsync(AddCommentDto dto)
        {
            var comment = _mapper.Map<Comment>(dto);
            if (comment == null)
            {
                _logger.LogWarning("Attempted to add a comment with null value.");
                throw new ArgumentNullException(nameof(comment));
            }

            comment.Date = DateTime.Now;
            await _uow.Comments.AddAsync(comment);
            await _uow.SaveChangesAsync();
            _logger.LogInformation("Added comment for movie with ID {MovieId}.", comment.MovieId);
        }

        public override async Task<DataTablesResponse<Movie>> SearchAsync(MovieDataTablesRequestDto request)
        {
            var entities = _uow.Repository.GetAll();
            _logger.LogInformation("Retrieved all movies for search.");

            var recordsTotal = entities.Count();

            entities = FilterEntities(request, entities);

            int recordsFiltered = entities.Count();

            entities = OrderByColumn(entities, request);

            var data = await GetPagedData(request, entities);

            _logger.LogInformation("Returning {Count} movies after filtering.", data.Count);

            var datatableResponse = new DataTablesResponse<Movie>()
            {
                Draw = request.Draw,
                RecordsTotal = recordsTotal,
                RecordsFiltered = recordsFiltered,
                Data = data
            };

            return datatableResponse;
        }

        public override async Task<GetMovieDto> GetByIdAsync(Guid id)
        {
            var entity = await _uow.Repository.GetAll()
            .Include(m => m.Country)
            .Include(m => m.Genres)
                .ThenInclude(x => x.Genre)
            .Include(m => m.Ratings)
            .Include(m => m.Comments)
            .Include(m => m.People)
                .ThenInclude(mp => mp.Person)
            .FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
            {
                _logger.LogWarning("Movie with ID {MovieId} not found.", id);
                return null;
            }

            _logger.LogInformation("Retrieved movie with ID {MovieId}.", id);
            return _mapper.Map<GetMovieDto>(entity);
        }

        public async Task AddRatingAsync(AddMovieRating dto)
        {
            var existinRating = await _uow.Ratings.FirstOrDefaultAsync(x => x.MovieId == dto.MovieId && x.UserId == dto.UserId);

            if (existinRating != null)
            {
                existinRating.StarCount = dto.StarCount;
                _logger.LogInformation("Updated rating for movie {MovieId} by user {UserId}.", dto.MovieId, dto.UserId);
            }
            else
            {
                var rating = _mapper.Map<MovieRating>(dto);
                await _uow.Ratings.AddAsync(rating);
                _logger.LogInformation("Added new rating for movie {MovieId} by user {UserId}.", dto.MovieId, dto.UserId);
            }

            await _uow.SaveChangesAsync();

            var averageRating = await _uow.Ratings
                .Where(r => r.MovieId == dto.MovieId)
                .AverageAsync(r => r.StarCount);

            var movie = await _uow.Movies.FirstOrDefaultAsync(m => m.Id == dto.MovieId);
            if (movie != null)
            {
                movie.Rating = (float)averageRating;
                await _uow.SaveChangesAsync();
                _logger.LogInformation("Updated average rating for movie {MovieId} to {Rating}.", dto.MovieId, averageRating);
            }
        }

        public override IQueryable<Movie> FilterEntities(MovieDataTablesRequestDto request, IQueryable<Movie> entities = null)
        {
            var searchTerm = request.SearchTerm?.ToUpper();
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                entities = entities.Where(s => s.Title.ToUpper().Contains(searchTerm));
                _logger.LogInformation("Filtered movies by search term: {SearchTerm}.", searchTerm);
            }

            if (!string.IsNullOrEmpty(request.Title))
            {
                entities = entities.Where(m => m.Title.Contains(request.Title));
                _logger.LogInformation("Filtered movies by title: {Title}.", request.Title);
            }

            if (request.Year.HasValue)
            {
                entities = entities.Where(m => m.ReleasedDate.Year == request.Year);
                _logger.LogInformation("Filtered movies by year: {Year}.", request.Year);
            }

            if (request.Country.HasValue)
            {
                entities = entities.Where(m => m.CountryId == request.Country);
                _logger.LogInformation("Filtered movies by country ID: {CountryId}.", request.Country);
            }

            if (request.Actor.HasValue)
            {
                entities = entities.Where(m => m.People.Any(p => p.PersonType == PersonType.Actor && p.PersonId == request.Actor));
                _logger.LogInformation("Filtered movies by actor ID: {ActorId}.", request.Actor);
            }

            if (request.Director.HasValue)
            {
                entities = entities.Where(m => m.People.Any(p => p.PersonType == PersonType.Director && p.PersonId == request.Director));
                _logger.LogInformation("Filtered movies by director ID: {DirectorId}.", request.Director);
            }

            return entities;
        }

        public async Task DeleteCommentAsync(int commentId)
        {
            try
            {
                await _uow.Comments.DeleteByIdAsync(commentId);
                _logger.LogInformation("Deleted comment with ID {CommentId}.", commentId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting comment with ID {CommentId}.", commentId);
                throw;
            }
        }

        public async Task<DataTablesResponse<GetCommentDto>> GetCommentsAsync(Guid id, DataTablesRequestDto request)
        {
            var comments = await _uow.Comments
                             .Where(c => c.MovieId == id)
                             .Include(x => x.User)
                             .OrderByDescending(c => c.Date)
                             .ToListAsync();

            var recordsTotal = comments.Count;
            comments = comments.Skip(request.Start).Take(request.Length).ToList();
            var recordsFiltered = comments.Count;
            var data = _mapper.Map<List<GetCommentDto>>(comments);

            var datatableResponse = new DataTablesResponse<GetCommentDto>()
            {
                Draw = request.Draw,
                RecordsTotal = recordsTotal,
                RecordsFiltered = recordsFiltered,
                Data = data
            };

            _logger.LogInformation("Retrieved {Count} comments for movie with ID {MovieId}.", data.Count, id);
            return datatableResponse;
        }

        public override async Task<Movie> BuildEntityForCreate(AddMovieDto dto)
        {
            if (dto.Poster == null || dto.Title == null)
            {
                _logger.LogWarning("Attempted to create a movie with incomplete data.");
                throw new ArgumentNullException(nameof(dto), "You have to complete all properties");
            }

            var movie = _mapper.Map<Movie>(dto);
            movie.Poster = await SaveFileAsync(dto.Poster);

            var country = await _uow.Countries.FirstOrDefaultAsync(x => x.Id == dto.CountryId);
            if (country != null)
            {
                movie.Country = country;
                _logger.LogInformation("Assigned country with ID {CountryId} to movie.", dto.CountryId);
            }

            var genres = await _uow.Genres.GetAll()
                            .Where(g => dto.GenreIds.Contains(g.Id))
                            .ToListAsync();
            movie.Genres = genres.Select(g => new MovieGenre { Movie = movie, Genre = g }).ToList();
            _logger.LogInformation("Assigned genres to movie.");

            movie.People = new List<MoviePerson>
            {
                new MoviePerson { Movie = movie, PersonId = dto.DirectorId, PersonType = DAL.Enums.PersonType.Director }
            };

            if (dto.Actors != null)
            {
                foreach (var actor in dto.Actors)
                {
                    movie.People.Add(new MoviePerson
                    {
                        Movie = movie,
                        PersonId = actor.PersonId,
                        PersonType = DAL.Enums.PersonType.Actor,
                        Order = actor.Order,
                    });
                }
                _logger.LogInformation("Assigned actors to movie.");
            }

            return movie;
        }

        public override async Task<Movie> BuildEntityForDelete(Guid id)
        {
            var movie = await _uow.Repository.GetByIdAsync(id);
            try
            {
                File.Delete(movie.Poster);
                _logger.LogInformation("Deleted poster file for movie with ID {MovieId}.", id);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to delete poster file for movie with ID {MovieId}.", id);
            }
            return movie;
        }

        public override async Task<Movie> BuildEntityForUpdate(EditMovieDto dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("Attempted to update a movie with null DTO.");
                throw new ArgumentNullException(nameof(dto));
            }

            var movieToUpdate = await _uow.Repository.GetByIdAsync(dto.Id);
            if (movieToUpdate == null)
            {
                _logger.LogWarning("Movie with ID {MovieId} not found for update.", dto.Id);
                throw new Exception($"Movie with id {dto.Id} doesn't exist");
            }

            var country = await _uow.Countries.FirstOrDefaultAsync(x => x.Id == dto.CountryId);
            if (country != null)
            {
                movieToUpdate.Country = country;
                _logger.LogInformation("Updated country for movie with ID {MovieId}.", dto.Id);
            }

            movieToUpdate.Title = dto.Title;
            movieToUpdate.Description = dto.Description;
            movieToUpdate.ReleasedDate = dto.ReleasedDate;

            if (dto.Poster != null)
            {
                if (!string.IsNullOrEmpty(movieToUpdate.Poster))
                {
                    File.Delete(movieToUpdate.Poster);
                    _logger.LogInformation("Deleted old poster for movie with ID {MovieId}.", dto.Id);
                }

                movieToUpdate.Poster = await SaveFileAsync(dto.Poster);
                _logger.LogInformation("Updated poster for movie with ID {MovieId}.", dto.Id);
            }

            return movieToUpdate;
        }

        public async Task<IEnumerable<ListMovieDto>> GetNewestMoviesAsync(int count)
        {
            var movies = await _uow.Movies.GetAll()
                .Include(x => x.People)
                    .ThenInclude(m => m.Person)
                .OrderByDescending(x => x.ReleasedDate)
                .Take(count)
                .ToListAsync();

            _logger.LogInformation("Retrieved {Count} newest movies.", count);
            return _mapper.Map<List<ListMovieDto>>(movies);
        }

        public async Task ImportMovieAsync(ExternalMovieDto dto)
        {
            var movie = new Movie
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                Description = dto.Plot,
                ReleasedDate = DateTime.Parse(dto.Released),
                Poster = dto.Poster,
                Duration = ParseDuration(dto.Runtime),
                Genres = new List<MovieGenre>(),
                People = new List<MoviePerson>()
            };

            if (float.TryParse(dto.ImdbRating, NumberStyles.Float, CultureInfo.InvariantCulture, out float imdbRating))
            {
                movie.IMDBRating = imdbRating;
            }

            await _uow.Movies.AddAsync(movie);

            await _countryService.ImportCountry(dto.Country, movie);
            await _personService.ImportPeopleAsync(dto.Actors, dto.Director, movie);
            await _genreService.ImportGenres(dto.Genre, movie);

            await _uow.SaveChangesAsync();
            _logger.LogInformation("Imported movie with title {Title} from external source.", dto.Title);
        }

        private uint ParseDuration(string runtime)
        {
            var parts = runtime.Split(" ");
            return uint.TryParse(parts[0], out var duration) ? duration : 0;
        }

        public async Task<IEnumerable<ListMovieDto>> GetTopRatedMoviesAsync(int count)
        {
            var movies = await _uow.Movies.GetAll()
                .Include(x => x.People)
                .ThenInclude(m => m.Person)
                .OrderByDescending(x => x.Rating)
                .Take(count)
                .ToListAsync();

            _logger.LogInformation("Retrieved {Count} top rated movies.", count);
            return _mapper.Map<List<ListMovieDto>>(movies);
        }

        public IQueryable<Movie> SortByParametrs(IQueryable<Movie> entities, MovieDataTablesRequestDto request)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateImdbRatings()
        {
            var movies = _uow.Movies.GetAll();

            foreach (var movie in movies)
            {
                var updatedMovie = _omdbService.GetItemByTitle(movie.Title);

                if (float.TryParse(updatedMovie.ImdbRating, NumberStyles.Float, CultureInfo.InvariantCulture, out float imdbRating))
                {
                    if (movie.IMDBRating != imdbRating)
                    {
                        movie.IMDBRating = imdbRating;
                        await _uow.Movies.UpdateAsync(movie);
                        _logger.LogInformation("Updated IMDB rating for movie with ID {MovieId} to {ImdbRating}.", movie.Id, imdbRating);
                    }
                }
            }

            await _uow.SaveChangesAsync();
        }
    }
}
