using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text;
using System.Text.Unicode;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Ultility;
using VillaBookingConsume.Models;
using VillaBookingConsume.Service.IService;

namespace VillaBookingConsume.Service
{
    public class BaseService : IBaseService
    {
        public BaseService(IHttpClientFactory httpClient)
        {
            HttpClient = httpClient;
        }

        public IHttpClientFactory HttpClient { get; set; }

        public async Task<T> SendAsync<T>(ApiRequest apiRequest)
        {
            try
            {
                var client = HttpClient.CreateClient("HotelApi");
                HttpRequestMessage message = new HttpRequestMessage();
                message.Headers.Add("Accept", "appilcation/json");
                message.RequestUri = new Uri(apiRequest.Url);
                if (apiRequest.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data), Encoding.UTF8,
                        "application/json");
                }

                switch (apiRequest.ApiType)
                {
                    case Enumerator.ApiType.GET:
                    {
                        message.Method = HttpMethod.Get;
                        break;
                    }
                    case Enumerator.ApiType.POST:
                    {
                        message.Method = HttpMethod.Post;
                        break;
                    }
                    case Enumerator.ApiType.PUT:
                    {
                        message.Method = HttpMethod.Put;
                        break;
                    }
                    case Enumerator.ApiType.DELETE:
                    {
                        message.Method = HttpMethod.Delete;
                        break;
                    }
                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }

                HttpResponseMessage apiResponse = new();

                apiResponse = await client.SendAsync(message);

                var apiContent = await apiResponse.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<T>(apiContent);

                return response;
            }
            catch (Exception ex)
            {
                var dto = new ApiResponse()
                {
                    IsSuccess = false,
                    ErrorMessages = new List<string> {Convert.ToString(ex.Message)}
                };

                var res = JsonConvert.SerializeObject(dto);
                var apiResponse = JsonConvert.DeserializeObject<T>(res);
                return apiResponse;
            }
        }
    }
}