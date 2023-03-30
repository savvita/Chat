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

        public int? MaxCount { get; set; }

        public List<Ability> Abilities { get; } = new List<Ability>();

        public Subscription()
        {

        }

        public Subscription(SubscriptionModel model)
        {
            Id = model.Id;
            Name = model.Name;
            Price = model.Price;
            MaxCount = model.MaxCount;
            model.Abilities.ToList().ForEach(x =>
            {
                Abilities.Add(new Ability(x));
            });
        }

        public static explicit operator SubscriptionModel(Subscription entity)
        {
            var model = new SubscriptionModel()
            {
                Id = entity.Id,
                Name = entity.Name,
                Price = entity.Price,
                MaxCount = entity.MaxCount
            };

            entity.Abilities.ForEach(x =>
            {
                model.Abilities.Add((AbilityModel)x);
            });

            return model;
        }
    }
}
