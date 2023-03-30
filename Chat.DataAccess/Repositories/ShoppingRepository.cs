using Chat.Domain.Interfaces;
using Chat.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Chat.DataAccess.Repositories
{
    public class ShoppingRepository : GenericRepository, IShoppingRepository
    {
        public ShoppingRepository(ChatDbContext db) : base(db)
        {
        }

        public async Task<ShoppingModel?> CreateAsync(ShoppingModel entity)
        {
            var res = (await _db.Shoppings.AddAsync(entity)).Entity;
            await _db.SaveChangesAsync();

            return res;
        }

        public async Task<IEnumerable<ShoppingModel>> GetAsync()
        {
            return await _db.Shoppings.Include(x => x.Subscription).ToListAsync();
        }

        public Task<ShoppingModel?> GetAsync(int id)
        {
            return _db.Shoppings.Include(x => x.Subscription).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<ShoppingModel>> GetUserShoppingsAsync(string id)
        {
            return await _db.Shoppings.Where(x => x.UserId == id).Include(x => x.Subscription).ToListAsync();
        }
    }
}
