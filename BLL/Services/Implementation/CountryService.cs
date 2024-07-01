using AutoMapper;
using BLL.DTO;
using BLL.Services.Interfaces;
using DAL.Entities;
using Data.Repositories.RepositoryInterfaces;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services.Implementation
{
    public class CountryService : ICountryService
    {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _uow;

            public CountryService(IMapper mapper, IUnitOfWork uow)
            {
                _mapper = mapper;
                _uow = uow;
            }

            public async Task<int> CreateAsync(CountryDto.Add dto)
            {
                if (dto == null)
                    throw new ArgumentNullException();
                if (_uow.Countries.Any(x => x.Name == dto.Name))
                    throw new Exception("Country already exist");
                var Country = _mapper.Map<Country>(dto);
                await _uow.Countries.AddAsync(Country, true);
                return Country.Id;
            }

            public async Task DeleteById(int id)
            {
                if (_uow.Movies.Any(x => x.CountryId == id))
                    throw new Exception("There are movies in this Country, so you won't be able to delete it.");
                var Country = await _uow.Countries.GetByIdAsync(id);
                if (Country == null)
                    throw new Exception($"Country with id: {id} does not exist");
                await _uow.Countries.DeleteByIdAsync(id);
            }

            public async Task UpdateAsync(CountryDto.Edit dto)
            {
                if (dto == null)
                    throw new ArgumentNullException();
                if (_uow.Countries.Any(x => x.Name == dto.Name && x.Id != dto.Id))
                    throw new Exception("Country already exist");
                var CountryToUpdate = _mapper.Map<Country>(dto);
                await _uow.Countries.UpdateAsync(CountryToUpdate);
            }

            public Task<List<CountryDto.Get>> GetAll()
            {
                return _uow.Countries.GetAll().ProjectTo<CountryDto.Get>(_mapper.ConfigurationProvider).ToListAsync();
            }

            public async Task<CountryDto.Get> GetById(int id)
            {
                return _mapper.Map<CountryDto.Get>(await _uow.Countries.GetByIdAsync(id));
            }
    }
}
