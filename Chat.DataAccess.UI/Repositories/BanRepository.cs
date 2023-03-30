using Chat.DataAccess.UI.Interfaces;
using Chat.DataAccess.UI.Models;
using Chat.Domain.Models;

namespace Chat.DataAccess.UI.Repositories
{
    public class BanRepository : IBanRepository
    {
        private readonly UnitOfWorks.UnitOfWorks _db;
        public BanRepository(UnitOfWorks.UnitOfWorks db)
        {
            _db = db;
        }

        public async Task<Ban?> CreateAsync(Ban entity)
        {
            var model = await _db.Bans.CreateAsync((BanModel)entity);
            return model != null ? new Ban(model) : null;

        }

        public async Task<IEnumerable<Ban>> GetAsync()
        {
            return (await _db.Bans.GetAsync()).Select(model => new Ban(model));
        }
    }
}
