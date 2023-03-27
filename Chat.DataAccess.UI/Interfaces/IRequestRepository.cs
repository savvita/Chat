using Chat.DataAccess.UI.Models;

namespace Chat.DataAccess.UI.Interfaces
{
    public interface IRequestRepository : IGenericRepository<Request>
    {
        Task<Request?> GetAsync(int id);
    }
}
