using System.ComponentModel.DataAnnotations;

namespace Chat.Domain.Models
{
    public class BanModel
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(20)]
        public string Description { get; set; } = null!;
        public int? Value { get; set; }
        [MaxLength(10)]
        public string? Units { get; set; }
    }
}
