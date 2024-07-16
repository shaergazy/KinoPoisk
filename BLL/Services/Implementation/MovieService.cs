using AutoMapper;
using BLL.DTO;
using BLL.DTO.Genre;
using BLL.DTO.Movie;
using BLL.DTO.Person;
using BLL.Services.Interfaces;
using DAL.Models;
using Data.Models;
using Data.Repositories.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using Repositories;

namespace BLL.Services.Implementation
{
    public class MovieService : SearchableService<ListMovieDto, AddMovieDto, EditMovieDto, GetMovieDto, Movie, Guid>, IMovieService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<Movie, Guid> _uow;

        public MovieService(IMapper mapper, IUnitOfWork<Movie, Guid> unitOfWork) : base(mapper, unitOfWork)
        {
            _mapper = mapper;
            _uow = unitOfWork;
        }

        public async Task<int> AddCommentAsync(AddCommentDo dto)
        {
            var comment = _mapper.Map<Comment>(dto);
            await _uow.Comments.AddAsync(comment);
            return comment.Id;
        }

        public override async Task<DataTablesResponse<Movie>> SearchAsync(DataTablesRequestDto request)
        {
            var entities = _uow.Repository.GetAll(); ;

            var recordsTotal = entities.Count();

            entities = FilterEntities(request.SearchTerm?.ToUpper(), entities);

            var recordsFiltered = entities.Count();

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

        public virtual async Task<GetMovieDto> GetByIdAsync(Guid id)
        {
            var entity = await _uow.Repository.GetAll()
            .Include(m => m.Country)
            .Include(m => m.Genres)
            .Include(m => m.Comments)
            .Include(m => m.People)
            .FirstOrDefaultAsync(x => x.Id == id);

            return _mapper.Map<GetMovieDto>(entity);
        }

        public async Task<int> AddRatingAsync(AddMovieRating dto)
        {
            var rating = _mapper.Map<MovieRating>(dto);
            await _uow.Ratings.AddAsync(rating);
            return rating.Id;
        }

        public override IQueryable<Movie> FilterEntities(string searchTerm, IQueryable<Movie> entities = null)
        {
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                entities = entities.Where(s =>
                    s.Title.ToUpper().Contains(searchTerm));
            }
            return entities;
        }

        public async Task DeleteCommentAsync(int commentId)
        {
            await _uow.Comments.DeleteByIdAsync(commentId);
        }

        public Task<IEnumerable<Comment>> GetCommentsAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public override async Task<Movie> BuildEntityForCreate(AddMovieDto dto)
        {
            if (dto.Poster == null || dto.Title == null)
                throw new ArgumentNullException(nameof(dto), "You have to complete all properties");

            var movie = _mapper.Map<Movie>(dto);
            movie.Poster = await SaveFileAsync(dto.Poster);

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

        public async Task<IEnumerable<ListMovieDto>> GetTopRatedMoviesAsync(int count)
        {
            var movies = await _uow.Movies.GetAll()
                .Include(x => x.People)
                .ThenInclude(m => m.Person)
                .Select(movie => new
                {
                    Movie = movie,
                    AverageRating = movie.Ratings.Average(r => (int?)r.StarCount) ?? 0
                })
                .OrderByDescending(x => x.AverageRating)
                .Select(x => x.Movie)
                .Take(count)
                .ToListAsync();

            return _mapper.Map<List<ListMovieDto>>(movies);
        }
    }
}
