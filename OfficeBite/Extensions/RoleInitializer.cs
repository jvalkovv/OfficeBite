using Microsoft.AspNetCore.Identity;
using OfficeBite.Extensions.Interfaces;

namespace OfficeBite.Extensions
{
    public class RoleInitializer : IRoleInitializer
    {
        private readonly IServiceProvider _serviceProvider;

        public RoleInitializer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        //Creating a new roles ...
        public async Task InitializeRolesAndUsersAsync()
        {
            using var scope = _serviceProvider.CreateScope();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            if (!await roleManager.RoleExistsAsync("Manager"))
            {
                await roleManager.CreateAsync(new IdentityRole("Manager"));
            }

            if (!await roleManager.RoleExistsAsync("Staff"))
            {
                await roleManager.CreateAsync(new IdentityRole("Staff"));
            }
            if (!await roleManager.RoleExistsAsync("Employee"))
            {
                await roleManager.CreateAsync(new IdentityRole("Employee"));
            }

            // Assign user to role
            var adminUser = await userManager.FindByEmailAsync("yordan_valkov@abv.bg");

            if (adminUser != null && !await userManager.IsInRoleAsync(adminUser, "Admin"))
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}
