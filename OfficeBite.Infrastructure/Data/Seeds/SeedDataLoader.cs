using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OfficeBite.Infrastructure.Data.Models;
using OfficeBite.Infrastructure.Data.Seeds.Interfaces;
using OfficeBite.Infrastructure.Extensions.Interfaces;

namespace OfficeBite.Infrastructure.Data.Seeds
{
    public class SeedDataLoader : ISeedDataLoader
    {
        private readonly OfficeBiteDbContext dbContext;
        private readonly IServiceProvider serviceProvider;
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IRoleInitializer roleInitializer;
        public SeedDataLoader(OfficeBiteDbContext _dbContext, IServiceProvider _serviceProvider, UserManager<IdentityUser> _userManager,
            RoleManager<IdentityRole> _roleManager, IRoleInitializer _roleInitializer)
        {
            this.dbContext = _dbContext;
            this.serviceProvider = _serviceProvider;
            this.userManager = _userManager;
            this.roleManager = _roleManager;
            this.roleInitializer = _roleInitializer;
        }

        public async Task InitializeSeedData()
        {

            await roleInitializer.InitializeRolesAndUsersAsync();

            await SeedData();
        }

        private async Task SeedData()
        {
            using (var dbContext = new OfficeBiteDbContext(
                       serviceProvider.GetRequiredService<DbContextOptions<OfficeBiteDbContext>>()))
            {
                if (!userManager.Users.Any())
                {
                    var usersData =
                        new List<(string Email, string Password, string FirstName, string LastName, string Role)>
                        {
                            ("admin@example.com", "Admin@123", "Admin", "AdminUser", "Admin"),
                            ("manager@example.com", "Manager@123", "Manager", "ManagerUser", "Manager"),
                            ("staff@example.com", "Staff@123", "Staff", "StaffUser", "Staff"),
                            ("employee@example.com", "Employee@123", "Employee", "EmployeeUser", "Employee"),
                            ("user@example.com", "User@123", "User", "User", "User")
                        };

                    foreach (var userData in usersData)
                    {
                        // Generate a username based on the email address
                        var userName = GenerateUserNameFromEmail(userData.Email);

                        var user = new IdentityUser
                        {
                            UserName = userName,
                            Email = userData.Email
                        };

                        var result = await userManager.CreateAsync(user, userData.Password);

                        if (result.Succeeded)
                        {
                            await userManager.AddToRoleAsync(user, userData.Role);

                            var userAgent = new UserAgent
                            {
                                UserId = user.Id,
                                FirstName = userData.FirstName,
                                LastName = userData.LastName,
                                Username = userName
                            };

                            dbContext.UserAgents.Add(userAgent);
                        }
                        else
                        {
                            // Handle creation failure if needed
                        }
                    }

                    await dbContext.SaveChangesAsync();
                }
                if (!dbContext.MenuTypes.Any())
                {
                    dbContext.MenuTypes.AddRange(
                        new MenuType
                        {

                            Name = "Класическо"
                        }, new MenuType
                        {

                            Name = "Разширено"

                        }, new MenuType
                        {

                            Name = "ВИП"
                        }, new MenuType
                        {

                            Name = "Аламинут"
                        }, new MenuType
                        {

                            Name = "Вегетарианско"
                        }, new MenuType
                        {

                            Name = "По избор"
                        });
                    await dbContext.SaveChangesAsync();
                }
                if (!dbContext.DishCategories.Any())
                {
                    dbContext.DishCategories.AddRange(
                        new DishCategory { Name = "Салати" },
                        new DishCategory { Name = "Предястия" },
                        new DishCategory { Name = "Супи" },
                        new DishCategory { Name = "Гарнитури" },
                        new DishCategory { Name = "Основни" },
                        new DishCategory { Name = "Десерти" },
                        new DishCategory { Name = "Напитки" }
                    );
                    await dbContext.SaveChangesAsync();
                }
                if (!dbContext.Dishes.Any())
                {
                    dbContext.Dishes.AddRange(
                        new Dish
                        {
                            DishName = "Салата Цезар с пиле",
                            CategoryId = 1,
                            Description =
                                "Зелена салата, айсберг, крутони, пармезан, овкусени с класически „Цезар” дресинг с аншоа",
                            ImageUrl = "/img/salata_cezar.jpg",
                            Price = (decimal)2.25
                        }, new Dish
                        {
                            DishName = "Салата Капрезе",
                            CategoryId = 1,
                            Description = "белен домат, моцарела и сос Песто",
                            ImageUrl = "/img/salata-kapreze.jpg",
                            Price = (decimal)2.00

                        },
                        new Dish
                        {
                            DishName = "Пилешки пръчици с хрупкава панировка",
                            CategoryId = 2,
                            Description = "Поднесени с млечен сос с копър",
                            ImageUrl = "/img/pecheni-pileshki-hapki.jpg",
                            Price = (decimal)2.75
                        }, new Dish
                        {
                            DishName = "Пилешка супа",
                            CategoryId = 3,
                            Description = "Пилешко месо, кореноплодни зеленчуци, картофи, магданоз",
                            ImageUrl = "/img/Pileshka_supa.jpg",
                            Price = (decimal)2.25
                        }, new Dish
                        {
                            DishName = "Домашни пържени картофи със сирене",
                            CategoryId = 4,
                            Description = "Домашни пържени картофи със сирене",
                            ImageUrl = "/img/domashni_purjeni_kartofki_sus_sirene.jpg",
                            Price = (decimal)1.75
                        }, new Dish
                        {
                            DishName = "Черен ориз с тиква и чушки",
                            CategoryId = 4,
                            Description = "Черен императорски ориз с тиква и чушки",
                            ImageUrl = "/img/cheren-oriz-s-tikva-i-chushki.jpg",
                            Price = (decimal)2.50
                        }, new Dish
                        {
                            DishName = "Стек от свински врат",
                            CategoryId = 5,
                            Description = "Гарниран с печени картофки с билки, поднесен с BBQ сос",
                            ImageUrl = "/img/Grilovan-svinski-vrat.jpg",
                            Price = (decimal)2.75
                        }, new Dish
                        {
                            DishName = "Бургер от Black Angus Beef",
                            CategoryId = 5,
                            Description =
                                "Хрупкав бекон, сирене чедър, айсберг, домат, яйце, червен лук, бургер сос, поднесен с пресни пържени картофки и кисели краставички",
                            ImageUrl = "/img/Black-Angus-burger.jpg",
                            Price = (decimal)3.25

                        }, new Dish
                        {
                            DishName = "Домашна бисквитена торта",
                            CategoryId = 6,
                            Description = "Домашна бисквитена торта",
                            ImageUrl = "/img/domashna_biskvitena_torta.jpg",
                            Price = (decimal)1.25
                        }, new Dish
                        {
                            DishName = "Тирамису",
                            CategoryId = 6,
                            Description = "Тирамису",
                            ImageUrl = "/img/tiramisu.jpg",
                            Price = (decimal)1.50
                        }, new Dish
                        {
                            DishName = "Фреш",
                            CategoryId = 7,
                            Description = "Фреш - Пъпеш",
                            ImageUrl = "/img/fresh.jpg",
                            Price = (decimal)2.50
                        }, new Dish
                        {
                            DishName = "Фрапе",
                            CategoryId = 7,
                            Description = "Фрапе черно - бяло",
                            ImageUrl = "/img/frape.jpg",
                            Price = (decimal)2.50
                        }
                    );
                    await dbContext.SaveChangesAsync();
                }
            }
        }

        private string GenerateUserNameFromEmail(string email)
        {
            var parts = email.Split('@');
            return parts[0];
        }
    }
}

