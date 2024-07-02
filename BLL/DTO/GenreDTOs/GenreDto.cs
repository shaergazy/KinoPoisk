using DAL.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace BLL.DTO.GenreDTOs
{
    public class Base
    {
        [Required]
        public string Name { get; set; }
    }
    public class IdHasBase : Base, IIdHas<int>
    {
        public int Id { get; set; }
    }
    public class AddGenreDto : Base { }
    public class EditGenreDto : IdHasBase { }
    public class DeleteGenreDto : IdHasBase { }
    public class GetGenreDto : IdHasBase { }
    public class ListGenreDto : IdHasBase { }
}
