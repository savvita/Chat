using System.Text.Json.Serialization;

namespace Chat.S3.Models
{
    public class TranscriptResult
    {
        [JsonPropertyName("transcript")]
        public string? Transcript { get; set; }
    }
}
