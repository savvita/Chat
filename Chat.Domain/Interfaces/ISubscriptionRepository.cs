using Chat.Domain.Models;

namespace Chat.Domain.Interfaces
{
    public interface ISubscriptionRepository : IGenericRepository<SubscriptionModel>
    {
        Task<SubscriptionModel?> GetAsync(int id);
    }
}
