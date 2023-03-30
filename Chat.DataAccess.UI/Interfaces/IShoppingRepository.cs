using Chat.DataAccess.UI.Models;

namespace Chat.DataAccess.UI.Interfaces
{
    public interface IShoppingRepository : IGenericRepository<Shopping>
    {
        Task<Shopping?> GetAsync(int id);
        Task<bool> CreateAsync(string userId, Subscription entity);
        Task<List<Shopping>> GetUserShoppingsAsync(string id);
    }
}
