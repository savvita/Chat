namespace Chat.Domain.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> CreateAsync(T entity);
        Task<IEnumerable<T>> GetAsync();
    }
}
