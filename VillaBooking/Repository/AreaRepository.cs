using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VillaBooking.Data;
using VillaBooking.Models;
using VillaBooking.Models.Dto;
using VillaBooking.Repository.IRepository;

namespace VillaBooking.Repository
{
    public class AreaRepository : Repository<Area>, IAreaRepository
    {
        private readonly ApplicationDbContext _context;
        internal DbSet<Area> _dbSet;
        public AreaRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task Update(Area area)
        {
            area.UpdateDate = DateTime.Now;
            _context.Areas.Update(area);
            await _context.SaveChangesAsync();
        }
    }
}