namespace Chat.DataAccess.UI.Models
{
    public class Result<T>
    {
        public string? Token { get; set; }
        public T? Value { get; set; }
        public int Hits { get; set; }
    }
}
