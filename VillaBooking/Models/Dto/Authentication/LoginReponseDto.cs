namespace VillaBookingConsume.Models.Dto.Authentication
{
    public class LoginReponseDto
    {
        public LocalUser User { get; set; }
        public string Token { get; set; }
        
    }
}