using System.ComponentModel.DataAnnotations;

namespace Chat.Domain.Models
{
    public class AbilityModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = null!;
        public virtual ICollection<SubscriptionModel> Subscriptions { get; } = new List<SubscriptionModel>();
    }
}
