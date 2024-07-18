using BLL.DTO;
using BLL.DTO.Person;
using DAL.Models;

namespace BLL.Services.Interfaces
{
    public interface IPersonService : ISearchableService<ListPersonDto, AddPersonDto, EditPersonDto, GetPersonDto, Person, int, DataTablesRequestDto>, IService
    {
        IEnumerable<ListPersonDto> GetActors();
        IEnumerable<ListPersonDto> GetDirectors();
    }
}
