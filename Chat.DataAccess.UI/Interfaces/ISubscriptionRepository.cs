using Chat.DataAccess.UI.Models;

namespace Chat.DataAccess.UI.Interfaces
{
    public interface ISubscriptionRepository : IGenericRepository<Subscription>
    {
        Task<Subscription?> GetAsync(int id);
    }
}
