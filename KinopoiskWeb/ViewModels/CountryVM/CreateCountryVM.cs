namespace KinopoiskWeb.ViewModels.CountryVM
{
    public class CreateCountryVM
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
        public IFormFile FlagFile {  get; set; }
    }
}
