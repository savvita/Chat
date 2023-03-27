using Chat.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Chat.DataAccess.UI.Models
{
    public class Subscription
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = null!;

        [Required]
        public decimal Price { get; set; }

        public List<SubscriptionAbility> SubscriptionAbilities { get; } = new List<SubscriptionAbility>();

        public Subscription()
        {

        }

        public Subscription(SubscriptionModel model)
        {
            Id = model.Id;
            Name = model.Name;
            Price = model.Price;
            model.SubscriptionAbilities.ToList().ForEach(x =>
            {
                SubscriptionAbilities.Add(new SubscriptionAbility(x));
            });
        }

        public static explicit operator SubscriptionModel(Subscription entity)
        {
            var model = new SubscriptionModel()
            {
                Id = entity.Id,
                Name = entity.Name,
                Price = entity.Price
            };

            entity.SubscriptionAbilities.ForEach(x =>
            {
                model.SubscriptionAbilities.Add((SubscriptionAbilityModel)x);
            });

            return model;
        }
    }
}
