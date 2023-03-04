using System.Security.AccessControl;
using Ultility;

namespace VillaBookingConsume.Models
{
    public class ApiRequest
    {
        public Enumerator.ApiType ApiType { get; set; }
        public string Url { get; set; }
        public object Data { get; set; }
        public string Token { get; set; }
    }
}