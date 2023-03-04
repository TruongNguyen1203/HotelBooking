using System.Threading.Tasks;
using VillaBookingConsume.Models.Dto;

namespace VillaBookingConsume.Service.IService
{
    public interface IHotelService
    {
        Task<T> GetAllAsync<T>();
        Task<T> GetByIdAsync<T>(int id);
        Task<T> CreateAsync<T>(HotelCreateDto hotelCreateDto, string token);
        Task<T> UpdateAsync<T>(HotelUpdateDto hotelUpdateDto, string token);
        Task<T> DeleteAsync<T>(int id, string token);
    }
}