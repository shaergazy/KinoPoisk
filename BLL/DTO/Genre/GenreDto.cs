using DAL.Enums;
using DAL.Interfaces;

namespace BLL.DTO.Genre
{
    public class Base
    {
        public ICollection<TranslationDto> Translations { get; set; } = new List<TranslationDto>();
    }
    public class IdHasBase : Base, IIdHas<int>
    {
        public int Id { get; set; }
    }
    public class AddGenreDto : Base { }
    public class EditGenreDto : IdHasBase { }
    public class DeleteGenreDto : IdHasBase { }
    public class GetGenreDto : IdHasBase { }
    public class ListGenreDto : IdHasBase {}
    public class TranslationDto
    {
        public LanguageCode LanguageCode { get; set; }
        public TranslatableFieldType FieldType { get; set; }
        public string Value { get; set; }
    }
}
