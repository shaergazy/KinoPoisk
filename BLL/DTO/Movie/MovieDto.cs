using BLL.DTO.Country;
using BLL.DTO.Genre;
using BLL.DTO.Person;
using Microsoft.AspNetCore.Http;

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
        public int? CountryId { get; set; }
        public ICollection<int>? GenreIds { get; set; }
        public uint? Duration { get; set; }
        public float? IMDBRating { get; set; }
        public int DirectorId { get; set; }
        public List<MoviePersonDto>? Actors { get; set; }
    }

    public class MoviePersonDto
    {
        public int PersonId { get; set; }
        public uint Order { get; set; }
    }

    public class EditMovieDto : AddMovieDto
    {
        public Guid Id { get; set; }
    }
    public class GetMovieDto : ListMovieDto
    {
        public DateTime DateRealesed { get; set; }
        public float Rating { get; set; }
        public GetCountryDto? Country { get; set; }
        public float? IMDBRating { get; set; }
        public ICollection<GetGenreDto> Genres { get; set; }
        public ICollection<GetCommentDto>? Comments { get; set; }
    }
    public class ListMovieDto
    {
        public Guid Id { get; set; }
        public string Poster { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public uint Duration { get; set; }
        public GetPersonDto? Director { get; set; }
        public ICollection<GetPersonDto>? Actors { get; set; }
        public float Rating { get; set; }
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
        public string UserName { get; set; }
        public DateTime? Date { get; set; }
    }

    public class AddCommentDto
    {
        public string Text { set; get; }
        public string UserId { set; get; }
        public Guid MovieId { set; get; }
    }
}
