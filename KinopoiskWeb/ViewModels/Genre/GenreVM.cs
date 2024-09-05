using DAL.Enums;

namespace KinopoiskWeb.ViewModels.Genre
{
    public class GenreVM
    {
        public int Id { get; set; } 
        public ICollection<TranslationVM> Translations { get; set; } = new List<TranslationVM>();
    }

    public class TranslationVM
    {
        public LanguageCode LanguageCode { get; set; }
        public TranslatableFieldType FieldType { get; set; }
        public string Value { get; set; }
    }
}
