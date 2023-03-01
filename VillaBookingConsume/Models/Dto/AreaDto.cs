using System.ComponentModel.DataAnnotations;

namespace VillaBookingConsume.Models.Dto
{
    public class AreaDto
    {
        [Required] public int Id { get; set; }
        [Required] public int HotelId { get; set; }
        public string SpecialDetails { get; set; }
    }
}