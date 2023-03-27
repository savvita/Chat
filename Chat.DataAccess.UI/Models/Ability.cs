using Chat.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Chat.DataAccess.UI.Models
{
    public class Ability
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = null!;

        public List<SubscriptionAbility> SubscriptionAbilities { get; } = new List<SubscriptionAbility>();
        public Ability()
        {
        }

        public Ability(AbilityModel model)
        {
            Id = model.Id;
            Name = model.Name;
            model.SubscriptionAbilities.ToList().ForEach(x =>
            {
                SubscriptionAbilities.Add(new SubscriptionAbility(x));
            });
        }

        public static explicit operator AbilityModel(Ability entity)
        {
            var model = new AbilityModel()
            {
                Id = entity.Id,
                Name = entity.Name
            };

            entity.SubscriptionAbilities.ForEach(x =>
            {
                model.SubscriptionAbilities.Add((SubscriptionAbilityModel)x);
            });

            return model;
        }
    }
}
