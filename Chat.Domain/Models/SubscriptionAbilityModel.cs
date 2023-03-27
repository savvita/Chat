using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chat.Domain.Models
{
    public class SubscriptionAbilityModel
    {
        [Key, Column(Order = 0)]
        public int SubscriptionId { get; set; }

        public virtual SubscriptionModel? Subscription { get; set; }

        [Key, Column(Order = 1)]
        public int AbilityId { get; set; }

        public virtual AbilityModel? Ability { get; set; }

        public int? MaxCount { get; set; } = null;
    }
}
