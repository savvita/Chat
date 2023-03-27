using Chat.DataAccess.UI.Models;

namespace Chat.DataAccess.UI.Interfaces
{
    public interface IShoppingRepository : IGenericRepository<Shopping>
    {
        Task<Shopping?> GetAsync(int id);
    }
}
