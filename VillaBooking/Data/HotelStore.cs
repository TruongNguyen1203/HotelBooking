using System.Collections.Generic;
using VillaBooking.Models.Dto;

namespace VillaBooking.Data
{
    public static class HotelStore
    {
        public static List<HotelDto> HotelDtos = new List<HotelDto>
        {
            new HotelDto() {Id = 1, Name = "Hotel 1"},
            new HotelDto() {Id = 2, Name = "Hotel 2"}
        };
    }
}