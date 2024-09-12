using DAL.Enums;
using DAL.Models;

namespace DAL.Extensions
{
    public static class TranslatableFieldExtensions
    {
        public static string GetTranslatedField(ICollection<TranslatableEntityField> translations, TranslatableFieldType fieldType, string languageCode)
        {
            return translations
                .FirstOrDefault(t => t.FieldType == fieldType && t.LanguageCode.ToString() == languageCode)?.Value ?? string.Empty;
        }

        public static string GetFullName(ICollection<TranslatableEntityField> translations, string languageCode)
        {
            var firstName = GetTranslatedField(translations, TranslatableFieldType.FirstName, languageCode);
            var lastName = GetTranslatedField(translations, TranslatableFieldType.LastName, languageCode);
            return $"{firstName} {lastName}".Trim();
        }
    }
}
