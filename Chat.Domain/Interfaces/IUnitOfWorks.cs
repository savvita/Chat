using Chat.Domain.Models;

namespace Chat.Domain.Interfaces
{
    public interface IUnitOfWorks : IDisposable
    {
        IAbilityRepository Abilities { get; }
        IRequestRepository Requests { get; }
        IShoppingRepository Shoppings { get; }
        IUserRepository Users { get; }
        ISubscriptionRepository Subscriptions { get; }

        Task<RequestModel?> CreateRequestAsync(RequestModel entity);
        Task<bool> BuySubscriptionAsync(string userId, SubscriptionModel model);
    }
}
