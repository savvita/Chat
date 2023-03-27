using Chat.DataAccess.UI.Interfaces;
using Chat.DataAccess.UI.Models;
using Chat.Domain.Models;

namespace Chat.DataAccess.UI.Repositories
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly UnitOfWorks.UnitOfWorks _db;
        public SubscriptionRepository(UnitOfWorks.UnitOfWorks db)
        {
            _db = db;
        }

        public async Task<Subscription?> CreateAsync(Subscription entity)
        {
            var model = await _db.Subscriptions.CreateAsync((SubscriptionModel)entity);
            return model != null ? new Subscription(model) : null;

        }

        public async Task<IEnumerable<Subscription>> GetAsync()
        {
            return (await _db.Subscriptions.GetAsync()).Select(model => new Subscription(model));
        }

        public async Task<Subscription?> GetAsync(int id)
        {
            var model = await _db.Subscriptions.GetAsync(id);

            return model != null ? new Subscription(model) : null;
        }
    }
}
