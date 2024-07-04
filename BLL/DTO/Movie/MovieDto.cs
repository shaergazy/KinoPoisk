using DAL.Interfaces;
using Data.Models;

namespace BLL.DTO.Movie
{
    public class Base
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime RealesedDate { get; set; }
        public int? CountryId { get; set; }
        public ICollection<int> MoviesIds { get; set; }
        public ICollection<DAL.Models.Comment> Comments { get; set; }
        public ICollection<MoviePerson> People { get; set; }
        public uint? Duration { get; set; }
        public float? IMDBRating { get; set; }
        public ICollection<MovieRating> Ratings { get; set; }
    }

    public class IdHasBase : Base, IIdHas<Guid>
    {
        public Guid Id { get; set; }
    }

    public class AddMovieDto : Base { }
    public class EditMovieDto : IdHasBase { }
    public class DeleteMovieDto : IdHasBase { }
    public class GetMovieDto : IdHasBase { }
    public class ListMovieDto : IdHasBase { }
}
