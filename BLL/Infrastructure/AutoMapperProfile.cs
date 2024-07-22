using AutoMapper;
using BLL.DTO.Country;
using BLL.DTO.Genre;
using BLL.DTO.Movie;
using BLL.DTO.Person;
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
            CreateMap<Movie, ListMovieDto>()
           .ForMember(dest => dest.Director, opt => opt.MapFrom(src => src.People.FirstOrDefault(p => p.PersonType.ToString() == "Director") != null
               ? new GetPersonDto { FirstName = src.People.First(p => p.PersonType.ToString() == "Director").Person.FirstName, LastName = src.People.First(p => p.PersonType.ToString() == "Director").Person.LastName }
               : null))
           .ForMember(dest => dest.Actors, opt => opt.MapFrom(src => src.People
                .OrderBy(p => p.Order)
                .Where(p => p.PersonType.ToString() == "Actor")
                .Select(a => new GetPersonDto
                {
                    FirstName = a.Person.FirstName,
                    LastName = a.Person.LastName
                }).ToList()))
           .ForMember(dest => dest.Rating, opt => opt.MapFrom(src =>
                src.Ratings != null && src.Ratings.Any() ? src.Ratings.Average(r => r.StarCount) : 0f));

            CreateMap<Movie, GetMovieDto>()
                 .ForMember(dest => dest.Poster, opt => opt.MapFrom(src => "/" + src.Poster.Replace("\\", "/")))
                    .ForMember(dest => dest.Director, opt => opt.MapFrom(src => src.People.FirstOrDefault(p => p.PersonType.ToString() == "Director") != null
               ? new GetPersonDto { FirstName = src.People.First(p => p.PersonType.ToString() == "Director").Person.FirstName, LastName = src.People.First(p => p.PersonType.ToString() == "Director").Person.LastName }
               : null))
           .ForMember(dest => dest.Actors, opt => opt.MapFrom(src => src.People
                .OrderBy(p => p.Order)
                .Where(p => p.PersonType.ToString() == "Actor")
                .Select(a => new GetPersonDto
                {
                    FirstName = a.Person.FirstName,
                    LastName = a.Person.LastName
                }).ToList()))

           .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country))

           .ForMember(dest => dest.Genres, opt => opt.MapFrom(src =>
                 src.Genres != null ? src.Genres.Select(g => new GetGenreDto
                 {
                     Id = g.GenreId,
                     Name = g.Genre != null ? g.Genre.Name : string.Empty
                 }).ToList() : new List<GetGenreDto>()))
           .ForMember(dest => dest.Comments, opt => opt.MapFrom(src =>
                 src.Comments != null ? src.Comments.Select(c => new GetCommentDto
                 {
                     Id = c.Id,
                     Text = c.Text
                 }).ToList() : new List<GetCommentDto>()))
            .ForMember(dest => dest.Rating, opt => opt.MapFrom(src =>
                src.Ratings != null && src.Ratings.Any() ? src.Ratings.Average(r => r.StarCount) : 0f))
            .ForMember(dest => dest.DateRealesed, opt => opt.MapFrom(src => src.ReleasedDate))
            .ReverseMap();


        }
    }
}
