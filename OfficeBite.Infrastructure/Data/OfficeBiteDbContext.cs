using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OfficeBite.Infrastructure.Data.Models;

namespace OfficeBite.Infrastructure.Data
{
    public class OfficeBiteDbContext : IdentityDbContext
    {
        public OfficeBiteDbContext(DbContextOptions<OfficeBiteDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<DishesInMenu>()
                .HasOne(d => d.MenuOrder)
                .WithMany(m => m.DishesInMenus)
                .HasForeignKey(d => d.RequestMenuNumber)
                .HasPrincipalKey(m => m.RequestMenuNumber)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(builder);
        }

        public DbSet<Order> Orders { get; set; }

        public DbSet<DishCategory> DishCategories { get; set; }

        public DbSet<UserAgent> UserAgents { get; set; }

        public DbSet<Dish> Dishes { get; set; }

        public DbSet<MenuType> MenuTypes { get; set; }

        public DbSet<DishesInMenu> DishesInMenus { get; set; }
        
        public DbSet<MenuOrder> MenuOrders { get; set; }

        public DbSet<OrderHistory> OrderHistories { get; set; }
        public DbSet<TwoFactorPushChallenge> TwoFactorPushChallenges { get; set; }


    }
}
