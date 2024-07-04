using AutoMapper;
using BLL.DTO.Country;
using BLL.DTO.Genre;
using BLL.DTO.Person;
using KinopoiskWeb.ViewModels.Country;
using KinopoiskWeb.ViewModels.Genre;
using KinopoiskWeb.ViewModels.Person;

namespace KinopoiskWeb.Infrastructure
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<GenreVM, AddGenreDto>().ReverseMap();
            CreateMap<IndexGenreVM, ListGenreDto>().ReverseMap();
            CreateMap<IndexGenreVM, GetGenreDto>().ReverseMap();
            CreateMap<GenreVM, EditGenreDto>().ReverseMap();

            CreateMap<CountryVM, AddCountryDto>().ReverseMap();
            CreateMap<IndexCountryVM, ListCountryDto>().ReverseMap();
            CreateMap<IndexCountryVM, GetCountryDto>().ReverseMap();
            CreateMap<CountryVM, EditCountryDto>().ReverseMap();

            CreateMap<PersonVM, AddPersonDto>().ReverseMap();
            CreateMap<IndexPersonVM, ListPersonDto>().ReverseMap();
            CreateMap<IndexPersonVM, GetPersonDto>().ReverseMap();
            CreateMap<PersonVM, EditPersonDto>().ReverseMap();
        }
    }
}
