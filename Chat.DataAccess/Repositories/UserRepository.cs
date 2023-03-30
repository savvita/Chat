using Chat.Domain.Interfaces;
using Chat.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Chat.DataAccess.Repositories
{
    public class UserRepository : GenericRepository, IUserRepository
    {
        public UserRepository(ChatDbContext db) : base(db)
        {
        }

        public async Task<UserModel?> CreateAsync(UserModel entity)
        {
            entity.SubscriptionId = 1;
            var res = (await _db.Users.AddAsync(entity)).Entity;
            await _db.SaveChangesAsync();

            return res;
        }

        public async Task<IEnumerable<UserModel>> GetAsync()
        {
            return await _db.Users
                .Include(x => x.Subscription)
                .ToListAsync();
        }

        public async Task<UserModel?> GetAsync(string id)
        {
            return await _db.Users.Include(x => x.Subscription).ThenInclude(x => x.Abilities).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<UserModel?> GetByUserNameAsync(string username)
        {
            return await Task.FromResult<UserModel?>(_db.Users.Include(x => x.Subscription).ThenInclude(x => x.Abilities).FirstOrDefault(u => u.UserName.Equals(username)));
        }

        public async Task<UserModel?> UpdateAsync(UserModel entity)
        {
            var res = _db.Users.Update(entity);

            await _db.SaveChangesAsync();

            return res.Entity;
        }
    }
}
