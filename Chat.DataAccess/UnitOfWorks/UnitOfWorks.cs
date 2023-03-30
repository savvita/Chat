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
        public IBanRepository Bans { get; }
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
            Bans = new BanRepository(context);

            UserManager = userManager;
            Roles = roleManager;
        }

        public async Task<RequestModel?> CreateRequestAsync(RequestModel entity)
        {
            var user = await Users.GetAsync(entity.UserId);

            if(user == null)
            {
                return null;
            }

            var now = DateTime.Now;

            if(user.BannedUntil != null)
            {
                if(user.BannedUntil > now)
                {
                    return null;
                }
                user.BannedUntil = null;
            }

            if(user.LastRequestDate != null)
            {
                if(((DateTime)user.LastRequestDate).Day != now.Day)
                {
                    user.Requests = 0;
                }

                if(user.Subscription == null || user.Subscription.MaxCount != null && user.Requests >= user.Subscription.MaxCount)
                {
                    return null;
                }
            }

            user.LastRequestDate = now;
            user.Requests++;

            await Users.UpdateAsync(user);

            return await Requests.CreateAsync(entity);   
        }


        public async Task<bool> BuySubscriptionAsync(string userId, SubscriptionModel model)
        {
            var user = await Users.GetAsync(userId);

            if (user == null)
            {
                return false;
            }

            var now = DateTime.Now;

            if (user.BannedUntil != null)
            {
                if (user.BannedUntil > now)
                {
                    return false;
                }
                user.BannedUntil = null;
            }

            if (user.LastRequestDate != null)
            {
                if (((DateTime)user.LastRequestDate).Day != now.Day)
                {
                    user.Requests = 0;
                }
            }

            user.SubscriptionId = model.Id;
            await Users.UpdateAsync(user);

            var res = await Shoppings.CreateAsync(new ShoppingModel
            {
                Date = now,
                Price = model.Price,
                SubscriptionId = model.Id,
                UserId = userId
            });

            return res != null;
        }
        public async void Dispose()
        {
            await _db.SaveChangesAsync();
            _db.Dispose();
        }
    }
}
