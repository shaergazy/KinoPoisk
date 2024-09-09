using DAL.Enums;

namespace BLL.DTO
{
    public class TranslationDto
    {
        public int Id { get; set; }
        public LanguageCode LanguageCode { get; set; }
        public TranslatableFieldType FieldType { get; set; }
        public string Value { get; set; }
    }
}
