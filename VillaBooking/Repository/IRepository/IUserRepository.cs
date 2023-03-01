using System.Threading.Tasks;
using VillaBookingConsume.Models;
using VillaBookingConsume.Models.Dto.Authentication;

namespace VillaBooking.Repository.IRepository
{
    public interface IUserRepository
    {
        Task<bool> IsUniqueUser(string username);
        Task<LoginReponseDto> Login(LoginRequestDto loginRequestDto);
        Task<LocalUser?> Register(RegistrationDto registrationDto);
    }
}