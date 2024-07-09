using BLL.DTO.Person;
using DAL.Models;

namespace BLL.Services.Interfaces
{
    public interface IPersonService : ISearchableService<ListPersonDto, AddPersonDto, EditPersonDto, GetPersonDto, Person, 
        int>, IService
    {
    }
}
