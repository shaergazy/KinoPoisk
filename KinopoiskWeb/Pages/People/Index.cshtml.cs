using AutoMapper;
using BLL.DTO.Person;
using BLL.Services.Interfaces;
using DAL.Models;
using KinopoiskWeb.ViewModels.Person;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KinopoiskWeb.Pages.People
{
    public class IndexModel : PageModel
    {
        private readonly ISearchableService<ListPersonDto, AddPersonDto, EditPersonDto, GetPersonDto, Person, int> _service;
        private readonly IMapper _mapper;

        public IndexModel(ISearchableService<ListPersonDto, AddPersonDto, EditPersonDto, GetPersonDto, Person, int> service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [BindProperty]
        public List<IndexPersonVM> People { get; set; }

        [BindProperty]
        public PersonVM Person { get; set; }

        [BindProperty]
        public int PersonId { get; set; }

        public async Task OnGetAsync()
        {
            People = _mapper.Map<List<IndexPersonVM>>(await _service.GetAllAsync());
        }

        public async Task<JsonResult> OnGetById(int id)
        {
            var person = await _service.GetByIdAsync(id);
            if (person == null)
            {
                return new JsonResult(NotFound());
            }

            return new JsonResult(_mapper.Map<IndexPersonVM>(person));
        }

        public async Task<IActionResult> OnPostHandleCreateOrUpdateAsync(PersonVM person)
        {
            if (person == null)
                throw new ArgumentNullException(nameof(person));
            if(person.Id == 0)
                await _service.CreateAsync(_mapper.Map<AddPersonDto>(person));
            else
                await _service.UpdateAsync(_mapper.Map<EditPersonDto>(person));

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            await _service.DeleteAsync(PersonId);

            return RedirectToPage();
        }
    }
}

