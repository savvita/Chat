using Chat.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Chat.DataAccess
{
    public class ChatDbContext : IdentityDbContext<UserModel>
    {
        public DbSet<AbilityModel> Abilities { get; set; }
        public DbSet<RequestModel> Requests { get; set; }
        public DbSet<ShoppingModel> Shoppings { get; set; }
        public DbSet<SubscriptionModel> Subscriptions{ get; set; }
        public DbSet<SubscriptionAbilityModel> SubscriptionAbility{ get; set; }
        public ChatDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AbilityModel>().HasData(
                new AbilityModel { Id = 1, Name = "Текст" },
                new AbilityModel { Id = 2, Name = "Зображення" },
                new AbilityModel { Id = 3, Name = "Голос" },
                new AbilityModel { Id = 4, Name = "Історія" }
            );
            modelBuilder.Entity<SubscriptionModel>().HasData(
                new SubscriptionModel { Id = 1, Name = "Безкоштовна", Price = 0 },
                new SubscriptionModel { Id = 2, Name = "Start", Price = 5 },
                new SubscriptionModel { Id = 3, Name = "Light", Price = 25 },
                new SubscriptionModel { Id = 3, Name = "Premium", Price = 50 }
            );
            modelBuilder.Entity<SubscriptionAbilityModel>().HasData(
                new SubscriptionAbilityModel { SubscriptionId = 1, AbilityId = 1, MaxCount = 5 },
                new SubscriptionAbilityModel { SubscriptionId = 1, AbilityId = 2, MaxCount = 5 },
                new SubscriptionAbilityModel { SubscriptionId = 2, AbilityId = 1, MaxCount = null },
                new SubscriptionAbilityModel { SubscriptionId = 2, AbilityId = 2, MaxCount = null },
                new SubscriptionAbilityModel { SubscriptionId = 3, AbilityId = 1, MaxCount = null },
                new SubscriptionAbilityModel { SubscriptionId = 3, AbilityId = 2, MaxCount = null },
                new SubscriptionAbilityModel { SubscriptionId = 3, AbilityId = 4, MaxCount = null },
                new SubscriptionAbilityModel { SubscriptionId = 4, AbilityId = 1, MaxCount = null },
                new SubscriptionAbilityModel { SubscriptionId = 4, AbilityId = 2, MaxCount = null },
                new SubscriptionAbilityModel { SubscriptionId = 4, AbilityId = 3, MaxCount = null },
                new SubscriptionAbilityModel { SubscriptionId = 4, AbilityId = 4, MaxCount = null }

            );
        }
    }
}
