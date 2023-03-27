using Chat.DataAccess.Repositories;
using Chat.Domain.Interfaces;
using Chat.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Chat.DataAccess.UnitOfWorks
{
    public class UnitOfWorks : IUnitOfWorks
    {
        private readonly ChatDbContext _db;
        public IAbilityRepository Abilities { get; }
        public IRequestRepository Requests { get; }
        public IShoppingRepository Shoppings { get; }
        public IUserRepository Users { get; }
        public ISubscriptionRepository Subscriptions { get; }
        public UserManager<UserModel> UserManager { get; }
        public RoleManager<IdentityRole> Roles { get; }

        public UnitOfWorks(ChatDbContext context, UserManager<UserModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = context;
            Abilities = new AbilityRepository(context);
            Requests = new RequestRepository(context);
            Shoppings = new ShoppingRepository(context);
            Users = new UserRepository(context);
            Subscriptions = new SubscriptionRepository(context);

            UserManager = userManager;
            Roles = roleManager;
        }

        public async void Dispose()
        {
            await _db.SaveChangesAsync();
            _db.Dispose();
        }
    }
}
