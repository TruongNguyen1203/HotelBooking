using System.Threading.Tasks;
using VillaBookingConsume.Models.Dto;

namespace VillaBookingConsume.Service.IService
{
    public interface IAuthService
    {
        Task<T> LoginAsync<T>(LoginRequestDto loginRequestDto);
        Task<T> RegisterAsync<T>(RegistrationDto registrationDto);
    }
}