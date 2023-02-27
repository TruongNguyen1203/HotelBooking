using System.Threading.Tasks;
using VillaBookingConsume.Models.Dto;

namespace VillaBookingConsume.Service.IService
{
    public interface IHotelService
    {
        Task<T> GetAllAsync<T>();
        Task<T> GetByIdAsync<T>(int id);
        Task<T> CreateAsync<T>(HotelCreateDto hotelCreateDto);
        Task<T> UpdateAsync<T>(HotelUpdateDto hotelUpdateDto);
        Task<T> DeleteAsync<T>(int id);
    }
}