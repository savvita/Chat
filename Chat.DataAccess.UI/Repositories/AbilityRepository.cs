using Chat.DataAccess.UI.Interfaces;
using Chat.DataAccess.UI.Models;
using Chat.Domain.Models;

namespace Chat.DataAccess.UI.Repositories
{
    public class AbilityRepository : IAbilityRepository
    {
        private readonly UnitOfWorks.UnitOfWorks _db;
        public AbilityRepository(UnitOfWorks.UnitOfWorks db)
        {
            _db = db;
        }

        public async Task<Ability?> CreateAsync(Ability entity)
        {
            var model = await _db.Abilities.CreateAsync((AbilityModel)entity);
            return model != null ? new Ability(model) : null;

        }

        public async Task<IEnumerable<Ability>> GetAsync()
        {
            return (await _db.Abilities.GetAsync()).Select(model => new Ability(model));
        }
    }
}
