using Chat.DataAccess.UI.Exceptions;
using Chat.DataAccess.UI.Interfaces;
using Chat.DataAccess.UI.Models;
using Chat.Domain.Models;
using Microsoft.AspNetCore.Identity;
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

            return new User(model);
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
                return user;
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

            return true;
        }

        public async Task<User?> GetAsync(string id, bool history)
        {
            var model = await _db.Users.GetAsync(id, history);

            return model != null ? new User(model) : null;
        }
    }
}
