using AutoMapper;
using BLL.DTO;
using BLL.DTO.Country;
using BLL.DTO.Genre;
using BLL.DTO.Movie;
using BLL.DTO.Person;
using KinopoiskWeb.DataTables;
using KinopoiskWeb.ViewModels;
using KinopoiskWeb.ViewModels.Country;
using KinopoiskWeb.ViewModels.Genre;
using KinopoiskWeb.ViewModels.Movie;
using KinopoiskWeb.ViewModels.Person;

namespace KinopoiskWeb.Infrastructure
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<GenreVM, AddGenreDto>().ReverseMap();
            CreateMap<GenreVM, ListGenreDto>().ReverseMap();
            CreateMap<GenreVM, GetGenreDto>().ReverseMap();
            CreateMap<GenreVM, EditGenreDto>().ReverseMap();

            CreateMap<TranslationVM, TranslationDto>().ReverseMap();

            CreateMap<CountryVM, AddCountryDto>().ReverseMap();
            CreateMap<CountryVM, ListCountryDto>().ReverseMap();
            CreateMap<CountryVM, GetCountryDto>().ReverseMap();
            CreateMap<CountryVM, EditCountryDto>().ReverseMap();

            CreateMap<PersonVM, AddPersonDto>().ReverseMap();
            CreateMap<IndexPersonVM, ListPersonDto>().ReverseMap();
            CreateMap<IndexPersonVM, GetPersonDto>().ReverseMap();
            CreateMap<PersonVM, EditPersonDto>().ReverseMap();

            // CreateMap<SubscriptionPlanVM, AddSubscriptionPlanDto>().ReverseMap();
            // CreateMap<SubscriptionPlanVM, ListSubscriptionPlanDto>().ReverseMap();
            // CreateMap<SubscriptionPlanVM, GetSubscriptionPlanDto>().ReverseMap();
            // CreateMap<SubscriptionPlanVM, EditSubscriptionPlanDto>().ReverseMap();

            CreateMap<DataTables.DataTablesRequest, BLL.DTO.DataTablesRequestDto>()
            .ForMember(dest => dest.SortColumn, opt => opt.MapFrom(src => src.Columns.ElementAt(src.Order.ElementAt(0).Column).Name))
            .ForMember(dest => dest.SortDirection, opt => opt.MapFrom(src => src.Order.ElementAt(0).Dir.ToLower()))
            .ForMember(dest => dest.SearchTerm, opt => opt.MapFrom(src => src.Search.Value)).ReverseMap();

            CreateMap<MovieDataTablesRequest, MovieDataTablesRequestDto>()
            .ForMember(dest => dest.SortColumn, opt => opt.MapFrom(src => src.Columns.ElementAt(src.Order.ElementAt(0).Column).Name))
            .ForMember(dest => dest.SortDirection, opt => opt.MapFrom(src => src.Order.ElementAt(0).Dir.ToLower()))
            .ForMember(dest => dest.SearchTerm, opt => opt.MapFrom(src => src.Search.Value)).ReverseMap();

            CreateMap(typeof(DataTablesResponseVM<>), typeof(BLL.DTO.DataTablesResponse<>)).ReverseMap();

            CreateMap<CreateMovieVM, AddMovieDto>()
            .ForMember(dest => dest.Actors, opt => opt.MapFrom(src => src.Actors))
            .ForMember(dest => dest.GenreIds, opt => opt.MapFrom(src => src.GenreIds ?? new List<Guid>()));

            //TODO: Fix mapping
            CreateMap<ActorVM, MoviePersonDto>()
            .ForMember(dest => dest.PersonId, opt => opt.MapFrom(src => src.PersonId))
            .ForMember(dest => dest.Order, opt => opt.MapFrom(src => src.Order));
            CreateMap<ListMovieDto, IndexMovieVM>()
           .ForMember(dest => dest.Director, opt => opt.MapFrom(src => src.Director != null ? src.Director : string.Empty))
           .ForMember(dest => dest.Actors, opt => opt.MapFrom(src => src.Actors))
           .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating));

            CreateMap<GetMovieDto, DetailsMovieVM>()
           .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments != null ? src.Comments.Select(c => c.Text).ToArray() : new string[0]))
           .ForMember(dest => dest.ReleasedDate, opt => opt.MapFrom(src => src.DateReleased))
           .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating));

            CreateMap<RateMovieVM, AddMovieRating>().ReverseMap();

            // CreateMap<PaymentDetailsDto, PaymentDetailsVM>().ReverseMap();

            // CreateMap<GetSubscriptionDto, SubscriptionVM>().ReverseMap();

            CreateMap<AddCommentVM, AddCommentDto>()
            .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.CommentText)).ReverseMap();

        }
    }
}
