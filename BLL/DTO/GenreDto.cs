using Common.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace BLL.DTO
{
    public class GenreDto
    {
        public class Base
        {
            [Required]
            public string Name { get; set; }
        }
        public class IdHasBase : Base, DAL.Interfaces.IIdHas<int>
        {
            public int Id {  get; set; }
        }
    }
}
