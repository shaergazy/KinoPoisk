using Microsoft.AspNetCore.Http;

namespace BLL.DTO.Movie
{
    public class Base
    {
        public Guid Id { get; set; }
        public ICollection<TranslationDto> Translations { get; set; }
    }

    public class AddMovieDto  : Base
    {
        public IFormFile Poster { get; set; }
        public DateTime Released { get; set; }
        public DateTime ReleasedDate { get; set; }
        public Guid CountryId { get; set; }
        public ICollection<Guid>? GenreIds { get; set; }
        public uint? Duration { get; set; }
        public float? IMDBRating { get; set; }
        public Guid DirectorId { get; set; }
        public List<MoviePersonDto>? Actors { get; set; }
    }

    public class MoviePersonDto
    {
        public Guid PersonId { get; set; }
        public uint Order { get; set; }
    }

    public class EditMovieDto : AddMovieDto { }
    public class GetMovieDto : ListMovieDto
    {
        public DateTime DateReleased { get; set; }
        public string? Country { get; set; }
        public float? IMDBRating { get; set; }
        public string[] Genres { get; set; }
        public ICollection<GetCommentDto>? Comments { get; set; }
    }
    public class ListMovieDto : Base
    {
        public string Poster { get; set; }
        public uint Duration { get; set; }
        public string? Director { get; set; }
        public ICollection<string>? Actors { get; set; }
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
