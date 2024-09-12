using KinopoiskWeb.Extensions;
using System.Globalization;

namespace KinopoiskWeb.ViewModels.Movie
{
    public class IndexMovieVM
    {
        public Guid Id { get; set; }
        public string Poster { get; set; }
        public string Title => Translations.GetTranslation(DAL.Enums.TranslatableFieldType.Title, CultureInfo.CurrentCulture.TwoLetterISOLanguageName);
        public string Description => Translations.GetTranslation(DAL.Enums.TranslatableFieldType.Description, CultureInfo.CurrentCulture.TwoLetterISOLanguageName);
        public string? Director { get; set; }
        public uint Duration { get; set; }
        public string[] Actors { get; set; }
        public float Rating { get; set; }

        public ICollection<TranslationVM> Translations { get; set; }
    }
}
