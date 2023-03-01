using AutoMapper;
using VillaBookingConsume.Models.Dto;

namespace VillaBookingConsume
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<HotelDto, HotelCreateDto>().ReverseMap();
            CreateMap<HotelDto, HotelUpdateDto>().ReverseMap();
        }
    }
}