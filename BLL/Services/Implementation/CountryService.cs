using AutoMapper;
using BLL.DataTables;
using BLL.DTO.Country;
using BLL.Services.Interfaces;
using Common.Extensions;
using Common.Helpers;
using DAL.Models;
using Data.Repositories.RepositoryInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace BLL.Services.Implementation
{
    public class CountryService : SearchableService<ListCountryDto, AddCountryDto, EditCountryDto, GetCountryDto, Country, int>, ICountryService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<Country, int> _uow;

        public CountryService(IMapper mapper, IUnitOfWork<Country, int> unitOfWork) : base(mapper, unitOfWork)
        {
            _mapper = mapper;
            _uow = unitOfWork;
        }

        public async Task<JsonResult> GetSortedAsync(DataTablesRequest request)
        {
            var entities = _uow.Repository.GetAll();

            var recordsTotal = entities.Count();

            var searchText = request.Search.Value?.ToUpper();
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                entities = entities.Where(s =>
                    s.Name.ToUpper().Contains(searchText)
                );
            }

            var recordsFiltered = entities.Count();

            var sortColumnName = request.Columns.ElementAt(request.Order.ElementAt(0).Column).Name;
            var sortDirection = request.Order.ElementAt(0).Dir.ToLower();

            entities = entities.OrderBy($"{sortColumnName} {sortDirection}");

            var skip = request.Start;
            var take = request.Length;
            var data = await entities
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            return new JsonResult(new
            {
                Draw = request.Draw,
                RecordsTotal = recordsTotal,
                RecordsFiltered = recordsFiltered,
                Data = data
            });
        }

        public override async Task<Country> BuildEntityForCreate(AddCountryDto dto)
        {
            if (dto.Flag == null || dto.Name == null)
                throw new ArgumentNullException("You have to complete all properties");
            if (_uow.Repository.Any(x => x.Name == dto.Name))
                throw new Exception("Country already exist");
            var country = _mapper.Map<Country>(dto);
            var file = dto.Flag;
            var fileName = GenerateUniqueFileName(file);
            var path = AppConstants.RelativeFilesPath.Combine(AppConstants.BaseDir, AppConstants.FlagDir, fileName);

            (Stream Source, string FileName) fileStream = await file.ToStream();
            await (path, fileStream.Source).SaveStreamByPath();

            var relativePath = AppConstants.RelativeFilesPath.Combine(AppConstants.FlagDir, fileName);

            country.FlagLink = relativePath;
            country.IsOwnPicture = true;
            return country;
        }

        public override async Task<Country> BuildEntityForDelete(int id)
        {
            if (_uow.Movies.Any(x => x.CountryId == id))
                throw new Exception("There are movies in this Country, so you won't be able to delete it.");
            var country = await _uow.Repository.GetByIdAsync(id);
            if (country == null)
                throw new Exception($"Country with id: {id} does not exist");

            if (country.IsOwnPicture)
                File.Delete(country.FlagLink);
            return country;
        }

        public override async Task<Country> BuildEntityForUpdate(EditCountryDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException();
            if (_uow.Repository.Any(x => x.Name == dto.Name && x.Id != dto.Id))
                throw new Exception("Country already exist");

            var countryToUpdate = await _uow.Repository.GetByIdAsync(dto.Id);
            if (countryToUpdate == null)
                throw new Exception($"Country with id {countryToUpdate.Id} doesnt exist");

            countryToUpdate.Name = dto.Name;
            countryToUpdate.ShortName = dto.ShortName;

            if (dto.Flag != null)
            {
                if (countryToUpdate.IsOwnPicture)
                    File.Delete(countryToUpdate.FlagLink);

                var file = dto.Flag;
                var path = AppConstants.RelativeFilesPath.Combine(AppConstants.BaseDir, AppConstants.FlagDir, file.FileName);

                (Stream Source, string FileName) fileStream = await file.ToStream();
                await (path, fileStream.Source).SaveStreamByPath();


                var relativePath = AppConstants.RelativeFilesPath.Combine(AppConstants.FlagDir, file.FileName);

                countryToUpdate.FlagLink = relativePath;
                countryToUpdate.IsOwnPicture = true;
            }
            return countryToUpdate;
        }

        public string GenerateUniqueFileName(IFormFile file)
        {
            return $"{Path.GetFileNameWithoutExtension(file.FileName)}_{DateTime.Now.ToString("yyyyMMddHHmmss")}{Path.GetExtension(file.FileName)}";
        }
    }
}
