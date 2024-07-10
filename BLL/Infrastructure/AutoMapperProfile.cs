using AutoMapper;
using BLL.DTO.Country;
using BLL.DTO.Genre;
using BLL.DTO.Movie;
using BLL.DTO.Person;
using Common.Helpers;
using DAL.Models;
using Data.Models;

namespace BLL.Infrastructure
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Genre, AddGenreDto>().ReverseMap();
            CreateMap<Genre, EditGenreDto>().ReverseMap();
            CreateMap<Genre, ListGenreDto>().ReverseMap();
            CreateMap<Genre, GetGenreDto>().ReverseMap();

            //AutoMapperProfile for Country
            CreateMap<Country, EditCountryDto>().ReverseMap();
            CreateMap<Country, AddCountryDto>().ReverseMap();
            CreateMap<Country, ListCountryDto>().ReverseMap();

            CreateMap<Country, DeleteCountryDto>().ReverseMap();
            CreateMap<Country, GetCountryDto>().ReverseMap();

            CreateMap<Person, EditPersonDto>().ReverseMap();
            CreateMap<Person, AddPersonDto>().ReverseMap();
            CreateMap<Person, GetPersonDto>().ReverseMap();
            CreateMap<Person, ListPersonDto>().ReverseMap();
            CreateMap<Person, DeletePersonDto>().ReverseMap();

            CreateMap<Comment, AddCommentDo>().ReverseMap();

            CreateMap<MovieRating, AddMovieRating>().ReverseMap();
            CreateMap<AddMovieDto, Movie>().ReverseMap();
        }
    }
}
