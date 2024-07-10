﻿using BLL.DTO.Country;
using BLL.DTO.Genre;
using BLL.DTO.Movie;
using BLL.DTO.Person;
using DAL.Models;

namespace BLL.Services.Interfaces
{
    public interface IMovieService : ISearchableService<ListMovieDto, AddMovieDto, EditMovieDto, GetMovieDto, Movie, Guid>, IService
    {
        Task<int> AddRatingAsync(AddMovieRating dto);
        Task<int> AddCommentAsync(AddCommentDo dto);

        Task<IEnumerable<Comment>> GetCommentsAsync(Guid id);
        Task DeleteCommentAsync(int commentId);

        //Task<IEnumerable<Movie>> GetMoviesFromExternalSourceAsync(string titleOrIMDBId);
        //Task ImportMovieFromExternalSourceAsync(Movie movie);

        Task<IEnumerable<Movie>> GetTopRatedMoviesAsync(int count);
        Task<IEnumerable<Movie>> GetNewestMoviesAsync(int count);
        IQueryable<ListCountryDto> GetCountries();
        IQueryable<ListGenreDto> GetGenres();
        IQueryable<ListPersonDto> GetPeople();
    }
}
