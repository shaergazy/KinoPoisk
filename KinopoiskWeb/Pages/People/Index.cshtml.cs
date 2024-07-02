//using BLL.DTO.PersonDTOs;
//using BLL.Services.Interfaces;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;

//namespace KinopoiskWeb.Pages.People
//{
//    public class IndexModel : PageModel
//    {
//        private readonly IPersonService _service;

//        public IndexModel(IPersonService service)
//        {
//            _service = service;
//        }

//        [BindProperty]
//        public List<PersonDto.Get> People { get; set; }

//        [BindProperty]
//        public PersonDto.Add NewPerson { get; set; }

//        [BindProperty]
//        public PersonDto.Edit EditedPerson { get; set; }

//        [BindProperty]
//        public PersonDto.Delete PersonToDelete { get; set; }

//        public async Task OnGetAsync()
//        {
//            People = await _service.GetAll();
//        }

//        public async Task<IActionResult> OnPostCreateAsync()
//        {
//            await _service.CreateAsync(NewPerson);

//            return RedirectToPage();
//        }

//        public async Task<IActionResult> OnPostEditAsync()
//        {
//            await _service.UpdateAsync(EditedPerson);

//            return RedirectToPage();
//        }

//        public async Task<IActionResult> OnPostDeleteAsync()
//        {
//            await _service.DeleteById(PersonToDelete.Id);

//            return RedirectToPage();
//        }
//    }
//}

