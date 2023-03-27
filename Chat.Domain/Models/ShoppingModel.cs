using System.ComponentModel.DataAnnotations;

namespace Chat.Domain.Models
{
    public class ShoppingModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }

        [Required]
        [MaxLength(100)]
        public string UserId { get; set; } = null!;
        public virtual UserModel? User { get; set; }

        public int SubscriptionId { get; set; }
        public virtual SubscriptionModel? Subscription { get; set; }

        public decimal Price { get; set; }
    }
}
