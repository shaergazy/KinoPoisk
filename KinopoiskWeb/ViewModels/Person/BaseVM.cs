namespace KinopoiskWeb.ViewModels.Person
{
    public class BaseVM
    {
        public DateTime BirthDate { get; set; }
        public ICollection<TranslationVM> Translations { get; set; }
    }
}
