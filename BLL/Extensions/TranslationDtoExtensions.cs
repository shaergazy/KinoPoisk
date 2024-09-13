using BLL.DTO;
using DAL.Enums;

namespace BLL.Extensions
{
    public static class TranslationDtoExtensions
    {
        public static string GetTranslatedField(this ICollection<TranslationDto> translations, TranslatableFieldType fieldType, string languageCode)
        {
            var a = translations
                .FirstOrDefault(t => t.FieldType == fieldType && t.LanguageCode.ToString() == languageCode)?.Value ?? string.Empty;
            return translations
                .FirstOrDefault(t => t.FieldType == fieldType && t.LanguageCode.ToString() == languageCode)?.Value ?? string.Empty;
        }

        public static string[] GetTranslatedFields(this ICollection<TranslationDto> translations, TranslatableFieldType fieldType, string languageCode)
        {
            var a = translations
                          .Where(x => x.FieldType == fieldType && x.LanguageCode.ToString() == languageCode)
                          .Select(x => x.Value).ToList();
            return translations
                          .Where(x => x.FieldType == fieldType && x.LanguageCode.ToString() == languageCode)
                          .Select(x => x.Value).ToArray();
        }

        public static string GetFullName(this ICollection<TranslationDto> translations, string languageCode)
        {
            var firstName = GetTranslatedField(translations, TranslatableFieldType.FirstName, languageCode);
            var lastName = GetTranslatedField(translations, TranslatableFieldType.LastName, languageCode);
            return $"{firstName} {lastName}".Trim();
        }
    }
}
