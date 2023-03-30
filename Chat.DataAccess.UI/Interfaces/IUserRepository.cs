using Chat.DataAccess.UI.Models;
using Chat.Domain.Models;
using System.Security.Claims;
using System.Security.Principal;

namespace Chat.DataAccess.UI.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetAsync(string id);

        Task<User?> CreateAsync(LoginModel entity, IEnumerable<string> roles);
        Task<IEnumerable<string>> GetRolesAsync(UserModel entity);
        Task<UserModel?> CheckCredentialsAsync(LoginModel model);
        Task<bool> CheckUserAsync(IIdentity? identity);
        Task<User?> GetByUserNameAsync(string username);
        Task<bool> BanAsync(string id, Ban? ban);
        Task<List<Claim>> GetClaimsAsync(UserModel user);
        Task<User?> UpdateAsync(User entity);
    }
}
