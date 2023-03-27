using Chat.Domain.Models;

namespace Chat.Domain.Interfaces
{
    public interface IUserRepository : IGenericRepository<UserModel>
    {
        Task<UserModel?> GetAsync(string id, bool history);
    }
}
