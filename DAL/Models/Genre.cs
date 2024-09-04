namespace DAL.Models
{
    public class Genre : TranslatableEntity
    {
        public ICollection<MovieGenre> Movies { get; set; }
    }
}
