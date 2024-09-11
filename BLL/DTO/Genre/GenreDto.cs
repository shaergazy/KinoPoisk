using DAL.Interfaces;

namespace BLL.DTO.Genre
{
    public class Base
    {
        public ICollection<TranslationDto> Translations { get; set; } = new List<TranslationDto>();
    }
    public class IdHasBase : Base, IIdHas<Guid>
    {
        public Guid Id { get; set; }
    }
    public class AddGenreDto : Base { }
    public class EditGenreDto : IdHasBase { }
    public class DeleteGenreDto : IdHasBase { }
    public class GetGenreDto : IdHasBase { }
    public class ListGenreDto : IdHasBase {}
}
