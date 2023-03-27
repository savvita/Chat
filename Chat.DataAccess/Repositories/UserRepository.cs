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
            var res = (await _db.Users.AddAsync(entity)).Entity;
            await _db.SaveChangesAsync();

            return res;
        }

        public async Task<IEnumerable<UserModel>> GetAsync()
        {
            return await _db.Users
                .Include(x => x.ShoppingHistory)
                .Include(x => x.Subscription)
                .ToListAsync();
        }

        public async Task<UserModel?> GetAsync(string id, bool history = false)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
            if(user == null)
            {
                return null;
            }

            if(history == true)
            {
                _db.Requests
                    .Where(x => x.UserId == id)
                    .ToList()
                    .ForEach(x => user.RequestHistory.Add(x));
                
            }

            return user;
        }
    }
}
