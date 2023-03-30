using Chat.Domain.Interfaces;
using Chat.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Chat.DataAccess.Repositories
{
    public class BanRepository : GenericRepository, IBanRepository
    {
        public BanRepository(ChatDbContext db) : base(db)
        {
        }

        public async Task<BanModel?> CreateAsync(BanModel entity)
        {
            var res = (await _db.Bans.AddAsync(entity)).Entity;
            await _db.SaveChangesAsync();

            return res;
        }

        public async Task<IEnumerable<BanModel>> GetAsync()
        {
            return await _db.Bans.ToListAsync();
        }
    }
}
