using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
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
            var options = new DbContextOptionsBuilder<OfficeBiteDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _dbContext = new OfficeBiteDbContext(options);


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


            _helperMethods = new HelperMethods(_dbContext);
        }

        [TearDown]
        public void TearDown()
        {

            _dbContext.Dispose();
        }

        [Test]
        public async Task GetDishForMenuAsync_ReturnsCorrectDishViewModels()
        {

            var result = await _helperMethods.GetDishForMenuAsync();


            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(2));

            var dishViewModel1 = result.FirstOrDefault(d => d.DishId == 1);
            ClassicAssert.IsNotNull(dishViewModel1);
            Assert.That(dishViewModel1.DishName, Is.EqualTo("Dish 1"));
            Assert.That(dishViewModel1.DishPrice, Is.EqualTo(10));
            Assert.That(dishViewModel1.CategoryId, Is.EqualTo(1));

            var dishViewModel2 = result.FirstOrDefault(d => d.DishId == 2);
            ClassicAssert.IsNotNull(dishViewModel2);
            Assert.That(dishViewModel2.DishName, Is.EqualTo("Dish 2"));
            Assert.That(dishViewModel2.DishPrice, Is.EqualTo(15));
            Assert.That(dishViewModel2.CategoryId, Is.EqualTo(2));
        }
    }
}
