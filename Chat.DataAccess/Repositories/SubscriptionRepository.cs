using Chat.Domain.Interfaces;
using Chat.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Chat.DataAccess.Repositories
{
    public class SubscriptionRepository : GenericRepository, ISubscriptionRepository
    {
        public SubscriptionRepository(ChatDbContext db) : base(db)
        {
        }

        public async Task<SubscriptionModel?> CreateAsync(SubscriptionModel entity)
        {
            var res = (await _db.Subscriptions.AddAsync(entity)).Entity;
            await _db.SaveChangesAsync();

            return res;
        }

        public async Task<IEnumerable<SubscriptionModel>> GetAsync()
        {
            return await _db.Subscriptions.ToListAsync();
        }

        public Task<SubscriptionModel?> GetAsync(int id)
        {
            return _db.Subscriptions.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
