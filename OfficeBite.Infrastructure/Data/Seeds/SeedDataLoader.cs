using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OfficeBite.Infrastructure.Data.Models;
using OfficeBite.Infrastructure.Data.Seeds.Interfaces;

namespace OfficeBite.Infrastructure.Data.Seeds
{
    public class SeedDataLoader : ISeedDataLoader
    {
        private readonly OfficeBiteDbContext dbContext;
        private readonly IServiceProvider serviceProvider;
        public SeedDataLoader(OfficeBiteDbContext _dbContext, IServiceProvider _serviceProvider)
        {
            this.dbContext = _dbContext;
            this.serviceProvider = _serviceProvider;
        }

        public void InitializeSeedData()
        {
            using (var dbContext = new OfficeBiteDbContext(
                       serviceProvider.GetRequiredService<DbContextOptions<OfficeBiteDbContext>>()))
            {

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
                    dbContext.SaveChanges();
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

                            Name = "По избор"
                        });
                    dbContext.SaveChanges();
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
                            Price = (decimal)15.95
                        }, new Dish
                        {
                            DishName = "Салата Капрезе",
                            CategoryId = 1,
                            Description = "белен домат, моцарела и сос Песто",
                            ImageUrl = "/img/salata-kapreze.jpg",
                            Price = (decimal)15.75

                        },
                        new Dish
                        {
                            DishName = "Пилешки пръчици с хрупкава панировка",
                            CategoryId = 2,
                            Description = "Поднесени с млечен сос с копър",
                            ImageUrl = "/img/pecheni-pileshki-hapki.jpg",
                            Price = (decimal)15.75
                        }, new Dish
                        {
                            DishName = "Скариди в хрупкава темпура",
                            CategoryId = 2,
                            Description = "Поднесени с пикантна майонеза",
                            ImageUrl = "/img/skaridi_tempura.jpg",
                            Price = (decimal)23.75
                        }, new Dish
                        {
                            DishName = "Пилешка супа",
                            CategoryId = 3,
                            Description = "Пилешко месо, кореноплодни зеленчуци, картофи, магданоз",
                            ImageUrl = "/img/Pileshka_supa.jpg",
                            Price = (decimal)7.75
                        }, new Dish
                        {
                            DishName = "Буябес",
                            CategoryId = 3,
                            Description = "Морска риба, скарида, октопод, миди и сезонни зеленчуци",
                            ImageUrl = "/img/ribena-chorba-buiabes.jpg",
                            Price = (decimal)22.75
                        }, new Dish
                        {
                            DishName = "Домашни пържени картофи със сирене",
                            CategoryId = 4,
                            Description = "Домашни пържени картофи със сирене",
                            ImageUrl = "/img/domashni_purjeni_kartofki_sus_sirene.jpg",
                            Price = (decimal)8.95
                        }, new Dish
                        {
                            DishName = "Черен ориз с тиква и чушки",
                            CategoryId = 4,
                            Description = "Черен императорски ориз с тиква и чушки",
                            ImageUrl = "/img/cheren-oriz-s-tikva-i-chushki.jpg",
                            Price = (decimal)7.95
                        }, new Dish
                        {
                            DishName = "Стек от свински врат",
                            CategoryId = 5,
                            Description = "Гарниран с печени картофки с билки, поднесен с BBQ сос",
                            ImageUrl = "/img/Grilovan-svinski-vrat.jpg",
                            Price = (decimal)22.95
                        }, new Dish
                        {
                            DishName = "Бургер от Black Angus Beef",
                            CategoryId = 5,
                            Description = "Хрупкав бекон, сирене чедър, айсберг, домат, яйце, червен лук, бургер сос, поднесен с пресни пържени картофки и кисели краставички",
                            ImageUrl = "/img/Black-Angus-burger.jpg",
                            Price = (decimal)25.95

                        }, new Dish
                        {
                            DishName = "Домашна бисквитена торта",
                            CategoryId = 6,
                            Description = "Домашна бисквитена торта",
                            ImageUrl = "/img/domashna_biskvitena_torta.jpg",
                            Price = (decimal)9.75
                        }, new Dish
                        {
                            DishName = "Тирамису",
                            CategoryId = 6,
                            Description = "Тирамису",
                            ImageUrl = "/img/tiramisu.jpg",
                            Price = (decimal)9.75
                        }, new Dish
                        {
                            DishName = "Фреш",
                            CategoryId = 7,
                            Description = "Фреш - Пъпеш",
                            ImageUrl = "/img/fresh.jpg",
                            Price = (decimal)6.95
                        }, new Dish
                        {
                            DishName = "Фрапе",
                            CategoryId = 7,
                            Description = "Фрапе черно - бяло",
                            ImageUrl = "/img/frape.jpg",
                            Price = (decimal)6.95
                        }
                    );
                    dbContext.SaveChanges();
                }
            }
        }
    }
}
