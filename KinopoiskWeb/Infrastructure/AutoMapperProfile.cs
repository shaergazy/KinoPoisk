using AutoMapper;
using BLL.DTO.CountryDTOs;
using BLL.DTO.GenreDTOs;
using BLL.DTO.PersonDTOs;
using KinopoiskWeb.ViewModels.CountryVM;
using KinopoiskWeb.ViewModels.GenreVM;
using KinopoiskWeb.ViewModels.PersonVM;

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

            CreateMap<CreatePersonVM, AddPersonDto>().ReverseMap();
            CreateMap<IndexPersonVM, ListPersonDto>().ReverseMap();
            CreateMap<EditPersonVM, EditPersonDto>().ReverseMap();
        }
    }
}
