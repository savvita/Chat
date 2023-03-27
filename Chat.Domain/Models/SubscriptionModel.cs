using System.ComponentModel.DataAnnotations;

namespace Chat.Domain.Models
{
    public class SubscriptionModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = null!;

        [Required]
        public decimal Price { get; set; }

        public virtual ICollection<SubscriptionAbilityModel> SubscriptionAbilities { get; } = new List<SubscriptionAbilityModel>();
    }
}
