using Microsoft.AspNetCore.Identity;

namespace Chat.Domain.Models
{
    public class UserModel : IdentityUser
    {

        public DateTime? BannedUntil { get; set; } = null;
        public int SubscriptionId { get; set; }
        public DateTime? LastRequestDate { get; set; }
        public int Requests { get; set; }
        public virtual SubscriptionModel? Subscription { get; set; }
        public virtual ICollection<ShoppingModel> ShoppingHistory { get; } = new List<ShoppingModel>();
        public virtual ICollection<RequestModel> RequestHistory { get; } = new List<RequestModel>();
    }
}
