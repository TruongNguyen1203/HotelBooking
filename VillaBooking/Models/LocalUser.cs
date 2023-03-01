using System.ComponentModel.DataAnnotations;

namespace VillaBookingConsume.Models
{
    public class LocalUser
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}