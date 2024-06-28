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
        }
    }
}
