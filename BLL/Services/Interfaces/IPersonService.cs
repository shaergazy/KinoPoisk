using BLL.DTO;
using BLL.DTO.Person;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace BLL.Services.Interfaces
{
    public interface IPersonService : ISearchableService<ListPersonDto, AddPersonDto, EditPersonDto, GetPersonDto, Person, 
        int, DataTablesRequestDto>, IService
    {
        public Task<JsonResult> GetSortedAsync(DataTablesRequestDto model);
    }
}
