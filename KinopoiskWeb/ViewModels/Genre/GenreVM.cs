using DAL.Enums;

namespace KinopoiskWeb.ViewModels.Genre
{
    public class GenreVM : BaseVM<int>
    {
        public string Name { get; set; }
        LanguageCode LanguageCode { get; set; }
    }
}
