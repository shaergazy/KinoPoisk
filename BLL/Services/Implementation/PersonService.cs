using AutoMapper;
using AutoMapper.QueryableExtensions;
using BLL.DTO.Person;
using BLL.Services.Interfaces;
using DAL.Models;
using Data.Repositories.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services.Implementation
{
    public class PersonService : IPersonService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;

        public PersonService(IMapper mapper, IUnitOfWork uow)
        {
            _mapper = mapper;
            _uow = uow;
        }

        public async Task<int> CreateAsync(AddPersonDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException();

            var person = _mapper.Map<Person>(dto);
            await _uow.People.AddAsync(person, true);
            return person.Id;
        }

        public async Task DeleteById(int id)
        {
            if (_uow.Movies.Any(x => x.People.Any(p => p.PersonId == id)))
                throw new Exception("There are movies in this Person, so you won't be able to delete it.");
            var person = await _uow.People.GetByIdAsync(id);
            if (person == null)
                throw new Exception($"Person with id: {id} does not exist");
            await _uow.People.DeleteByIdAsync(id);
        }

        public async Task UpdateAsync(EditPersonDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException();

            var personToUpdate = _mapper.Map<Person>(dto);
            await _uow.People.UpdateAsync(personToUpdate);
        }

        public Task<List<ListPersonDto>> GetAll()
        {
            return _uow.People.GetAll().ProjectTo<ListPersonDto>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<GetPersonDto> GetById(int id)
        {
            return _mapper.Map<GetPersonDto>(await _uow.People.GetByIdAsync(id));
        }
    }
}
