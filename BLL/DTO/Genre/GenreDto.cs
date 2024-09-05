using DAL.Enums;
using DAL.Interfaces;

namespace BLL.DTO.Genre
{
    public class Base
    {
        
    }
    public class IdHasBase : Base, IIdHas<int>
    {
        public int Id { get; set; }
    }
    public class AddGenreDto : Base { public ICollection<TranslationDto> Translations { get; set; } = new List<TranslationDto>(); }
    public class EditGenreDto : IdHasBase { public ICollection<TranslationDto> Translations { get; set; } = new List<TranslationDto>(); }
    public class DeleteGenreDto : IdHasBase { }
    public class GetGenreDto : IdHasBase 
    {
        public string Name { get; set; }
        public LanguageCode LanguageCode { get; set; }
    }
    public class ListGenreDto : IdHasBase 
    {
        public string Name { get; set; }
        public LanguageCode LanguageCode{ get; set; }
    }
    public class TranslationDto
    {
        public LanguageCode LanguageCode { get; set; }
        public TranslatableFieldType FieldType { get; set; }
        public string Value { get; set; }
    }
}
