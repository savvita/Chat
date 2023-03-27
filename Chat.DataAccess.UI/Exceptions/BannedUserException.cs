namespace Chat.DataAccess.UI.Exceptions
{
    public class BannedUserException : Exception
    {
        public string UserId { get; }
        public BannedUserException(string userId) : base($"User {userId} is banned")
        {
            UserId = userId;
        }
    }
}
