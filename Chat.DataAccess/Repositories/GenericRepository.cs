namespace Chat.DataAccess.Repositories
{
    public class GenericRepository
    {
        protected readonly ChatDbContext _db;

        public GenericRepository(ChatDbContext db)
        {
            _db = db;
        }
    }
}
