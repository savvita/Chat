using Chat.DataAccess.UI.Interfaces;
using Chat.DataAccess.UI.Repositories;
using Chat.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Chat.DataAccess.UI
{
    public class DbContext
    {
        private readonly ChatDbContext _context;
        public IAbilityRepository Abilities { get; }
        public IRequestRepository Requests{ get; }
        public IShoppingRepository Shoppings { get; }
        public ISubscriptionRepository Subscriptions{ get; }
        public IUserRepository Users { get; }

        public DbContext(ChatDbContext context, UserManager<UserModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            var uow = new UnitOfWorks.UnitOfWorks(context, userManager, roleManager);

            Abilities = new AbilityRepository(uow);
            Requests = new RequestRepository(uow);
            Shoppings = new ShoppingRepository(uow);
            Subscriptions = new SubscriptionRepository(uow);
            Users = new UserRepository(uow);
        }
    }
}
