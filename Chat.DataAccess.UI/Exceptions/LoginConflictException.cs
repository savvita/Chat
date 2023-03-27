namespace Chat.DataAccess.UI.Exceptions
{
    public class LoginConflictException : Exception
    {
        public LoginConflictException() : base("Login is already registered")
        {

        }
    }
}
