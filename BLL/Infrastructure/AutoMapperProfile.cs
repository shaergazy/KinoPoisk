using AutoMapper;
using BLL.DTO;
using DAL.Entities;

namespace BLL.Infrastructure
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Genre, GenreDto.Base>().ReverseMap();
            CreateMap<Genre, GenreDto.IdHasBase>().ReverseMap();

            //AutoMapperProfile for Country
            CreateMap<Country, CountryDto.Edit>().ReverseMap();
            CreateMap<Country, CountryDto.Add>().ReverseMap();
            CreateMap<Country, CountryDto.Get>().ReverseMap();
            CreateMap<Country, CountryDto.Delete>().ReverseMap();
        }
    }
}
