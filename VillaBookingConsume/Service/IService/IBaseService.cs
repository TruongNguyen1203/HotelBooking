using System.Threading.Tasks;
using VillaBookingConsume.Models;

namespace VillaBookingConsume.Service.IService
{
    public interface IBaseService
    {
        Task<T> SendAsync<T>(ApiRequest apiRequest);
    }
}