using Chat.Domain.Models;

namespace Chat.Domain.Interfaces
{
    public interface IUserRepository : IGenericRepository<UserModel>
    {
        Task<UserModel?> GetAsync(string id);
        Task<UserModel?> GetByUserNameAsync(string username);
        Task<UserModel?> UpdateAsync(UserModel entity);
    }
}
