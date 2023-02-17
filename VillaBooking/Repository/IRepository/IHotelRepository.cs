using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VillaBooking.Models;

namespace VillaBooking.Repository.IRepository
{
    public interface IHotelRepository : IRepository<Hotel>
    {
        Task UpdateAsync(Hotel entity);
    }
}