namespace Chat.DataAccess.UI.Exceptions
{
    public class MaxRequestCountException : Exception
    {
        public int MaxCount { get; }
        public MaxRequestCountException(int maxCount) : base("Max request count reached")
        {
            MaxCount = maxCount;
        }
    }
}
