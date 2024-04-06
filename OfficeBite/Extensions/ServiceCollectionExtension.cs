using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OfficeBite.Extensions.Interfaces;
using OfficeBite.Infrastructure.Data;
using OfficeBite.Infrastructure.Data.Seeds;
using OfficeBite.Infrastructure.Data.Seeds.Interfaces;

namespace OfficeBite.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddOfficeBiteServices(this IServiceCollection services)
        {
            return services;
        }

        //Connection to DB
        public static IServiceCollection AddOfficeBiteDbContext(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("DefaultConnection");

            services.AddDbContext<OfficeBiteDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddDatabaseDeveloperPageExceptionFilter();

            return services;
        }

        public static IServiceCollection AddSeedDataLoader(this IServiceCollection services)
        {
            services.AddScoped<ISeedDataLoader, SeedDataLoader>();

            return services;
        }
        //Identity Application Password Validation
        public static IServiceCollection AddApplicationIdentity(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IRoleInitializer, RoleInitializer>();

            services.AddScoped<IUserRoleStore<IdentityUser>, UserStore<IdentityUser, IdentityRole, OfficeBiteDbContext, string>>();
            services.AddScoped<IRoleStore<IdentityRole>, RoleStore<IdentityRole, OfficeBiteDbContext, string>>();

            services.AddDefaultIdentity<IdentityUser>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireNonAlphanumeric = true;
                })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<OfficeBiteDbContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<RoleManager<IdentityRole>>();

            return services;
        }

        public static IServiceCollection ConfigureCookie(this IServiceCollection services,
            IConfiguration config)
        {
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "OfficeBiteCookies";
                options.Cookie.SameSite = SameSiteMode.Strict;
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromDays(1);
                options.LoginPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
            });

            return services;
        }
    }
}
