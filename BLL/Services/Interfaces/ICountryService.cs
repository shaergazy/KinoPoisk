﻿using BLL.DTO;
using BLL.DTO.Country;
using DAL.Models;

namespace BLL.Services.Interfaces
{
    public interface ICountryService : ISearchableService<ListCountryDto, AddCountryDto, EditCountryDto, GetCountryDto, Country, Guid, DataTablesRequestDto>, IService 
    {
        Task ImportCountry(string countryNames, Movie movie);
    }
}
