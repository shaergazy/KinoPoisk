using AutoMapper;
using BLL.DTO;
using BLL.DTO.Country;
using BLL.DTO.Genre;
using BLL.DTO.Movie;
using BLL.DTO.Person;
using BLL.DTO.SubscriptionPlan;
using DAL.Enums;
using DAL.Models;
using Data.Models;

namespace BLL.Infrastructure
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ListGenreDto, Genre>()
           .ForMember(dest => dest.Translations, opt => opt.MapFrom(src => src.Translations)).ReverseMap();

            CreateMap<GetGenreDto, Genre>()
                .ForMember(dest => dest.Translations, opt => opt.MapFrom(src => src.Translations)).ReverseMap();

            CreateMap<AddGenreDto, Genre>()
           .ForMember(dest => dest.Translations, opt => opt.MapFrom(src => src.Translations));

            CreateMap<EditGenreDto, Genre>()
                .ForMember(dest => dest.Translations, opt => opt.MapFrom(src => src.Translations));

            CreateMap<DeleteGenreDto, Genre>().ReverseMap();

            CreateMap<TranslatableEntityField, TranslationDto>().ReverseMap();


            CreateMap<ListCountryDto, Country>()
            .ForMember(dest => dest.Translations, opt => opt.MapFrom(src => src.Translations)).ReverseMap();

            CreateMap<GetCountryDto, Country>()
                .ForMember(dest => dest.Translations, opt => opt.MapFrom(src => src.Translations)).ReverseMap();

            CreateMap<AddCountryDto, Country>()
           .ForMember(dest => dest.Translations, opt => opt.MapFrom(src => src.Translations));

            CreateMap<EditCountryDto, Country>()
                .ForMember(dest => dest.Translations, opt => opt.MapFrom(src => src.Translations));

            CreateMap<ListPersonDto, Person>()
            .ForMember(dest => dest.Translations, opt => opt.MapFrom(src => src.Translations)).ReverseMap();

            CreateMap<GetPersonDto, Person>()
                .ForMember(dest => dest.Translations, opt => opt.MapFrom(src => src.Translations)).ReverseMap();

            CreateMap<AddPersonDto, Person>()
           .ForMember(dest => dest.Translations, opt => opt.MapFrom(src => src.Translations));

            CreateMap<EditPersonDto, Person>()
                .ForMember(dest => dest.Translations, opt => opt.MapFrom(src => src.Translations));

            CreateMap<DeletePersonDto, Person>().ReverseMap();


            //CreateMap<Genre, AddGenreDto>().ReverseMap();
            //CreateMap<Genre, EditGenreDto>().ReverseMap();
            //CreateMap<Genre, ListGenreDto>().ReverseMap();
            //CreateMap<Genre, GetGenreDto>().ReverseMap();

            //AutoMapperProfile for Country
            //      CreateMap<Country, EditCountryDto>().ReverseMap();
            //      CreateMap<Country, AddCountryDto>().ReverseMap();
            //      CreateMap<Country, ListCountryDto>().ReverseMap();

            //      CreateMap<Country, DeleteCountryDto>().ReverseMap();
            //      CreateMap<Country, GetCountryDto>().ReverseMap();

            //      CreateMap<Person, EditPersonDto>().ReverseMap();
            //      CreateMap<Person, AddPersonDto>().ReverseMap();
            //      CreateMap<Person, GetPersonDto>().ReverseMap();
            //      CreateMap<Person, ListPersonDto>().ReverseMap();
            //      CreateMap<Person, DeletePersonDto>().ReverseMap();

            //      CreateMap<SubscriptionPlan, EditSubscriptionPlanDto>().ReverseMap();
            //      CreateMap<SubscriptionPlan, AddSubscriptionPlanDto>().ReverseMap();
            //      CreateMap<SubscriptionPlan, GetSubscriptionPlanDto>().ReverseMap();
            //      CreateMap<SubscriptionPlan, ListSubscriptionPlanDto>().ReverseMap();
            //      CreateMap<SubscriptionPlan, DeleteSubscriptionPlanDto>().ReverseMap();

            //      CreateMap<Comment, AddCommentDto>().ReverseMap();
            //      CreateMap<Comment, GetCommentDto>()
            //      .ForMember(x => x.UserName, opt => opt.MapFrom(src => src.User.Email)).ReverseMap();

            //      CreateMap<MovieRating, AddMovieRating>().ReverseMap();
            //      CreateMap<AddMovieDto, Movie>().ReverseMap();
           // CreateMap<Movie, ListMovieDto>()
           //.ForMember(dest => dest.Director, opt => opt.MapFrom(src => src.People.FirstOrDefault(p => p.PersonType.ToString() == "Director") != null
           //    ? new GetPersonDto { FirstName = src.People.First(p => p.PersonType.ToString() == "Director").Person.FirstName, LastName = src.People.First(p => p.PersonType.ToString() == "Director").Person.LastName }
           //    : null))
           //.ForMember(dest => dest.Actors, opt => opt.MapFrom(src => src.People
           //     .OrderBy(p => p.Order)
           //     .Where(p => p.PersonType.ToString() == "Actor")
           //     .Select(a => new GetPersonDto
           //     {
           //         FirstName = a.Person.FirstName,
           //         LastName = a.Person.LastName
           //     }).ToList()));

            //      CreateMap<Movie, GetMovieDto>()
            //.ForMember(dest => dest.Poster, opt => opt.MapFrom(src => src.Poster.Replace("\\", "/")))
            //.ForMember(dest => dest.Director, opt => opt.MapFrom(src =>
            //    src.People.FirstOrDefault(p => p.PersonType.ToString() == "Director") != null
            //    ? new GetPersonDto
            //    {
            //        FirstName = src.People.First(p => p.PersonType.ToString() == "Director").Person.FirstName,
            //        LastName = src.People.First(p => p.PersonType.ToString() == "Director").Person.LastName
            //    }
            //    : null))
            //.ForMember(dest => dest.Actors, opt => opt.MapFrom(src => src.People
            //    .OrderBy(p => p.Order)
            //    .Where(p => p.PersonType.ToString() == "Actor")
            //    .Select(a => new GetPersonDto
            //    {
            //        FirstName = a.Person.FirstName,
            //        LastName = a.Person.LastName
            //    }).ToList()))
            //.ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country))
            //.ForMember(dest => dest.Comments, opt => opt.MapFrom(src =>
            //    src.Comments != null ? src.Comments.Select(c => new GetCommentDto
            //    {
            //        Id = c.Id,
            //        Text = c.Text
            //    }).ToList() : new List<GetCommentDto>()))
            //.ForMember(dest => dest.DateRealesed, opt => opt.MapFrom(src => src.ReleasedDate))
            //.ReverseMap();

            //      CreateMap<Subscription, GetSubscriptionDto>()
            //      .ForMember(dest => dest.SubscriptionPlanName, opt => opt.MapFrom(src => src.Plan.Name))
            //      .ForMember(dest => dest.Cost, opt => opt.MapFrom(src => src.Plan.Cost));
        }
    }
}
