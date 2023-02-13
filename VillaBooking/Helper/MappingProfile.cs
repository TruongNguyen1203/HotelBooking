using AutoMapper;
using VillaBooking.Models;
using VillaBooking.Models.Dto;

namespace VillaBooking.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Hotel, HotelDto>().ReverseMap()
                .ForMember(x => x.UpdatedDate, o => o.Ignore());
        }
    }
}