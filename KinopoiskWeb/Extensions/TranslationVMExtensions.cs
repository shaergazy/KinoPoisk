using DAL.Enums;
using KinopoiskWeb.ViewModels;

namespace KinopoiskWeb.Extensions
{
    public static class TranslationVMExtensions
    {
        public static string GetTranslation(this ICollection<TranslationVM> translations, TranslatableFieldType fieldType, string languageCode)
        {
            var s = translations.FirstOrDefault(x => x.FieldType == fieldType && x.LanguageCode.ToString() == languageCode)?.Value ?? string.Empty;
            return translations.FirstOrDefault(x => x.FieldType == fieldType && x.LanguageCode.ToString() == languageCode)?.Value ?? string.Empty;
        }
    }
}
