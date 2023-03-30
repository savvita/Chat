using Chat.DataAccess.UI.Interfaces;
using Chat.DataAccess.UI.Models;
using Chat.Domain.Models;

namespace Chat.DataAccess.UI.Repositories
{
    public class ShoppingRepository : IShoppingRepository
    {
        private readonly UnitOfWorks.UnitOfWorks _db;
        public ShoppingRepository(UnitOfWorks.UnitOfWorks db)
        {
            _db = db;
        }

        public async Task<bool> CreateAsync(string userId, Subscription entity)
        {
            return await _db.BuySubscriptionAsync(userId, (SubscriptionModel)entity);
        }

        public async Task<Shopping?> CreateAsync(Shopping entity)
        {
            var model = await _db.Shoppings.CreateAsync((ShoppingModel)entity);
            return model != null ? new Shopping(model) : null;

        }

        public async Task<IEnumerable<Shopping>> GetAsync()
        {
            return (await _db.Shoppings.GetAsync()).Select(model => new Shopping(model));
        }

        public async Task<Shopping?> GetAsync(int id)
        {
            var model = await _db.Shoppings.GetAsync(id);

            return model != null ? new Shopping(model) : null;
        }

        public async Task<List<Shopping>> GetUserShoppingsAsync(string id)
        {
            return (await _db.Shoppings.GetUserShoppingsAsync(id)).Select(x => new Shopping(x)).ToList();
        }
    }
}
