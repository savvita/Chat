using System.Text.Json.Serialization;

namespace Chat.S3.Models
{
    public class Results
    {
        [JsonPropertyName("transcripts")]
        public List<TranscriptResult> Transcripts { get; set; } = new();
    }
}
