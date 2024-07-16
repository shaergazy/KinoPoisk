using AutoMapper;
using BLL.DTO;
using BLL.DTO.Person;
using BLL.Services.Interfaces;
using DAL.Models;
using KinopoiskWeb.DataTables;
using KinopoiskWeb.ViewModels.Person;
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
        public PersonVM Person { get; set; }

        [BindProperty]
        public int PersonId { get; set; }

        public async Task OnGetAsync()
        {
            //People = _mapper.Map<List<IndexPersonVM>>(_service.GetAll());
        }

        [BindProperty]
        public DataTablesRequest DataTablesRequest { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var response = _mapper.Map<DataTablesResponseVM<Person>>(await _service.SearchAsync(_mapper
                                  .Map<DataTablesRequestDto>(DataTablesRequest)));
            return new JsonResult(response);
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

        public JsonResult OnGetPeople(string searchTerm)
        {
            var entities = _service.GetAll();
            if (!string.IsNullOrWhiteSpace(searchTerm) && searchTerm.Length >= 3)
            {
                entities = entities.Where(m => (m.FirstName + " " + m.LastName).Contains(searchTerm)
                                            || (m.LastName + " " + m.FirstName).Contains(searchTerm));
            }
            return new JsonResult(entities);
        }
    }
}

