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
    public class MovieService : SearchableService<ListMovieDto, AddMovieDto, EditMovieDto, GetMovieDto, Movie, Guid>,
        IMovieService
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

        public override IQueryable<Movie> FilterEntities(IQueryable<Movie> entities, string searchTerm)
        {
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                //entities = entities.Where(s =>
                //    s.Name.ToUpper().Contains(searchTerm));
            }
            return entities;
        }

        public Task<IEnumerable<Comment>> GetCommentsAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        //public Task<IEnumerable<Comment>> GetCommentsAsync(Guid id)
        //{
        //    var comments = _uow.Movies.GetAll().Where(x => x.Id == id).Include(p => p.Comments);
        //    var d = comments.
        //}

        public async Task<IEnumerable<Movie>> GetNewestMoviesAsync(int count)
        {
            return await _uow.Movies.GetAll().OrderBy(x => x.ReleasedDate).Take(count).ToListAsync();
        }

        public async Task<IEnumerable<Movie>> GetTopRatedMoviesAsync(int count)
        {
            return await _uow.Movies.GetAll().Select(movie => new
            {
                Movie = movie,
                AverageRating = movie.Ratings.Average(r => (int?)r.StarCount) ?? 0
            })
            .OrderByDescending(x => x.AverageRating)
            .Select(x => x.Movie)
            .Take(count)
            .ToListAsync();
        }

        public override async Task<Movie> BuildEntityForCreate(AddMovieDto dto)
        {
            if (dto.Poster == null || dto.Title == null)
                throw new ArgumentNullException("You have to complete all properties");

            var movie = _mapper.Map<Movie>(dto);
            var file = dto.Poster;
            var fileName = GenerateUniqueFileName(file);
            var path = AppConstants.RelativeFilesPath.Combine(AppConstants.BaseDir, AppConstants.PosterDir, fileName);

            (Stream Source, string FileName) fileStream = await file.ToStream();
            await (path, fileStream.Source).SaveStreamByPath();

            var relativePath = AppConstants.RelativeFilesPath.Combine(AppConstants.PosterDir, fileName);

            var country = await _uow.Countries.FirstOrDefaultAsync(x => x.Id == dto.SelectedCountry);
            if (country != null)
                movie.Country = country;

            var genres = dto.SelectedGenres ?? new List<int>();
            if (movie.Genres == null)
            {
                movie.Genres = new List<MovieGenre>();
            }

            foreach (var genreId in genres)
            {
                var genre = await _uow.Genres.FirstOrDefaultAsync(x => x.Id == genreId);
                if (genre != null)
                {
                    movie.Genres.Add(new MovieGenre
                    {
                        Movie = movie,
                        Genre = genre,
                    });
                }
                else
                {
                   //TODO Logging
                }
            }
            movie.Poster = relativePath;
            return movie;
        }

        public override async Task<Movie> BuildEntityForDelete(Guid id)
        {
            var movie = await _uow.Repository.GetByIdAsync(id);

                File.Delete(movie.Poster);
            return movie;
        }

        public override async Task<Movie> BuildEntityForUpdate(EditMovieDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException();

            var movieToUpdate = await _uow.Repository.GetByIdAsync(dto.Id);
            if (movieToUpdate == null)
                throw new Exception($"Movie with id {movieToUpdate.Id} doesnt exist");
            var country = await _uow.Countries.FirstOrDefaultAsync(x => x.Id == dto.SelectedCountry);
            if (country != null)
                movieToUpdate.Country = country;

            movieToUpdate.Title = dto.Title;
            movieToUpdate.Description = dto.Description;
            movieToUpdate.ReleasedDate = dto.ReleasedDate;


            if (dto.Poster != null)
            {
                if (movieToUpdate.Poster != null)
                    File.Delete(movieToUpdate.Poster);

                var file = dto.Poster;
                var path = AppConstants.RelativeFilesPath.Combine(AppConstants.BaseDir, AppConstants.PosterDir, file.FileName);

                (Stream Source, string FileName) fileStream = await file.ToStream();
                await (path, fileStream.Source).SaveStreamByPath();


                var relativePath = AppConstants.RelativeFilesPath.Combine(AppConstants.PosterDir, file.FileName);

                movieToUpdate.Poster = relativePath;
            }
            return movieToUpdate;
        }

        public string GenerateUniqueFileName(IFormFile file)
        {
            return $"{Path.GetFileNameWithoutExtension(file.FileName)}_{DateTime.Now.ToString("yyyyMMddHHmmss")}{Path.GetExtension(file.FileName)}";
        }

        public IQueryable<ListCountryDto> GetCountries()
        {
            var countries = _uow.Countries.GetAll();
            return _mapper.Map<IQueryable<ListCountryDto>>(countries);
        }

        public IQueryable<ListGenreDto> GetGenres()
        {
            var countries = _uow.Genres.GetAll();
            return _mapper.Map<IQueryable<ListGenreDto>>(countries);
        }

        public IQueryable<ListPersonDto> GetPeople()
        {
            var countries = _uow.People.GetAll();
            return _mapper.Map<IQueryable<ListPersonDto>>(countries);
        }
    }
}

