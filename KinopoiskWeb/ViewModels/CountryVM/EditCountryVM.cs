namespace KinopoiskWeb.ViewModels.CountryVM
{
    public class EditCountryVM 
    { 
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public IFormFile? Flag { get; set; }

    }
}
