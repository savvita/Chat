using Chat.Domain.Interfaces;
using Chat.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Chat.DataAccess.Repositories
{
    public class RequestRepository : GenericRepository, IRequestRepository
    {
        public RequestRepository(ChatDbContext db) : base(db)
        {
        }

        public async Task<RequestModel?> CreateAsync(RequestModel entity)
        {
            var res = (await _db.Requests.AddAsync(entity)).Entity;
            await _db.SaveChangesAsync();

            return res;
        }

        public async Task<IEnumerable<RequestModel>> GetAsync()
        {
            return await _db.Requests.ToListAsync();
        }

        public Task<RequestModel?> GetAsync(int id)
        {
            return _db.Requests.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
