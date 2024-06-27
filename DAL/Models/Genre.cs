using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class Genre
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
