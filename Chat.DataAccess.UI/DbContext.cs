using Chat.DataAccess.UI.Exceptions;
using Chat.DataAccess.UI.Interfaces;
using Chat.DataAccess.UI.Models;
using Chat.DataAccess.UI.Repositories;
using Chat.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Chat.DataAccess.UI
{
    public class DbContext
    {
        private readonly UnitOfWorks.UnitOfWorks _uow;
        public IAbilityRepository Abilities { get; }
        public IRequestRepository Requests{ get; }
        public IShoppingRepository Shoppings { get; }
        public ISubscriptionRepository Subscriptions{ get; }
        public IUserRepository Users { get; }
        public IBanRepository Bans { get; }

        public DbContext(ChatDbContext context, UserManager<UserModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            _uow = new UnitOfWorks.UnitOfWorks(context, userManager, roleManager);

            Abilities = new AbilityRepository(_uow);
            Requests = new RequestRepository(_uow);
            Shoppings = new ShoppingRepository(_uow);
            Subscriptions = new SubscriptionRepository(_uow);
            Users = new UserRepository(_uow);
            Bans = new BanRepository(_uow);
        }

        public async Task<Request?> CreateRequestAsync(Request entity)
        {
            var user = await Users.GetAsync(entity.UserId);

            if (user == null)
            {
                throw new UserNotFoundException(entity.UserId);
            }

            var now = DateTime.Now;

            if (user.BannedUntil != null)
            {
                if (user.BannedUntil > now)
                {
                    throw new BannedUserException(user.Id);
                }
            }

            if(user.Subscription == null)
            {
                throw new SubscriptionNotFoundException();
            }

            if(user.Subscription.MaxCount != null && user.Requests >= user.Subscription.MaxCount)
            {
                throw new MaxRequestCountException((int)user.Subscription.MaxCount);
            }

            var model = await _uow.CreateRequestAsync((RequestModel)entity);
            return model != null ? new Request(model) : null;
        }
    }
}
