using AutoMapper;
using BLL.DTO.Country;
using BLL.DTO.Genre;
using BLL.DTO.Movie;
using BLL.DTO.Person;
using BLL.Services.Interfaces;
using Common.Extensions;
using Common.Helpers;
using DAL.Models;
using Data.Models;
using Data.Repositories.RepositoryInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

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

        public async Task<int> AddRatingAsync(AddMovieRating dto)
        {
            var rating = _mapper.Map<MovieRating>(dto);
            await _uow.Ratings.AddAsync(rating);
            return rating.Id;
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

        public string GenerateUniqueFileName(IFormFile file)
        {
            return $"{Path.GetFileNameWithoutExtension(file.FileName)}_{DateTime.Now:yyyyMMddHHmmss}{Path.GetExtension(file.FileName)}";
        }

        public IEnumerable<ListCountryDto> GetCountries()
        {
            var countries = _uow.Countries.GetAll();
            return _mapper.Map<List<ListCountryDto>>(countries);
        }

        public IEnumerable<ListGenreDto> GetGenres()
        {
            var genres = _uow.Genres.GetAll();
            return _mapper.Map<List<ListGenreDto>>(genres);
        }

        public IEnumerable<ListPersonDto> GetPeople()
        {
            var people = _uow.People.GetAll().ToList();
            return _mapper.Map<List<ListPersonDto>>(people);
        }

        public async Task<string> SaveFileAsync(IFormFile file)
        {
            var fileName = GenerateUniqueFileName(file);
            var path = Path.Combine(AppConstants.BaseDir, AppConstants.PosterDir, fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return AppConstants.RelativeFilesPath.Combine(AppConstants.PosterDir, fileName);
        }

        public async Task<IEnumerable<ListMovieDto>> GetNewestMoviesAsync(int count)
        {
            var movies = await _uow.Movies.GetAll()
                .Include(x => x.People)
                .ThenInclude(m => m.Person)
                .OrderByDescending(x => x.ReleasedDate)
                .Take(count)
                .ToListAsync();

            foreach (var movie in movies)
            {
               movie.People = movie.People.OrderBy(p => p.Order).ToList();
            }

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

            foreach (var movie in movies)
            {
                movie.People.OrderBy(p => p.Order);
            }

            return _mapper.Map<List<ListMovieDto>>(movies);
        }
    }
}
