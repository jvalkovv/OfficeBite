using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework.Legacy;
using OfficeBite.Core.Extensions.Interfaces;
using OfficeBite.Core.Models.CategoryModels;
using OfficeBite.Core.Models.DishModels;
using OfficeBite.Core.Services;
using OfficeBite.Core.Services.Contracts;
using OfficeBite.Infrastructure.Data;
using OfficeBite.Infrastructure.Data.Common;
using OfficeBite.Infrastructure.Data.Models;

namespace OfficeBiteTests.DishControllerTests
{
    [TestFixture]
    public class DishControllerTests
    {
        private IDishService dishService;
        private Mock<IRepository> repositoryMock;
        private Mock<IHelperMethods> helperMethodsMock;
        private List<Dish> testDishes;
        private List<MenuOrder> testMenuOrder;
        private List<DishesInMenu> testDishesInMenu;
        private OfficeBiteDbContext dbContext;

        [SetUp]
        public void Setup()
        {
            repositoryMock = new Mock<IRepository>();
            helperMethodsMock = new Mock<IHelperMethods>();

            var options = new DbContextOptionsBuilder<OfficeBiteDbContext>()
                .UseInMemoryDatabase(databaseName: "Test_Dishes_Database")
                .Options;

            dbContext = new OfficeBiteDbContext(options);

            repositoryMock.Setup(repo => repo.All<Dish>())
                .Returns(dbContext.Dishes);
            repositoryMock.Setup(repo => repo.All<MenuOrder>())
                .Returns(dbContext.MenuOrders);
            repositoryMock.Setup(repo => repo.All<DishesInMenu>())
                .Returns(dbContext.DishesInMenus);

            testDishes = new List<Dish>
            {
                new Dish
                    { Id = 1, IsVisible = true, DishName = "Test Dish", Price = 2, Description = "Test Description", ImageUrl = "test.jpg" }
            };

            dbContext.Dishes.AddRange(testDishes);
            dbContext.SaveChanges();

            testMenuOrder = new List<MenuOrder>
            {
                new MenuOrder { Id = 1, IsVisible = true, MenuTypeId = 1 }
            };
            dbContext.MenuOrders.AddRange(testMenuOrder);
            dbContext.SaveChanges();

            testDishesInMenu = new List<DishesInMenu>
            {
                new DishesInMenu { Id = 1, IsVisible = true }
            };
            dbContext.DishesInMenus.AddRange(testDishesInMenu);
            dbContext.SaveChanges();

            dishService = new DishService(repositoryMock.Object, helperMethodsMock.Object);
        }
        [TearDown]
        public void TearDown()
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();
        }


        [Test]
        public async Task GetAllDishes_ShouldReturnAllDishes()
        {
            var categories = new List<CategoryViewModel>();
            var dishes = new List<DishViewModel>();
            helperMethodsMock.Setup(m => m.GetCategoryAsync()).ReturnsAsync(categories);

            helperMethodsMock.Setup(m => m.GetDishesAsync()).ReturnsAsync(dishes);


            var result = await dishService.GetAllDishes();

            ClassicAssert.IsNotNull(result);
            ClassicAssert.AreSame(categories, result.Categories);
            ClassicAssert.AreSame(dishes, result.Dishes);
        }

        [Test]
        public async Task GetAllHiddenDishes_ShouldReturnHiddenDishes()
        {
            var dishes = new List<DishViewModel> { new DishViewModel { DishId = 1, IsVisible = false } };
            helperMethodsMock.Setup(m => m.GetDishesAsync()).ReturnsAsync(dishes);

            var dishEntities = new List<Dish> { new Dish { Id = 2, IsVisible = false } };
            repositoryMock.Setup(repo => repo.All<Dish>()).Returns(dishEntities.AsQueryable());

            var categories = new List<CategoryViewModel>
            {
                new CategoryViewModel { Id = 1, Name = "Category" }
            };
            helperMethodsMock.Setup(m => m.GetCategoryAsync()).ReturnsAsync(categories);

            var result = await dishService.GetAllHiddenDishes();

            ClassicAssert.IsNotNull(result);
            ClassicAssert.AreEqual(1, result.Dishes.Count());
            ClassicAssert.IsFalse(result.Dishes.First().IsVisible);
        }

