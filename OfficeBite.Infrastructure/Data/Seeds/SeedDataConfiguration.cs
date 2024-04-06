using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OfficeBite.Infrastructure.Data.Models;

namespace OfficeBite.Infrastructure.Data.Seeds
{
    public static class SeedDataConfiguration
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {

            using var context = new OfficeBiteDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<OfficeBiteDbContext>>());


            if (context.DishCategories.Any() || context.Dishes.Any() || context.MenuTypes.Any())
            {
                return;
            }

            context.DishCategories.AddRange(
                new DishCategory { Name = "Салати" },
                new DishCategory { Name = "Предястия" },
                new DishCategory { Id = 3, Name = "Супи" },
                new DishCategory { Id = 4, Name = "Гарнитури" },
                new DishCategory { Id = 5, Name = "Основни" },
                new DishCategory { Id = 6, Name = "Десерти" },
                new DishCategory { Id = 7, Name = "Напитки" }
            );

            context.Dishes.AddRange(
                new Dish
                {
                    Id = 1,
                    DishName = "Салата Цезар с пиле",
                    CategoryId = 1,
                    Description = "Зелена салата, айсберг, крутони, пармезан, овкусени с класически „Цезар” дресинг с аншоа",
                    ImageUrl = "/img/salata_cezar.jpg",
                    Price = (decimal)15.95
                }, new Dish
                {
                    Id = 2,
                    DishName = "Салата Капрезе",
                    CategoryId = 1,
                    Description = "белен домат, моцарела и сос Песто",
                    ImageUrl = "/img/salata-kapreze.jpg",
                    Price = (decimal)15.75

                },
                new Dish
                {
                    Id = 3,
                    DishName = "Пилешки пръчици с хрупкава панировка",
                    CategoryId = 2,
                    Description = "Поднесени с млечен сос с копър",
                    ImageUrl = "/img/pecheni-pileshki-hapki.jpg",
                    Price = (decimal)15.75
                }, new Dish
                {
                    Id = 4,
                    DishName = "Скариди в хрупкава темпура",
                    CategoryId = 2,
                    Description = "Поднесени с пикантна майонеза",
                    ImageUrl = "/img/skaridi_tempura.jpg",
                    Price = (decimal)23.75
                }, new Dish
                {
                    Id = 5,
                    DishName = "Пилешка супа",
                    CategoryId = 3
                }, new Dish
                {
                    Id = 6,
                    DishName = "Буябес",
                    CategoryId = 3
                }, new Dish
                {
                    Id = 7,
                    DishName = "Домашни пържени картофи със сирене",
                    CategoryId = 4
                }, new Dish
                {
                    Id = 8,
                    DishName = "Черен ориз с тиква и чушки",
                    CategoryId = 4
                }, new Dish
                {
                    Id = 9,
                    DishName = "Стек от свински врат",
                    CategoryId = 5
                }, new Dish
                {
                    Id = 10,
                    DishName = "Бургер от Black Angus Beef",
                    CategoryId = 5
                }, new Dish
                {
                    Id = 11,
                    DishName = "Домашна бисквитена торта",
                    CategoryId = 6
                }, new Dish
                {
                    Id = 12,
                    DishName = "Тирамису",
                    CategoryId = 6
                }, new Dish
                {
                    Id = 13,
                    DishName = "Фреш",
                    CategoryId = 7
                }, new Dish
                {
                    Id = 14,
                    DishName = "Фрапе",
                    CategoryId = 7
                }
            );

            context.MenuTypes.AddRange(
                new MenuType
                {
                    Id = 1,
                    Name = "Класическо"
                }, new MenuType
                {
                    Id = 2,
                    Name = "Разширено"

                }, new MenuType
                {
                    Id = 3,
                    Name = "ВИП"
                }, new MenuType
                {
                    Id = 4,
                    Name = "Аламинут"
                }, new MenuType
                {
                    Id = 5,
                    Name = "По избор"
                });

            context.SaveChanges();
        }
    }
}

