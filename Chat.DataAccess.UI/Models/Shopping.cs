using Chat.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Chat.DataAccess.UI.Models
{
    public class Shopping
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }

        [Required]
        public string UserId { get; set; } = null!;

        public virtual Subscription? Subscription { get; set; }

        public decimal Price { get; set; }

        public Shopping()
        {

        }

        public Shopping(ShoppingModel model)
        {
            Id = model.Id;
            Date = model.Date;
            UserId = model.UserId;
            Price = model.Price;

            if(model.Subscription != null)
            {
                Subscription = new Subscription(model.Subscription);
            }
        }

        public static explicit operator ShoppingModel(Shopping entity)
        {
            var model = new ShoppingModel()
            {
                Id = entity.Id,
                Date = entity.Date,
                UserId = entity.UserId,
                Price = entity.Price
            };

            if(entity.Subscription != null)
            {
                model.Subscription = (SubscriptionModel)entity.Subscription;
                model.SubscriptionId = entity.Subscription.Id;
            }

            return model;
        }
    }
}
