using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Ultility;
using VillaBookingConsume.Models;
using VillaBookingConsume.Models.Dto;
using VillaBookingConsume.Service.IService;

namespace VillaBookingConsume.Service
{
    public class AreaService : BaseService, IAreaService
    {
        private readonly IHttpClientFactory _client;
        private string _hotelApiUrl;
        public AreaService(IHttpClientFactory httpClient, IHttpClientFactory client, IConfiguration configuration) : base(httpClient)
        {
            _client = client;
            _hotelApiUrl = string.Format($"{configuration.GetValue<string>("ServiceUrls:HotelApi")}/api/areas");
        }

        public async Task<T> GetAllAsync<T>()
        {
            return await SendAsync<T>(new ApiRequest()
            {
                Url = _hotelApiUrl,
                ApiType = Enumerator.ApiType.GET,
            });
        }

        public async Task<T> GetByIdAsync<T>(int id)
        {
            return await SendAsync<T>(new ApiRequest()
            {
                Url = string.Format($"{_hotelApiUrl}/id?id={id}"),
                ApiType = Enumerator.ApiType.GET,
            });
        }

        public async Task<T> CreateAsync<T>(AreaDto areaDto)
        {
            return await SendAsync<T>(new ApiRequest()
            {
                ApiType = Enumerator.ApiType.POST,
                Url = _hotelApiUrl,
                Data = areaDto
            });
        }

        public async Task<T> UpdateAsync<T>(AreaDto areaDto)
        {
            return await SendAsync<T>(new ApiRequest()
            {
                ApiType = Enumerator.ApiType.PUT,
                Url = string.Format($"{_hotelApiUrl}?id={areaDto.Id}") ,
                Data = areaDto
            });
        }

        public async Task<T> DeleteAsync<T>(int id)
        {
            return await SendAsync<T>(new ApiRequest()
            {
                ApiType = Enumerator.ApiType.DELETE,
                Url = String.Format($"{_hotelApiUrl}/id?id={id}")
            });
        }
    }
}