using System.ComponentModel.DataAnnotations;

namespace KinopoiskWeb.ViewModels.Country
{
    public class CountryVM 
    { 
        public Guid Id { get; set; }

        public ICollection<TranslationVM> Translations { get; set; }

        [Required]
        [StringLength(2, MinimumLength =2)]
        public string ShortName { get; set; }

        public IFormFile? Flag { get; set; }

    }
}
