using System.ComponentModel.DataAnnotations;

namespace Chat.Domain.Models
{
    public class RequestModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }

        [Required]
        public string UserId { get; set; } = null!;
        public virtual UserModel? User { get; set; }

        [Required]
        [MaxLength(500)]
        public string RequestMessage { get; set; } = null!;

        public string? ResponseMessage { get; set; } = null;
    }
}
