using Chat.DataAccess.UI.Exceptions;
using Chat.DataAccess.UI.Interfaces;
using Chat.DataAccess.UI.Models;
using Chat.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;

namespace Chat.DataAccess.UI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UnitOfWorks.UnitOfWorks _db;
        public UserRepository(UnitOfWorks.UnitOfWorks db)
        {
            _db = db;
        }

        public async Task<User?> CreateAsync(User entity)
        {
            var user = await _db.UserManager.FindByNameAsync(entity.UserName);

            if (user != null)
            {
                throw new LoginConflictException();
            }

            var model = await _db.Users.CreateAsync((UserModel)entity);

            return model != null ? new User(model) : null;
        }

        public async Task<IEnumerable<User>> GetAsync()
        {
            var users = (await _db.Users.GetAsync()).Select(async u =>
            {
                var user = new User(u);
                user.IsAdmin = await _db.UserManager.IsInRoleAsync(u, UserRoles.Admin);

                return user;
            }).Select(u => u.Result).ToList();

            return users;
        }

        public async Task<User?> CreateAsync(LoginModel entity, IEnumerable<string> roles)
        {
            var user = await _db.UserManager.FindByNameAsync(entity.UserName);

            if (user != null)
            {
                throw new LoginConflictException();
            }

            UserModel model = new UserModel()
            {
                UserName = entity.UserName,
                BannedUntil = null,
                LastRequestDate = null,
                SubscriptionId = 1,
                Requests = 0,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _db.UserManager.CreateAsync(model, entity.Password);

            if (!result.Succeeded)
            {
                throw new AuthorizationException();
            }

            foreach (var role in roles)
            {
                await AddToRoleAsync(model, role);
            }

            var free = await _db.Subscriptions.GetAsync(1);
            if (free == null)
            {
                throw new SubscriptionNotFoundException();
            }
            await _db.BuySubscriptionAsync(model.Id, free);

            var res = await _db.Users.GetAsync(model.Id);

            if (res == null)
            {
                throw new InternalServerException();
            }

            return new User(res);
        }

        private async Task AddToRoleAsync(UserModel user, string role)
        {
            if (!await _db.Roles.RoleExistsAsync(role))
            {
                await _db.Roles.CreateAsync(new IdentityRole(role));
            }

            if (await _db.Roles.RoleExistsAsync(role))
                await _db.UserManager.AddToRoleAsync(user, role);
        }

        public async Task<IEnumerable<string>> GetRolesAsync(UserModel entity)
        {
            return (await _db.UserManager.GetRolesAsync(entity)).ToList();
        }

        public async Task<UserModel?> CheckCredentialsAsync(LoginModel model)
        {
            var user = await _db.UserManager.FindByNameAsync(model.UserName);

            if (user != null && await _db.UserManager.CheckPasswordAsync(user, model.Password))
            {
                await CheckDatesAsync(user);
                return await _db.Users.GetAsync(user.Id);
            }

            return null;
        }

        public async Task<bool> CheckUserAsync(IIdentity? identity)
        {
            if (identity == null || identity.Name == null)
            {
                throw new ArgumentNullException();
            }

            var user = await _db.UserManager.FindByNameAsync(identity.Name);

            if (user == null)
            {
                throw new UserNotFoundException(identity.Name);
            }

            if (user.BannedUntil != null && user.BannedUntil > DateTime.Now)
            {
                throw new BannedUserException(user.Id);
            }

            await CheckDatesAsync(user);

            return true;
        }

        private async Task CheckDatesAsync(UserModel user)
        {
            var now = DateTime.Now;
            bool isChanged = false;

            if (user.BannedUntil != null && user.BannedUntil <= now)
            {
                user.BannedUntil = null;
                isChanged = true;
            }

            if (user.LastRequestDate != null && ((DateTime)user.LastRequestDate).Day != now.Day && user.Requests != 0)
            {
                user.Requests = 0;
                isChanged = true;
            }

            if (isChanged)
            {
                await _db.Users.UpdateAsync(user);
            }
        }

        public async Task<User?> GetAsync(string id)
        {
            var model = await _db.Users.GetAsync(id);

            return model != null ? new User(model) : null;
        }

        public async Task<User?> GetByUserNameAsync(string username)
        {
            var model = await _db.Users.GetByUserNameAsync(username);

            if (model == null)
            {
                return null;
            }

            var user = new User(model);
            user.IsAdmin = await _db.UserManager.IsInRoleAsync(model, UserRoles.Admin);

            return user;
        }

        public async Task<bool> BanAsync(string id, Ban? ban)
        {
            var model = await _db.Users.GetAsync(id);
            if (model == null)
            {
                return false;
            }

            if (ban == null)
            {
                model.BannedUntil = null;
            }
            else
            {
                switch (ban.Units)
                {
                    case "day":
                        if (ban.Value != null) model.BannedUntil = DateTime.Now.AddDays((int)ban.Value);
                        break;
                    case "week":
                        if (ban.Value != null) model.BannedUntil = DateTime.Now.AddDays((int)ban.Value * 7);
                        break;
                    case "month":
                        if (ban.Value != null) model.BannedUntil = DateTime.Now.AddMonths((int)ban.Value);
                        break;
                    case "year":
                        if (ban.Value != null) model.BannedUntil = DateTime.Now.AddYears((int)ban.Value);
                        break;
                    default:
                        model.BannedUntil = DateTime.Now.AddYears(1000);
                        break;
                }
            }

            var res = await _db.Users.UpdateAsync(model);
            return res != null;
        }

        public async Task<List<Claim>> GetClaimsAsync(UserModel user)
        {
            if (user.BannedUntil != null && user.BannedUntil > DateTime.Now)
            {
                throw new BannedUserException(user.Id);
            }

            var roles = (await GetRolesAsync(user)).ToList();

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("left", user.Subscription != null && user.Subscription.MaxCount != null ? (user.Subscription.MaxCount - user.Requests).ToString()! : "")
                };

            roles.ForEach(role => claims.Add(new Claim(ClaimTypes.Role, role)));
            if (user.Subscription != null)
            {
                claims.Add(new Claim("subscription", user.Subscription.Id.ToString()));
                user.Subscription.Abilities
                    .ToList()
                    .ForEach(entity => claims.Add(new Claim("ability", entity.Id.ToString())));
            }

            return claims;
        }

        public async Task<User?> UpdateAsync(User entity)
        {
            var model = await _db.UserManager.FindByIdAsync(entity.Id);

            if (model == null)
            {
                throw new UserNotFoundException(entity.Id);
            }
            var user = await _db.UserManager.FindByNameAsync(entity.UserName);

            if (user != null && model.Id != user.Id)
            {
                throw new LoginConflictException();
            }

            model.UserName = entity.UserName;
            model.BannedUntil = entity.BannedUntil;           

            await _db.Users.UpdateAsync(model);

            if (!await _db.Roles.RoleExistsAsync(UserRoles.Admin))
            {
                await _db.Roles.CreateAsync(new IdentityRole(UserRoles.Admin));
            }


            if (entity.IsAdmin)
            {
                if (await _db.Roles.RoleExistsAsync(UserRoles.Admin))
                    await _db.UserManager.AddToRoleAsync(model, UserRoles.Admin);
            }
            else
            {
                if (await _db.Roles.RoleExistsAsync(UserRoles.Admin))
                    await _db.UserManager.RemoveFromRoleAsync(model, UserRoles.Admin);
            }

            return entity;
        }
    }
}