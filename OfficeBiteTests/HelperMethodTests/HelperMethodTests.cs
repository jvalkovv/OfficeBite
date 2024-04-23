using Microsoft.EntityFrameworkCore;
using NUnit.Framework.Legacy;
using OfficeBite.Core.Extensions;
using OfficeBite.Infrastructure.Data;
using OfficeBite.Infrastructure.Data.Models;

namespace OfficeBiteTests.HelperMethodTests
{
    [TestFixture]
    public class HelperMethodsTests
    {
        private OfficeBiteDbContext _dbContext;
        private HelperMethods _helperMethods;

        [SetUp]
        public void Setup()
        {
            // Инициализация на контекста на базата данни в паметта
            var options = new DbContextOptionsBuilder<OfficeBiteDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _dbContext = new OfficeBiteDbContext(options);

            // Попълване на базата данни с фалшиви данни за тестовете
            _dbContext.DishCategories.AddRange(new List<DishCategory>
            {
                new DishCategory { Id = 1, Name = "Category 1" },
                new DishCategory { Id = 2, Name = "Category 2" }
            });

            _dbContext.Dishes.AddRange(new List<Dish>
            {
                new Dish { Id = 1, DishName = "Dish 1", Price = 10, CategoryId = 1 },
                new Dish { Id = 2, DishName = "Dish 2", Price = 15, CategoryId = 2 }
            });

            _dbContext.SaveChanges();

            // Инициализация на HelperMethods с мокнат контекст на базата данни
            _helperMethods = new HelperMethods(_dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            // Почистване на ресурсите
            _dbContext.Dispose();
        }

        [Test]
        public async Task GetDishForMenuAsync_ReturnsCorrectDishViewModels()
        {
            // Act
            var result = await _helperMethods.GetDishForMenuAsync();

            // ClassicAssert
            ClassicAssert.IsNotNull(result);
            ClassicAssert.AreEqual(2, result.Count());

            // Проверка на правилността на върнатите модели за ястия
            var dishViewModel1 = result.FirstOrDefault(d => d.DishId == 1);
            ClassicAssert.IsNotNull(dishViewModel1);
            ClassicAssert.AreEqual("Dish 1", dishViewModel1.DishName);
            ClassicAssert.AreEqual(10, dishViewModel1.DishPrice);
            ClassicAssert.AreEqual(1, dishViewModel1.CategoryId);

            var dishViewModel2 = result.FirstOrDefault(d => d.DishId == 2);
            ClassicAssert.IsNotNull(dishViewModel2);
            ClassicAssert.AreEqual("Dish 2", dishViewModel2.DishName);
            ClassicAssert.AreEqual(15, dishViewModel2.DishPrice);
            ClassicAssert.AreEqual(2, dishViewModel2.CategoryId);
        }
    }
}
