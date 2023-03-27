namespace Chat.Domain.Interfaces
{
    public interface IUnitOfWorks : IDisposable
    {
        IAbilityRepository Abilities { get; }
        IRequestRepository Requests { get; }
        IShoppingRepository Shoppings { get; }
        IUserRepository Users { get; }
        ISubscriptionRepository Subscriptions { get; }
    }
}
