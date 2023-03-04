using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Ultility;
using VillaBookingConsume.Models;
using VillaBookingConsume.Models.Dto;
using VillaBookingConsume.Service.IService;

namespace VillaBookingConsume.Service
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly IHttpClientFactory _client;
        private string _hotelApiUrl;

        public AuthService(IHttpClientFactory client, IConfiguration configuration) : base(client)
        {
            _client = client;
            _hotelApiUrl = string.Format($"{configuration.GetValue<string>("ServiceUrls:HotelApi")}/api/auth");
        }

        public async Task<T> LoginAsync<T>(LoginRequestDto loginRequestDto)
        {
            return await SendAsync<T>(new ApiRequest()
            {
                ApiType = Enumerator.ApiType.POST,
                Data = loginRequestDto,
                Url = string.Format($"{_hotelApiUrl}/login")
            });
        }

        public async Task<T> RegisterAsync<T>(RegistrationDto registrationDto)
        {
            return await SendAsync<T>(new ApiRequest()
            {
                ApiType = Enumerator.ApiType.POST,
                Data = registrationDto,
                Url = string.Format($"{_hotelApiUrl}/register")
            });
        }
    }
}