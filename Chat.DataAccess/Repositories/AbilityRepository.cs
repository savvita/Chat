using Chat.Domain.Interfaces;
using Chat.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Chat.DataAccess.Repositories
{
    public class AbilityRepository : GenericRepository, IAbilityRepository
    {
        public AbilityRepository(ChatDbContext db) : base(db)
        {
        }

        public async Task<AbilityModel?> CreateAsync(AbilityModel entity)
        {
            var res = (await _db.Abilities.AddAsync(entity)).Entity;
            await _db.SaveChangesAsync();

            return res;
        }

        public async Task<IEnumerable<AbilityModel>> GetAsync()
        {
            return await _db.Abilities.ToListAsync();
        }
    }
}
