namespace Chat.DataAccess.UI.Exceptions
{
    public class AuthorizationException : Exception
    {
        public AuthorizationException() : base("Authorization failed")
        {

        }
    }
}
