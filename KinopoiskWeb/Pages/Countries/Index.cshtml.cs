using DAL.Entities;
using Data.Repositories.RepositoryInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace KinopoiskWeb.Pages.Countries
{
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _uow;

        public IndexModel(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [BindProperty]
        public List<Country> Countries { get; set; }

        [BindProperty]
        public Country NewCountry { get; set; }

        [BindProperty]
        public Country EditedCountry { get; set; }

        [BindProperty]
        public Country CountryToDelete { get; set; }

        public async Task OnGetAsync()
        {
            Countries = await _uow.Countries.GetAll().ToListAsync();
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            if (!ModelState.IsValid)
            {
                return RedirectToPage("/Error");
            }

            await _uow.Countries.AddAsync(NewCountry, true);

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEditAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var country = _uow.Countries.GetAll().FirstOrDefault(x => x.Id == EditedCountry.Id);
            if (country == null)
            {
                return NotFound();
            }

            country.Name = EditedCountry.Name;
            country.ShortName = EditedCountry.ShortName;
            country.FlagLink = EditedCountry.FlagLink;

            await _uow.Countries.UpdateAsync(country);
            await _uow.SaveChangesAsync();

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            var country = _uow.Countries.GetAll().FirstOrDefault(x => x.Id == CountryToDelete.Id);
            if (country == null)
            {
                return NotFound();
            }

            await _uow.Countries.DeleteAsync(CountryToDelete.Id);
            await _uow.SaveChangesAsync();

            return RedirectToPage();
        }
    }

}
