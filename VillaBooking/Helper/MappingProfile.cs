using AutoMapper;
using VillaBooking.Models;
using VillaBooking.Models.Dto;
using VillaBookingConsume.Models;
using VillaBookingConsume.Models.Dto.Authentication;

namespace VillaBooking.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Hotel, HotelDto>().ReverseMap()
                .ForMember(x => x.UpdatedDate, o => o.Ignore());
            CreateMap<Hotel, HotelCreateDto>().ReverseMap();
            CreateMap<Hotel, HotelUpdateDto>().ReverseMap();

            CreateMap<Area, AreaDto>().ReverseMap();
            CreateMap<LocalUser, RegistrationDto>().ReverseMap();
        }
    }
}