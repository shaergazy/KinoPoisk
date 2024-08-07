﻿using BLL.DTO;
using BLL.DTO.Genre;
using DAL.Models;

namespace BLL.Services.Interfaces
{
    public interface IGenreService : ISearchableService<ListGenreDto, AddGenreDto, EditGenreDto, GetGenreDto, Genre, int, DataTablesRequestDto>, IService
    {
        Task ImportGenres(string genreNames, Movie movie);
    }
}
