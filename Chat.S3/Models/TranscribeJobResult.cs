using System.Text.Json.Serialization;

namespace Chat.S3.Models
{
    public class TranscribeJobResult
    {
        [JsonPropertyName("results")]
        public Results? Results { get; set; }  
    }
}
