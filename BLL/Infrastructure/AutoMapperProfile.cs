using AutoMapper;
using BLL.DTO.CountryDTOs;
using BLL.DTO.GenreDTOs;
using BLL.DTO.PersonDTOs;
using DAL.Entities;

namespace BLL.Infrastructure
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Genre, AddGenreDto>().ReverseMap();
            CreateMap<Genre, EditGenreDto>().ReverseMap();
            CreateMap<Genre, ListGenreDto>().ReverseMap();

            //AutoMapperProfile for Country
            CreateMap<Country, EditCountryDto>().ReverseMap();
            CreateMap<Country, AddCountryDto>().ReverseMap();
            CreateMap<Country, ListCountryDto>().ReverseMap();
            CreateMap<Country, DeleteCountryDto>().ReverseMap();

            CreateMap<Person, EditPersonDto>().ReverseMap();
            CreateMap<Person, AddPersonDto>().ReverseMap();
            CreateMap<Person, GetPersonDto>().ReverseMap();
            CreateMap<Person, DeletePersonDto>().ReverseMap();
        }
    }
}
