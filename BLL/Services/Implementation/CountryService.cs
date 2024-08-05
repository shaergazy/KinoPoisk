using AutoMapper;
using BLL.DTO;
using BLL.DTO.Country;
using BLL.Services.Interfaces;
using DAL.Models;
using Data.Repositories.RepositoryInterfaces;
using Microsoft.Extensions.Logging;
using System.Linq.Dynamic.Core;

namespace BLL.Services.Implementation
{
    public class CountryService : SearchableService<ListCountryDto, AddCountryDto, EditCountryDto, GetCountryDto, Country, int, DataTablesRequestDto>, ICountryService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<Country, int> _uow;
        private readonly ILogger<CountryService> _logger;

        public CountryService(IMapper mapper, IUnitOfWork<Country, int> unitOfWork, ILogger<CountryService> logger) : base(mapper, unitOfWork, logger)
        {
            _mapper = mapper;
            _uow = unitOfWork;
            _logger = logger;
        }

        public async Task ImportCountry(string countryNames, Movie movie)
        {
            var countries = countryNames.Split(", ");
            foreach (var countryName in countries)
            {
                try
                {
                    var country = await _uow.Countries.FirstOrDefaultAsync(c => c.Name == countryName);
                    if (country == null)
                    {
                        country = new Country { Name = countryName };
                        await _uow.Countries.AddAsync(country);
                    }
                    movie.Country = country;

                    _logger.LogInformation("Imported country {CountryName} for movie {MovieTitle}", countryName, movie.Title);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error importing country {CountryName} for movie {MovieTitle}", countryName, movie.Title);
                }
            }
        }

        public override IQueryable<Country> FilterEntities(DataTablesRequestDto request, IQueryable<Country>? entities = null)
        {
            var searchTerm = request.SearchTerm;
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                entities = entities.Where(s => s.Name.ToUpper().Contains(searchTerm.ToUpper()));
                _logger.LogInformation("Filtered countries by search term: {SearchTerm}", searchTerm);
            }

            return entities;
        }

        public override async Task<Country> BuildEntityForCreate(AddCountryDto dto)
        {
            if (dto.Flag == null || dto.Name == null)
            {
                _logger.LogWarning("Invalid AddCountryDto: Flag or Name is null");
                throw new ArgumentNullException("You have to complete all properties");
            }

            if (_uow.Repository.Any(x => x.Name == dto.Name))
            {
                _logger.LogWarning("Attempted to create an already existing country: {CountryName}", dto.Name);
                throw new Exception("Country already exist");
            }

            var country = _mapper.Map<Country>(dto);
            var relativePath = await SaveFileAsync(dto.Flag);

            country.FlagLink = relativePath;
            country.IsOwnPicture = true;

            _logger.LogInformation("Created new country: {CountryName}", country.Name);

            return country;
        }

        public override async Task<Country> BuildEntityForDelete(int id)
        {
            if (_uow.Movies.Any(x => x.CountryId == id))
            {
                _logger.LogWarning("Attempted to delete country with existing movies: {CountryId}", id);
                throw new Exception("There are movies in this Country, so you won't be able to delete it.");
            }

            var country = await _uow.Repository.GetByIdAsync(id);
            if (country == null)
            {
                _logger.LogWarning("Attempted to delete non-existing country: {CountryId}", id);
                throw new Exception($"Country with id: {id} does not exist");
            }

            if (country.IsOwnPicture)
            {
                _logger.LogInformation("Deleting flag image for country: {CountryId}", id);
                File.Delete(country.FlagLink);
            }

            _logger.LogInformation("Deleted country: {CountryId}", id);

            return country;
        }

        public override async Task<Country> BuildEntityForUpdate(EditCountryDto dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("Invalid EditCountryDto: dto is null");
                throw new ArgumentNullException();
            }

            if (_uow.Repository.Any(x => x.Name == dto.Name && x.Id != dto.Id))
            {
                _logger.LogWarning("Attempted to update to an already existing country name: {CountryName}", dto.Name);
                throw new Exception("Country already exist");
            }

            var countryToUpdate = await _uow.Repository.GetByIdAsync(dto.Id);
            if (countryToUpdate == null)
            {
                _logger.LogWarning("Attempted to update non-existing country: {CountryId}", dto.Id);
                throw new Exception($"Country with id {dto.Id} doesn't exist");
            }

            countryToUpdate.Name = dto.Name;
            countryToUpdate.ShortName = dto.ShortName;

            if (dto.Flag != null)
            {
                if (countryToUpdate.IsOwnPicture)
                {
                    _logger.LogInformation("Deleting old flag image for country: {CountryId}", dto.Id);
                    File.Delete(countryToUpdate.FlagLink);
                }

                countryToUpdate.FlagLink = await SaveFileAsync(dto.Flag);
                countryToUpdate.IsOwnPicture = true;

                _logger.LogInformation("Updated flag image for country: {CountryId}", dto.Id);
            }

            _logger.LogInformation("Updated country: {CountryId}", dto.Id);

            return countryToUpdate;
        }
    }
}
