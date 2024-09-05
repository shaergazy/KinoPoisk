using DAL.Enums;
using KinopoiskWeb.Extensions;
using System.Globalization;

namespace KinopoiskWeb.ViewModels.Genre
{
    public class GenreVM
    {
        public int Id { get; set; }
        public string Name => Translations.GetTranslation(TranslatableFieldType.Name, CultureInfo.CurrentCulture.TwoLetterISOLanguageName);
        public string EnglishName {  get; set; } = string.Empty;
        public string RussianName { get; set; } = string.Empty;
        public ICollection<TranslationVM> Translations { get; set; } = new List<TranslationVM>();
    }
}
