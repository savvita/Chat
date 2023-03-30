using Chat.DataAccess.UI.Interfaces;
using Chat.DataAccess.UI.Models;
using Chat.Domain.Models;

namespace Chat.DataAccess.UI.Repositories
{
    public class RequestRepository : IRequestRepository
    {
        private readonly UnitOfWorks.UnitOfWorks _db;
        public RequestRepository(UnitOfWorks.UnitOfWorks db)
        {
            _db = db;
        }

        public async Task<Request?> CreateAsync(Request entity)
        {
            var model = await _db.Requests.CreateAsync((RequestModel)entity);
            return model != null ? new Request(model) : null;

        }

        public async Task<IEnumerable<Request>> GetAsync()
        {
            return (await _db.Requests.GetAsync()).Select(model => new Request(model));
        }

        public async Task<Request?> GetAsync(int id)
        {
            var model = await _db.Requests.GetAsync(id);

            return model != null ? new Request(model) : null;
        }

        public async Task<List<Request>> GetUserRequestsAsync(string userId)
        {
            return (await _db.Requests.GetUserRequestsAsync(userId)).Select(model => new Request(model)).ToList();
        }
    }
}
