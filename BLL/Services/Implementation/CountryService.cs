using AutoMapper;
using AutoMapper.QueryableExtensions;
using BLL.DTO.CountryDTOs;
using BLL.Services.Interfaces;
using Common.Extensions;
using Common.Helpers;
using DAL.Entities;
using Data.Repositories.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

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

        public async Task<int> CreateAsync(AddCountryDto dto)
        {
            if (dto.Flag == null || dto.Name == null)
                throw new ArgumentNullException();
            if (_uow.Countries.Any(x => x.Name == dto.Name))
                throw new Exception("Country already exist");
            var country = _mapper.Map<Country>(dto);
            var file = dto.Flag;
            var path = AppConstants.RelativeFilesPath.Combine(AppConstants.BaseDir, AppConstants.FlagDir, file.FileName);
                
            (Stream Source, string FileName) fileStream = await file.ToStream();
            await (path, fileStream.Source).SaveStreamByPath();


            var relativePath = AppConstants.RelativeFilesPath.Combine(AppConstants.FlagDir, file.FileName);

            country.FlagLink = relativePath;
            country.IsOwnPicture = true;
            await _uow.Countries.AddAsync(country, true);
            return country.Id;
        }

        public async Task DeleteById(int id)
        {
            if (_uow.Movies.Any(x => x.CountryId == id))
                throw new Exception("There are movies in this Country, so you won't be able to delete it.");
            var country = await _uow.Countries.GetByIdAsync(id);
            if (country == null)
                throw new Exception($"Country with id: {id} does not exist");

            await _uow.Countries.DeleteByIdAsync(id);

            if(country.IsOwnPicture)
                File.Delete(country.FlagLink);
        }

        public async Task UpdateAsync(EditCountryDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException();
            if (_uow.Countries.Any(x => x.Name == dto.Name && x.Id != dto.Id))
                throw new Exception("Country already exist");
            var countryToUpdate = await _uow.Countries.GetByIdAsync(dto.Id);

            countryToUpdate.Name = dto.Name;
            countryToUpdate.ShortName = dto.ShortName;

            if (dto.Flag !=  null)
            {
                var file = dto.Flag;
                var path = AppConstants.RelativeFilesPath.Combine(AppConstants.BaseDir, AppConstants.FlagDir, file.FileName);

                (Stream Source, string FileName) fileStream = await file.ToStream();
                await (path, fileStream.Source).SaveStreamByPath();


                var relativePath = AppConstants.RelativeFilesPath.Combine(AppConstants.FlagDir, file.FileName);

                countryToUpdate.FlagLink = relativePath;
                countryToUpdate.IsOwnPicture = true;
            }
            await _uow.Countries.UpdateAsync(countryToUpdate);
        }

        public Task<List<ListCountryDto>> GetAll()
        {
            return _uow.Countries.GetAll().ProjectTo<ListCountryDto>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<GetCountryDto> GetById(int id)
        {
            return _mapper.Map<GetCountryDto>(await _uow.Countries.GetByIdAsync(id));
        }
    }
}
