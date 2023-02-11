using System.ComponentModel.DataAnnotations;

namespace VillaBooking.Models.Dto
{
    public class HotelDto
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        public int Sqft { get; set; }
        public int Ocupation { get; set; }
    }
}