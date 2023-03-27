using Chat.Domain.Models;

namespace Chat.DataAccess.UI.Models
{
    public class SubscriptionAbility
    {
        public virtual Subscription? Subscription { get; set; }

        public virtual Ability? Ability { get; set; }

        public int? MaxCount { get; set; } = null;

        public SubscriptionAbility()
        {

        }

        public SubscriptionAbility(SubscriptionAbilityModel model)
        {
            MaxCount = model.MaxCount;

            if (model.Subscription != null)
            {
                Subscription = new Subscription(model.Subscription);
            }

            if (model.Ability != null)
            {
                Ability = new Ability(model.Ability);
            }
        }

        public static explicit operator SubscriptionAbilityModel(SubscriptionAbility entity)
        {
            var model = new SubscriptionAbilityModel()
            {
                MaxCount = entity.MaxCount
            };

            if(entity.Ability != null)
            {
                model.Ability = (AbilityModel)entity.Ability;
                model.AbilityId = entity.Ability.Id;
            }

            if (entity.Subscription != null)
            {
                model.Subscription = (SubscriptionModel)entity.Subscription;
                model.SubscriptionId = entity.Subscription.Id;
            }

            return model;
        }
    }
}
