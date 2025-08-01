using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OfficeBite.Core.Services;
using OfficeBite.Core.Services.Contracts;
using OfficeBite.Infrastructure.Data;
using OfficeBite.Infrastructure.Data.Common;
using OfficeBite.Infrastructure.Data.Seeds;
using OfficeBite.Infrastructure.Data.Seeds.Interfaces;
using OfficeBite.Infrastructure.Extensions;
using OfficeBite.Infrastructure.Extensions.Interfaces;

namespace OfficeBite.Core.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddOfficeBiteServices(this IServiceCollection services)
        {
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IStaffService, StaffService>();
            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<IDishService, DishService>();
            services.AddScoped<IOrderService, OrderService>();

            return services;
        }

        //Connection to DB
        public static IServiceCollection AddOfficeBiteDbContext(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("DefaultConnection");

            services.AddDbContext<OfficeBiteDbContext>(options =>
            {
                options.UseSqlServer(connectionString,
                    options =>
                    {
                        options.EnableRetryOnFailure(maxRetryCount: 1, maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorNumbersToAdd: null);
                    });
            });

            services.AddScoped<IRepository, Repository>();

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
                options.ExpireTimeSpan = TimeSpan.FromMinutes(3);
                options.LoginPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
            });

            return services;
        }

        public static IServiceCollection AddApplicationExternalFbIdentity(this IServiceCollection services, IConfiguration config)
        {
            services.AddAuthentication()
                .AddFacebook(options =>
                {
                    IConfigurationSection fbAuthNSection =
                        config.GetSection("Authentication:FB");
                    options.AppId = config["Authentication:Facebook:AppId"];
                    options.AppSecret = config["Authentication:Facebook:AppSecret"];
                });

            return services;

        }

        //public static IServiceCollection ConfigureCustomRoutes(this IServiceCollection services)
        //{
        //    services.AddRouting(options =>
        //    {
        //        options.LowercaseUrls = true; // Optional: Ensure lowercase URLs
        //    });

        //    services.Configure<RouteOptions>(options =>
        //    {
        //        options.AppendTrailingSlash = false; // Optional: Remove trailing slashes
        //        options.LowercaseQueryStrings = true; // Optional: Ensure lowercase query strings
        //    });

        //    return services;
        //}

    }
}
