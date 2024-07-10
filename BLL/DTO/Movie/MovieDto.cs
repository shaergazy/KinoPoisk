using BLL.DTO.Country;
using BLL.DTO.Genre;
using Common.Enums;
using Data.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BLL.DTO.Movie
{
    //public class Base
    //{
    //    public string Title { get; set; }
    //    public string Description { get; set; }
    //    public int? CountryId { get; set; }
    //    public float? IMDBRating { get; set; }
    //}

    //public class IdHasBase : Base, IIdHas<Guid>
    //{
    //    public Guid Id { get; set; }
    //}

    public class AddMovieDto  
    {
        //public Guid Id { get; set; }
        public IFormFile Poster { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Released { get; set; }
        public DateTime ReleasedDate { get; set; }
        public int? SelectedCountry { get; set; }
        public ICollection<int>? SelectedGenres { get; set; }
        public uint? Duration { get; set; }
        public float? IMDBRating { get; set; }
        //public ICollection<MoviePerson>? People { get; set; }
    }
    public class EditMovieDto : AddMovieDto
    {
        public Guid Id { get; set; }
    }
    public class GetMovieDto : ListMovieDto
    {
        public uint? Duration { get; set; }
        public DateTime DateRealesed { get; set; }
        public float Rating { get; set; }
        public GetCountryDto? Country { get; set; }
        public float? IMDBRating { get; set; }
        public ICollection<GetGenreDto> Genres { get; set; }
        public ICollection<GetCommentDto>? Comments { get; set; }
    }
    public class ListMovieDto
    {
        public string Poster { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public MoviePersonDto? Direcror { get; set; }
        public ICollection<MoviePersonDto>? Actors { get; set; }
    }
    public class MoviePersonDto
    {
        public int Id { get; set; }
        public Guid MovieId { get; set; }
        public int PersonId { get; set; }
        public PersonType PersonType { get; set; }
        public uint PersonOrderId { get; set; }
    }


    public class AddMovieRating
    {
        public Guid MovieId { get; set; }
        public Guid UserId { get; set; }
        public int StarCount { get; set; }
    }

    public class GetCommentDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
    }

    public class AddCommentDo
    {
        public string Text { set; get; }
        public Guid UserId { set; get; }
        public Guid MovieId { set; get; }
    }
}
