using AutoMapper;
using Common.DTO;
using DAL.Entities;

namespace Common.Infrastructure
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Genre, GenreDto.Base>().ReverseMap();
            CreateMap<Genre, GenreDto.IdHasBase>().ReverseMap();
        }
    }
}
