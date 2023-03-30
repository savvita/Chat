using Chat.Domain.Models;

namespace Chat.Domain.Interfaces
{
    public interface IRequestRepository : IGenericRepository<RequestModel>
    {
        Task<RequestModel?> GetAsync(int id);
        Task<List<RequestModel>> GetUserRequestsAsync(string userId);
        Task<RequestModel> UpdateAsync(RequestModel entity);
    }
}
