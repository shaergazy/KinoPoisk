using DAL.Enums;
using KinopoiskWeb.Extensions;
using System.Globalization;

namespace KinopoiskWeb.ViewModels.Genre
{
    public class GenreVM
    {
        public Guid Id { get; set; }
        public string Name => Translations.GetTranslation(TranslatableFieldType.Name, CultureInfo.CurrentCulture.TwoLetterISOLanguageName);
        public ICollection<TranslationVM> Translations { get; set; } = new List<TranslationVM>();
    }
}