        [Test]
        public async Task HideDish_ShouldReturnDishViewModel()
        {
            var dishId = 1;
            var dish = new Dish
            { Id = dishId, IsVisible = true, DishName = "Test Dish", Price = 2, Description = "Test Description", ImageUrl = "test.jpg" };
            var dishViewModel = new DishViewModel
            {
                DishId = dish.Id,
                IsVisible = dish.IsVisible,
                DishName = dish.DishName,
                DishPrice = dish.Price,
                Description = dish.Description,
                ImageUrl = dish.ImageUrl
            };

            repositoryMock.Setup(repo => repo.GetByIdAsync<Dish>(dishId)).ReturnsAsync(dish);


            var result = await dishService.HideDish(dishId);

            ClassicAssert.IsNotNull(result);
            ClassicAssert.AreEqual(dishViewModel.DishId, result.DishId);
            ClassicAssert.AreEqual(dishViewModel.IsVisible, result.IsVisible);
            ClassicAssert.AreEqual(dishViewModel.DishName, result.DishName);
            ClassicAssert.AreEqual(dishViewModel.DishPrice, result.DishPrice);
            ClassicAssert.AreEqual(dishViewModel.Description, result.Description);
            ClassicAssert.AreEqual(dishViewModel.ImageUrl, result.ImageUrl);
        }



        [Test]
        public async Task UnHideDish_ShouldReturnDishViewModel()
        {
            var dishId = 1;
            var dish = new Dish { Id = dishId, IsVisible = true, DishName = "Test Dish", Price = 2, Description = "Test Description", ImageUrl = "test.jpg" };
            var dishViewModel = new DishViewModel
            {
                DishId = dish.Id,
                IsVisible = dish.IsVisible,
                DishName = dish.DishName,
                DishPrice = dish.Price,
                Description = dish.Description,
                ImageUrl = dish.ImageUrl
            };


            repositoryMock.Setup(repo => repo.GetByIdAsync<Dish>(dishId)).ReturnsAsync(dish);

            var result = await dishService.UnHideDish(dishId);

            ClassicAssert.IsNotNull(result);
            ClassicAssert.AreEqual(dishViewModel.DishId, result.DishId);
            ClassicAssert.AreEqual(dishViewModel.IsVisible, result.IsVisible);
            ClassicAssert.AreEqual(dishViewModel.DishName, result.DishName);
            ClassicAssert.AreEqual(dishViewModel.DishPrice, result.DishPrice);
            ClassicAssert.AreEqual(dishViewModel.Description, result.Description);
            ClassicAssert.AreEqual(dishViewModel.ImageUrl, result.ImageUrl);
        }

        [Test]
        public async Task EditDish_ShouldReturnAllDishesViewModel()
        {

            var dishId = 1;
            var dish = new Dish { Id = dishId, DishName = "Test Dish", Price = 10, Description = "Test Description", CategoryId = 1 };
            var category = new CategoryViewModel { Id = dish.CategoryId, Name = "Test Category" };

            repositoryMock.Setup(repo => repo.GetByIdAsync<Dish>(dishId)).ReturnsAsync(dish);
            helperMethodsMock.Setup(helper => helper.GetCategoryAsync()).ReturnsAsync(new List<CategoryViewModel> { category });

            // Act
            var result = await dishService.EditDish(dishId);

            // Assert
            ClassicAssert.IsNotNull(result);
            ClassicAssert.AreEqual(dish.Id, result.DishId);
            ClassicAssert.AreEqual(dish.DishName, result.DishName);
            ClassicAssert.AreEqual(dish.Price, result.DishPrice);
            ClassicAssert.AreEqual(dish.Description, result.Description);
            ClassicAssert.AreEqual(dish.CategoryId, result.CategoryId);
            ClassicAssert.IsNotNull(result.Categories);
        }



        [Test]
        public async Task AddDish_ShouldReturnAllDishesViewModel()
        {
            // Arrange
            var categories = new List<CategoryViewModel>
                { new CategoryViewModel() { Id = 1, Name = "Category 1" },
                    new CategoryViewModel { Id = 2, Name = "Category 2" } };

            var dishes = new List<DishViewModel>
                { new DishViewModel { DishId = 1, DishName = "Dish 1" },
                    new DishViewModel { DishId = 2, DishName = "Dish 2" } };

            helperMethodsMock.Setup(helper => helper.GetCategoryAsync()).ReturnsAsync(categories);
            helperMethodsMock.Setup(helper => helper.GetDishesAsync()).ReturnsAsync(dishes);

            // Act
            var result = await dishService.AddDish();

            // Assert
            ClassicAssert.IsNotNull(result);
            ClassicAssert.IsNotNull(result.Categories);
            ClassicAssert.IsNotNull(result.Dishes);
            ClassicAssert.AreEqual(categories.Count, result.Categories.Count());
            ClassicAssert.AreEqual(dishes.Count, result.Dishes.Count());
        }

        [Test]
        public async Task DeleteDishConfirm_ShouldDeleteDish()
        {
            var dishId = 1;
            var dish = new Dish { Id = dishId };

            repositoryMock.Setup(repo => repo.GetByIdAsync<Dish>(dishId)).ReturnsAsync(dish);

            await dishService.DeleteDishConfirm(dishId);

            repositoryMock.Verify(repo => repo.DeleteAsync<Dish>(dishId), Times.Once);
        }
    }
}
