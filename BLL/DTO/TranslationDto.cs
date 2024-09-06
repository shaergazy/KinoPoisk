using DAL.Enums;

namespace BLL.DTO
{
    public class TranslationDto
    {
        public LanguageCode LanguageCode { get; set; }
        public TranslatableFieldType FieldType { get; set; }
        public string Value { get; set; }
    }
}
