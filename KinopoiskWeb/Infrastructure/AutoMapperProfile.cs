using AutoMapper;
using BLL.DTO.CountryDTOs;
using BLL.DTO.GenreDTOs;
using KinopoiskWeb.ViewModels.CountryVM;
using KinopoiskWeb.ViewModels.GenreVM;

namespace KinopoiskWeb.Infrastructure
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreateGenreVM, AddGenreDto>().ReverseMap();
            CreateMap<IndexGenreVM, ListGenreDto>().ReverseMap();
            CreateMap<EditGenreVM, EditGenreDto>().ReverseMap();

            CreateMap<CreateCountryVM, AddCountryDto>().ReverseMap();
            CreateMap<IndexCountryVM, ListCountryDto>().ReverseMap();
            CreateMap<EditCountryVM, EditCountryDto>().ReverseMap();
        }
    }
}
