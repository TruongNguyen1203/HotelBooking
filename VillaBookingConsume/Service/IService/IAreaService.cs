using System.Threading.Tasks;
using VillaBookingConsume.Models.Dto;

namespace VillaBookingConsume.Service.IService
{
    public interface IAreaService
    {
        Task<T> GetAllAsync<T>();
        Task<T> GetByIdAsync<T>(int id);
        Task<T> CreateAsync<T>(AreaDto areaDto);
        Task<T> UpdateAsync<T>(AreaDto areaDto);
        Task<T> DeleteAsync<T>(int id);
    }
}