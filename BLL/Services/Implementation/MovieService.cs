using AutoMapper;
using BLL.DTO;
using BLL.DTO.Movie;
using BLL.Services.Interfaces;
using DAL.Enums;
using DAL.Models;
using Data.Models;
using Data.Repositories.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services.Implementation
{
    public class MovieService : SearchableService<ListMovieDto, AddMovieDto, EditMovieDto, GetMovieDto, Movie, Guid, MovieDataTablesRequestDto>, IMovieService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<Movie, Guid> _uow;
        private readonly ICountryService _countryService;
        private readonly IGenreService _genreService;
        private readonly IPersonService _personService;

        public MovieService(IMapper mapper, IUnitOfWork<Movie, Guid> unitOfWork, ICountryService countryService, IGenreService genreService, IPersonService personService) : base(mapper, unitOfWork)
        {
            _mapper = mapper;
            _uow = unitOfWork;
            _countryService = countryService;
            _genreService = genreService;
            _personService = personService;
        }

        public async Task AddCommentAsync(AddCommentDo dto)
        {
            var comment = _mapper.Map<Comment>(dto);
            if (comment == null ) 
                throw new ArgumentNullException(nameof(comment));

            comment.Date = DateTime.Now;
            await _uow.Comments.AddAsync(comment);
            await _uow.SaveChangesAsync();
        }

        public override async Task<DataTablesResponse<Movie>> SearchAsync(MovieDataTablesRequestDto request)
        {
            var entities = _uow.Repository.GetAll(); 

            var recordsTotal = entities.Count();

            entities = FilterEntities(request, entities);

            int recordsFiltered = entities.Count();

            entities = OrderByColumn(entities, request);

            var data = await GetPagedData(request, entities);

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

            return _mapper.Map<GetMovieDto>(entity);
        }

        public async Task AddRatingAsync(AddMovieRating dto)
        {
            var existinRating = await _uow.Ratings.FirstOrDefaultAsync(x => x.MovieId == dto.MovieId && x.UserId == dto.UserId);

            if (existinRating != null)
            {
                existinRating.StarCount = dto.StarCount;
            }
            else
            {
                var rating = _mapper.Map<MovieRating>(dto);
                await _uow.Ratings.AddAsync(rating);
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
            }
        }


        public override IQueryable<Movie> FilterEntities(MovieDataTablesRequestDto request, IQueryable<Movie> entities = null)
        {
            var searchTerm = request.SearchTerm?.ToUpper();
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
                entities = entities.Where(s => s.Title.ToUpper().Contains(searchTerm));

            if (!string.IsNullOrEmpty(request.Title))
                entities = entities.Where(m => m.Title.Contains(request.Title));

            if (request.Year.HasValue)
                entities = entities.Where(m => m.ReleasedDate.Year == request.Year);

            if (request.Country.HasValue)
                entities = entities.Where(m => m.CountryId == request.Country);

            if (request.Actor.HasValue)
            {
                entities = entities.Where(m => m.People.Any(p => p.PersonType == PersonType.Actor && p.PersonId == request.Actor));
            }

            if (request.Director.HasValue)
            {
                entities = entities.Where(m => m.People.Any(p => p.PersonType == PersonType.Director && p.PersonId == request.Director));
            }

            return entities;
        }


        public async Task DeleteCommentAsync(int commentId)
        {
            await _uow.Comments.DeleteByIdAsync(commentId);
        }

        public async Task<IEnumerable<GetCommentDto>> GetCommentsAsync(Guid movieId)
            {
            try
            {
                var comments = await _uow.Comments
                             .Where(c => c.MovieId == movieId)
                             .Include(x => x.User)
                             .OrderByDescending(c => c.Date)
                             .ToListAsync();
                var d = _mapper.Map<List<GetCommentDto>>(comments);
                return _mapper.Map<List<GetCommentDto>>(comments);
            }
            catch (Exception)
            {

                throw;
            }
            
           
        }

        public override async Task<Movie> BuildEntityForCreate(AddMovieDto dto)
        {
            if (dto.Poster == null || dto.Title == null)
                throw new ArgumentNullException(nameof(dto), "You have to complete all properties");

            var movie = _mapper.Map<Movie>(dto);
            movie.Poster = await   SaveFileAsync(dto.Poster);

            var country = await _uow.Countries.FirstOrDefaultAsync(x => x.Id == dto.CountryId);
            if (country != null)
                movie.Country = country;

            var genres = await _uow.Genres.GetAll()
                            .Where(g => dto.GenreIds.Contains(g.Id))
                            .ToListAsync();
            movie.Genres = genres.Select(g => new MovieGenre { Movie = movie, Genre = g }).ToList();

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
            }

            return movie;
        }

        public override async Task<Movie> BuildEntityForDelete(Guid id)
        {
            var movie = await _uow.Repository.GetByIdAsync(id);
            try
            {
                File.Delete(movie.Poster);
            }
            catch (Exception)
            {
                // TODO: Add logging
            }
            return movie;
        }

        public override async Task<Movie> BuildEntityForUpdate(EditMovieDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var movieToUpdate = await _uow.Repository.GetByIdAsync(dto.Id);
            if (movieToUpdate == null)
                throw new Exception($"Movie with id {dto.Id} doesn't exist");

            var country = await _uow.Countries.FirstOrDefaultAsync(x => x.Id == dto.CountryId);
            if (country != null)
                movieToUpdate.Country = country;

            movieToUpdate.Title = dto.Title;
            movieToUpdate.Description = dto.Description;
            movieToUpdate.ReleasedDate = dto.ReleasedDate;

            if (dto.Poster != null)
            {
                if (!string.IsNullOrEmpty(movieToUpdate.Poster))
                {
                    File.Delete(movieToUpdate.Poster);
                }

                movieToUpdate.Poster = await SaveFileAsync(dto.Poster);
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

            return _mapper.Map<List<ListMovieDto>>(movies);
        }

        public async Task ImportMovieAsync(Item dto)
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
            if (float.TryParse(dto.ImdbRating, out float imdbRating))
            {
                movie.IMDBRating = imdbRating;
            }

            await _uow.Movies.AddAsync(movie);

            await _countryService.ImportCountry(dto.Country, movie);

            await _personService.ImportPeopleAsync(dto.Actors, dto.Director, movie);

            await _genreService.ImportGenres(dto.Genre, movie);

            await _uow.SaveChangesAsync();
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

            return _mapper.Map<List<ListMovieDto>>(movies);
        }

        public IQueryable<Movie> SortByParametrs(IQueryable<Movie> entities, MovieDataTablesRequestDto request)
        {
            throw new NotImplementedException();
        }
    }
}
