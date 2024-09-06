using Data.Models;
using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class Person : TranslatableEntity
    {
        public DateTime BirthDate { get; set; }
        public ICollection<MoviePerson> Movies { get; set; }
    }
}
