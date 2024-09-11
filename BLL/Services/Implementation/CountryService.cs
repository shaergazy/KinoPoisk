using AutoMapper;
using BLL.DTO;
using BLL.DTO.Country;
using BLL.Services.Interfaces;
using DAL.Enums;
using DAL.Models;
using Data.Repositories.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Dynamic.Core;

namespace BLL.Services.Implementation
{
    public class CountryService : TranslatableService<ListCountryDto, AddCountryDto, EditCountryDto, GetCountryDto, Country, Guid, DataTablesRequestDto>, ICountryService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<Country, Guid> _uow;
        private readonly ILogger<CountryService> _logger;

        public CountryService(IMapper mapper, IUnitOfWork<Country, Guid> unitOfWork, ILogger<CountryService> logger) : base(mapper, unitOfWork, logger)
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
                var movieTitle = movie.Translations.FirstOrDefault(x => x.FieldType == TranslatableFieldType.Title);
                try
                {
                    var country = await _uow.Countries
                        .Include(c => c.Translations)
                        .FirstOrDefaultAsync(c => c.Translations.Any(t => t.Value == countryName 
                                                                    && t.LanguageCode == LanguageCode.en
                                                                    || t.LanguageCode == LanguageCode.ru));

                    if (country == null)
                    {
                        country = new Country();
                        country.Translations.Add(new TranslatableEntityField
                        {
                            LanguageCode = LanguageCode.en,
                            FieldType = TranslatableFieldType.Name,
                            Value = countryName
                        });

                        country.Translations.Add(new TranslatableEntityField
                        {
                            LanguageCode = LanguageCode.ru,
                            FieldType = TranslatableFieldType.Name,
                            Value = countryName
                        });

                        await _uow.Countries.AddAsync(country);
                    }
                    movie.Country = country;
                    _logger.LogInformation("Imported country {CountryName} for movie {MovieTitle}", countryName, movieTitle);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error importing country {CountryName} for movie {MovieTitle}", countryName, movieTitle);
                }
            }
        }


        public override IQueryable<Country> FilterEntities(DataTablesRequestDto request, IQueryable<Country>? entities = null)
        {
            var searchTerm = request.SearchTerm;
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                entities = entities.Where(c => c.Translations.Any(t => t.Value.ToUpper().Contains(searchTerm.ToUpper())));
                _logger.LogInformation("Filtered countries by search term: {SearchTerm}", searchTerm);
            }

            return entities;
        }

        public override async Task<Country> BuildEntityForCreate(AddCountryDto dto)
        {
            var countries = _uow.Repository
            .AsEnumerable()
            .Where(x => x.Translations.Any(t => dto.Translations.Any(d => d.Value == t.Value && d.LanguageCode == t.LanguageCode)))
            .ToList();

            if (dto.Flag == null || dto.Translations == null || !dto.Translations.Any())
            {
                _logger.LogWarning("Invalid AddCountryDto: Flag or Translations are null");
                throw new ArgumentNullException("You have to complete all properties");
            }

            if (countries.Count != 0)
            {
                _logger.LogWarning("Attempted to create an already existing country with one of the provided translations");
                throw new Exception("Country already exists");
            }

            var country = new Country
            {
                Translations = dto.Translations.Select(t => new TranslatableEntityField
                {
                    LanguageCode = t.LanguageCode,
                    FieldType = t.FieldType,
                    Value = t.Value
                }).ToList(),
                ShortName = dto.ShortName
            };

            var relativePath = await SaveFileAsync(dto.Flag);
            country.FlagLink = relativePath;
            country.IsOwnPicture = true;

            _logger.LogInformation("Created new country with translations");

            return country;
        }


        public override async Task<Country> BuildEntityForDelete(Guid id)
        {
            if (_uow.Movies.Any(x => x.CountryId == id))
            {
                _logger.LogWarning("Attempted to delete country with existing movies: {CountryId}", id);
                throw new Exception("There are movies in this Country, so you won't be able to delete it.");
            }

            var country = await GetWithTranslationsByIdAsync(id);

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

            var countryToUpdate = await GetWithTranslationsByIdAsync(dto.Id);
            if (countryToUpdate == null)
            {
                _logger.LogWarning("Attempted to update non-existing country: {CountryId}", dto.Id);
                throw new Exception($"Country with id {dto.Id} doesn't exist");
            }

            foreach (var translationDto in dto.Translations)
            {
                var translation = countryToUpdate.Translations.FirstOrDefault(t => t.LanguageCode == translationDto.LanguageCode && t.FieldType == translationDto.FieldType);
                if (translation != null)
                {
                    translation.Value = translationDto.Value;
                }
                else
                {
                    countryToUpdate.Translations.Add(new TranslatableEntityField
                    {
                        LanguageCode = translationDto.LanguageCode,
                        FieldType = translationDto.FieldType,
                        Value = translationDto.Value
                    });
                }
            }

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
