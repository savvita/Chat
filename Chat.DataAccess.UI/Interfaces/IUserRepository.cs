using Chat.DataAccess.UI.Models;
using Chat.Domain.Models;
using System.Security.Principal;

namespace Chat.DataAccess.UI.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetAsync(string id, bool history);

        Task<User?> CreateAsync(LoginModel entity, IEnumerable<string> roles);
        Task<IEnumerable<string>> GetRolesAsync(UserModel entity);
        Task<UserModel?> CheckCredentialsAsync(LoginModel model);
        Task<bool> CheckUserAsync(IIdentity? identity);
    }
}
