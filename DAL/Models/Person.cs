using Data.Models;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class Person
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<MoviePerson> Movies { get;}
    }
}
