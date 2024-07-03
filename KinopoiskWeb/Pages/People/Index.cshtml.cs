using AutoMapper;
using BLL.DTO.PersonDTOs;
using BLL.Services.Interfaces;
using KinopoiskWeb.ViewModels.PersonVM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KinopoiskWeb.Pages.People
{
    public class IndexModel : PageModel
    {
        private readonly IPersonService _service;
        private readonly IMapper _mapper;

        public IndexModel(IPersonService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [BindProperty]
        public List<IndexPersonVM> People { get; set; }

        [BindProperty]
        public CreatePersonVM NewPerson { get; set; }

        [BindProperty]
        public EditPersonVM EditedPerson { get; set; }

        [BindProperty]
        public int PersonId { get; set; }

        public async Task OnGetAsync()
        {
            People = _mapper.Map<List<IndexPersonVM>>(await _service.GetAll());
        }

        public async Task<JsonResult> OnGetGetPerson(int id)
        {
            var person = await _service.GetById(id);
            if (person == null)
            {
                return new JsonResult(NotFound());
            }

            return new JsonResult(person);
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            await _service.CreateAsync(_mapper.Map<AddPersonDto>(NewPerson));

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEditAsync()
        {
            await _service.UpdateAsync(_mapper.Map<EditPersonDto>(EditedPerson));

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            await _service.DeleteById(PersonId);

            return RedirectToPage();
        }
    }
}

