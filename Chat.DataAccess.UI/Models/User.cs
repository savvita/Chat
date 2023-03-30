using Chat.Domain.Models;

namespace Chat.DataAccess.UI.Models
{
    public class User
    {
        public string Id { get; set; } = null!;

        public string? UserName { get; set; }

        public bool IsAdmin { get; set; }
        public DateTime? BannedUntil { get; set; } = null;
        public int Requests { get; }
        public virtual Subscription? Subscription { get; set; }

        public User()
        {
        }

        public User(UserModel model)
        {
            Id = model.Id;
            UserName = model.UserName;
            BannedUntil = model.BannedUntil;
            Requests = model.Requests;

            if(model.Subscription != null)
            {
                Subscription = new Subscription(model.Subscription);
            }
        }

        public static explicit operator UserModel(User entity)
        {
            var model = new UserModel()
            {
                Id = entity.Id,
                UserName = entity.UserName,
                BannedUntil = entity.BannedUntil,
                Requests = entity.Requests
            };

            if (entity.Subscription != null)
            {
                model.Subscription = (SubscriptionModel)entity.Subscription;
                model.SubscriptionId = entity.Subscription.Id;
            }

            return model;
        }
    }
}
