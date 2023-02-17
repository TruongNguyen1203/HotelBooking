using System.Threading.Tasks;
using VillaBooking.Models;
using VillaBooking.Models.Dto;

namespace VillaBooking.Repository.IRepository
{
    public interface IAreaRepository : IRepository<Area>
    {
        Task Update(Area area);
    }
}