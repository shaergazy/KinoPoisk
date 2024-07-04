using System.ComponentModel.DataAnnotations;

namespace KinopoiskWeb.ViewModels.Country
{
    public class CountryVM 
    { 
        public int Id { get; set; }

        [Required]
        [StringLength(128, MinimumLength = 1)]
        public string Name { get; set; }

        [Required]
        [StringLength(2, MinimumLength =2)]
        public string ShortName { get; set; }

        public IFormFile? Flag { get; set; }

    }
}
