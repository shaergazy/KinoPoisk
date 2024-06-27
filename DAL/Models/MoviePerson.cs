using Common.Enums;
using DAL.Entities;

namespace Data.Models
{
    public class MoviePerson
    {
        public int Id { get; set; }
        public Guid MovieId { get; set; }
        public Movie Movie { get; set; }
        public int PersonId { get; set; }
        public Person Person { get; set; }
        public PersonType PersonType { get; set; }
    }
}
