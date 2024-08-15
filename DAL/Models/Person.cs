using Data.Models;
using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class Person
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public ICollection<MoviePerson> Movies { get; set; }
    }
}
