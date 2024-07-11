using AutoMapper;
using BLL.DTO.Country;
using BLL.DTO.Genre;
using BLL.DTO.Movie;
using BLL.DTO.Person;
using Data.Models;
using KinopoiskWeb.ViewModels.Country;
using KinopoiskWeb.ViewModels.Genre;
using KinopoiskWeb.ViewModels.Movie;
using KinopoiskWeb.ViewModels.Person;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using Newtonsoft.Json;

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

            CreateMap<DataTables.DataTablesRequest, BLL.DTO.DataTablesRequestDto>()
            .ForMember(dest => dest.SortColumn, opt => opt.MapFrom(src => src.Columns.ElementAt(src.Order.ElementAt(0).Column).Name))
            .ForMember(dest => dest.SortDirection, opt => opt.MapFrom(src => src.Order.ElementAt(0).Dir.ToLower()))
            .ForMember(dest => dest.SearchTerm, opt => opt.MapFrom(src => src.Search.Value)).ReverseMap();

            CreateMap(typeof(DataTables.DataTablesResponseVM<>), typeof(BLL.DTO.DataTablesResponse<>)).ReverseMap();

            CreateMap<CreateMovieVM, AddMovieDto>()
            .ForMember(dest => dest.Actors, opt => opt.MapFrom(src => src.Actors))
            .ForMember(dest => dest.GenreIds, opt => opt.MapFrom(src => src.GenreIds ?? new List<int>()));

            CreateMap<ActorVM, MoviePersonDto>()
            .ForMember(dest => dest.PersonId, opt => opt.MapFrom(src => src.PersonId))
            .ForMember(dest => dest.Order, opt => opt.MapFrom(src => src.Order));
        }
    }
}
