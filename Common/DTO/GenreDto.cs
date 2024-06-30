using Common.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Common.DTO
{
    public class GenreDto
    {
        public class Base
        {
            [Required]
            public string Name { get; set; }
        }
        public class IdHasBase : Base, IIdHas<int>
        {
            public int Id {  get; set; }
        }
    }
}
