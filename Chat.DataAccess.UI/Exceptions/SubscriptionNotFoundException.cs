namespace Chat.DataAccess.UI.Exceptions
{
    public class SubscriptionNotFoundException : Exception
    {
        public SubscriptionNotFoundException() : base("Subscription not found")
        {

        }
    }
}
