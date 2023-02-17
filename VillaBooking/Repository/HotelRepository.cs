using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VillaBooking.Data;
using VillaBooking.Models;
using VillaBooking.Repository.IRepository;

namespace VillaBooking.Repository
{
    public class HotelRepository : Repository<Hotel>, IHotelRepository
    {
        private readonly ApplicationDbContext _context;
        public HotelRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task UpdateAsync(Hotel entity)
        {
            entity.UpdatedDate = DateTime.Now;
            _context.Hotels.Update(entity);
            await SaveAsync();
        }
    }
}