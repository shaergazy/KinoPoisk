using DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class Genre
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<MovieGenre> Movies { get; set; }
    }
}
