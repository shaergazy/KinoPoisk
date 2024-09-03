namespace DAL.Models
{
    public class Genre : TranslatableEntity
    {
        public string Name { get; set; }
        public ICollection<MovieGenre> Movies { get; set; }
    }
}
