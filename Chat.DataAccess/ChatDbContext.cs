using Chat.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Chat.DataAccess
{
    public class ChatDbContext : IdentityDbContext<UserModel>
    {
        public DbSet<AbilityModel> Abilities { get; set; }
        public DbSet<BanModel> Bans { get; set; }
        public DbSet<RequestModel> Requests { get; set; }
        public DbSet<ShoppingModel> Shoppings { get; set; }
        public DbSet<SubscriptionModel> Subscriptions{ get; set; }
        public ChatDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var text = new AbilityModel { Id = 1, Name = "Текст" };
            var image = new AbilityModel { Id = 2, Name = "Зображення" };
            var voice = new AbilityModel { Id = 3, Name = "Голос" };
            var history = new AbilityModel { Id = 4, Name = "Історія" };

            modelBuilder.Entity<AbilityModel>().HasData(text, image, voice, history);

            var free = new SubscriptionModel { Id = 1, Name = "Безкоштовна", Price = 0, MaxCount = 5 };
            var start = new SubscriptionModel { Id = 2, Name = "Start", Price = 5, MaxCount = null };
            var light = new SubscriptionModel { Id = 3, Name = "Light", Price = 25, MaxCount = null };
            var premium = new SubscriptionModel { Id = 4, Name = "Premium", Price = 50, MaxCount = null };
           
            modelBuilder.Entity<SubscriptionModel>().HasData(free, start, light, premium);
            modelBuilder.Entity<BanModel>().HasData(
                new BanModel { Id = 1, Description = "1 день", Value = 1, Units = "day" },  
                new BanModel { Id = 2, Description = "1 тиждень", Value = 1, Units = "week" },  
                new BanModel { Id = 3, Description = "3 місяці", Value = 3, Units = "month" },  
                new BanModel { Id = 4, Description = "1 рік", Value = 1, Units = "year" },  
                new BanModel { Id = 5, Description = "Назавжди", Value = null, Units = null }
            );

        }
    }
}
