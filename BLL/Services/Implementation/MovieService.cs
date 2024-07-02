//using AutoMapper;
//using BLL.DTO;
//using DAL.Entities;
//using Data.Repositories.RepositoryInterfaces;
//using AutoMapper.QueryableExtensions;
//using Microsoft.EntityFrameworkCore;

//namespace BLL.Services.Implementation
//{
//    public class MovieService
//    {
//        private readonly IMapper _mapper;
//        private readonly IUnitOfWork _uow;

//        public PersonService(IMapper mapper, IUnitOfWork uow)
//        {
//            _mapper = mapper;
//            _uow = uow;
//        }

//        public async Task<int> CreateAsync(PersonDto.Add dto)
//        {
//            if (dto == null)
//                throw new ArgumentNullException();

//            var person = _mapper.Map<Person>(dto);
//            await _uow.People.AddAsync(person, true);
//            return person.Id;
//        }

//        public async Task DeleteById(int id)
//        {
//            if (_uow.Movies.Any(x => x.People.Any(p => p.PersonId == id)))
//                throw new Exception("There are movies in this Person, so you won't be able to delete it.");
//            var person = await _uow.People.GetByIdAsync(id);
//            if (person == null)
//                throw new Exception($"Person with id: {id} does not exist");
//            await _uow.People.DeleteByIdAsync(id);
//        }

//        public async Task UpdateAsync(PersonDto.Edit dto)
//        {
//            if (dto == null)
//                throw new ArgumentNullException();

//            var personToUpdate = _mapper.Map<Person>(dto);
//            await _uow.People.UpdateAsync(personToUpdate);
//        }

//        public Task<List<PersonDto.Get>> GetAll()
//        {
//            return _uow.People.GetAll().ProjectTo<PersonDto.Get>(_mapper.ConfigurationProvider).ToListAsync();
//        }

//        public async Task<PersonDto.Get> GetById(int id)
//        {
//            return _mapper.Map<PersonDto.Get>(await _uow.People.GetByIdAsync(id));
//        }
//    }
//}
