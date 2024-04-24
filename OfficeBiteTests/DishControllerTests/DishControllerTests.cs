using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework.Legacy;
using OfficeBite.Controllers;
using OfficeBite.Core.Models.CategoryModels;
using OfficeBite.Core.Models.DishModels;
using OfficeBite.Extensions.Interfaces;
using OfficeBite.Infrastructure.Data;
using OfficeBite.Infrastructure.Data.Models;
using System.Security.Claims;

namespace OfficeBiteTests.DishControllerTests
{
    [TestFixture]
    public class DishControllerTests
    {
        private DishController _controller;
        private OfficeBiteDbContext _dbContext;
        private Mock<IHelperMethods> _helperMethodsMock;

        private OfficeBiteDbContext CreateDbContext(List<DishCategory> categories, List<Dish> dishes)
        {
            var options = new DbContextOptionsBuilder<OfficeBiteDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            var dbContext = new OfficeBiteDbContext(options);

            dbContext.DishCategories.AddRange(categories);
            dbContext.Dishes.AddRange(dishes);
            dbContext.SaveChanges();

            return dbContext;
        }
        private static IFormFile CreateFormFile(string fileName, string contentType)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write("Test file content");
            writer.Flush();
            stream.Position = 0;

            return new FormFile(stream, 0, stream.Length, null, Path.GetFileName(fileName))
            {
                Headers = new HeaderDictionary(),
                ContentType = contentType
            };
        }
        [SetUp]
        public void Setup()
        {
            var categories = new List<DishCategory>
            {
                new DishCategory { Id = 1, Name = "Category 1" },
                new DishCategory { Id = 2, Name = "Category 2" }
            };
            var dishes = new List<Dish>
            {
                new Dish { Id = 1, DishName = "Dish 1", Price = 1, CategoryId = 1},
                new Dish { Id = 2, DishName = "Dish 2", Price = 2, CategoryId = 2}
            };

            _dbContext = CreateDbContext(categories, dishes);

       
            _helperMethodsMock = new Mock<IHelperMethods>();
            _helperMethodsMock.Setup(m => m.GetDishesAsync()).ReturnsAsync(new List<DishViewModel>
            {
                new DishViewModel { DishId = 3, DishName = "Mock Dish 1", DishPrice = 3 },
                new DishViewModel { DishId = 4, DishName = "Mock Dish 2", DishPrice = 4 }
            });
            _helperMethodsMock.Setup(m => m.GetCategoryAsync())
                .ReturnsAsync(categories.Select(c => new CategoryViewModel { Id = c.Id, Name = c.Name }));

      
            _controller = new DishController(_helperMethodsMock.Object, _dbContext);

            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "sampleUserId")
            }, "TestAuthType");

            var principal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = principal }
            };
        }


        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
            _controller.Dispose();
            _dbContext.Dispose();
        }
        [Test]
        public async Task AllDishes_ReturnsViewWithModel()
        {
            // Arrange

            var userId = "sampleUserId";
            var userAgent = new UserAgent { UserId = userId };
            _dbContext.UserAgents.AddRange(new List<UserAgent> { userAgent });
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _controller.AllDishes();

            // Assert
            Assert.That(result, Is.InstanceOf<ViewResult>());

            var viewResult = result as ViewResult;
            Assert.That(viewResult.Model, Is.InstanceOf<AllDishesViewModel>());

            var model = viewResult.Model as AllDishesViewModel;

            Assert.That(model.Categories.Count(), Is.EqualTo(2));
            Assert.That(model.Dishes.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task AllDishes_ReturnsUserNullRedirectToPage()
        {
            // Arrange
            var userId = "nullUserId";
            var userAgent = new UserAgent { UserId = userId };
            _dbContext.UserAgents.AddRange(new List<UserAgent> { userAgent });
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _controller.AllDishes();

            // Assert
            Assert.That(result, Is.InstanceOf<RedirectToPageResult>());

        }

        [Test]
        public async Task AllHiddenDishes_ReturnsViewWithModel()
        {
            // Arrange

            var userId = "sampleUserId";
            var userAgent = new UserAgent { UserId = userId };
            _dbContext.UserAgents.AddRange(new List<UserAgent> { userAgent });
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _controller.AllHiddenDishes();

            // Assert
            Assert.That(result, Is.InstanceOf<ViewResult>());

            var viewResult = result as ViewResult;
            Assert.That(viewResult.Model, Is.InstanceOf<AllDishesViewModel>());

            var model = viewResult.Model as AllDishesViewModel;

            Assert.That(model.Categories.Count(), Is.EqualTo(2));
            Assert.That(model.Dishes.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task AllHiddenDishes_ReturnsUserNullRedirectToPage()
        {
            // Arrange
            var userId = "nullUserId";
            var userAgent = new UserAgent { UserId = userId };
            _dbContext.UserAgents.AddRange(new List<UserAgent> { userAgent });
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _controller.AllHiddenDishes();

            // Assert
            Assert.That(result, Is.InstanceOf<RedirectToPageResult>());

        }

        [Test]
        public async Task HideDishReturnsViewWithModel()
        {
            var dishId = await _dbContext.Dishes.FirstOrDefaultAsync(id => id.Id == 1);

            var result = await _controller.HideDish(dishId.Id);

            Assert.That(result, Is.InstanceOf<ViewResult>());

            var viewResult = result as ViewResult;
            Assert.That(viewResult.Model, Is.InstanceOf<DishViewModel>());

            var model = viewResult.Model as DishViewModel;

            Assert.That(model.DishId, Is.EqualTo(1));

        }

        [Test]
        public async Task HideDishConfirmReturnsRedirectToActionResult()
        {
            var dish = await _dbContext.Dishes.FirstOrDefaultAsync(id => id.Id == 1);

            var result = await _controller.HideDishConfirm(dish.Id);

            Assert.That(result, Is.InstanceOf<RedirectToActionResult>());

        }

        [Test]
        public async Task HideDishConfirmReturnDishNotFound()
        {
            var dishId = 3;
            var result = await _controller.HideDishConfirm(dishId);

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task HideDishConfirmReturnsDishIsVisible()
        {
            // Arrange
            var dish = await _dbContext.Dishes.FirstOrDefaultAsync(id => id.Id == 1);
            dish.IsVisible = true;

            // Act
            var result = await _controller.HideDishConfirm(dish.Id);

            // Assert
            Assert.That(result, Is.InstanceOf<RedirectToActionResult>());
            Assert.That(dish.IsVisible, Is.False);
        }

        [Test]
        public async Task HideDishConfirmReturnsDishInMenuIsVisible()
        {
            // Arrange
            var dish = await _dbContext.Dishes.FirstOrDefaultAsync(id => id.Id == 1);
            var dishesInMenu = new DishesInMenu
            {
                IsVisible = true,
                DishId = dish.Id
            };

            await _dbContext.DishesInMenus.AddAsync(dishesInMenu);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _controller.HideDishConfirm(dish.Id);

            // Assert
            Assert.That(result, Is.InstanceOf<RedirectToActionResult>());
            Assert.That(dishesInMenu.IsVisible, Is.False);
        }

        [Test]
        public async Task HideDishConfirmReturnsDishInMenuOrdersIsVisible()
        {
            // Arrange
            var dish = await _dbContext.Dishes.FirstOrDefaultAsync(id => id.Id == 1);
            var dishesInMenu = new DishesInMenu
            {
                IsVisible = true,
                DishId = dish.Id,
                RequestMenuNumber = 1
            };

            await _dbContext.DishesInMenus.AddAsync(dishesInMenu);
            await _dbContext.SaveChangesAsync();

            var menuOrders = new MenuOrder
            {
                RequestMenuNumber = 1,
                IsVisible = true
            };
            await _dbContext.MenuOrders.AddAsync(menuOrders);
            await _dbContext.SaveChangesAsync();
            // Act
            var result = await _controller.HideDishConfirm(dish.Id);

            // Assert
            Assert.That(result, Is.InstanceOf<RedirectToActionResult>());
            Assert.That(menuOrders.IsVisible, Is.False);
        }

        [Test]
        public async Task UnHideDishReturnsViewWithModel()
        {
            var dishId = await _dbContext.Dishes.FirstOrDefaultAsync(id => id.Id == 1);

            var result = await _controller.UnHideDish(dishId.Id);

            Assert.That(result, Is.InstanceOf<ViewResult>());

            var viewResult = result as ViewResult;
            Assert.That(viewResult.Model, Is.InstanceOf<DishViewModel>());

            var model = viewResult.Model as DishViewModel;

            Assert.That(model.DishId, Is.EqualTo(1));
        }

        [Test]
        public async Task UnHideDishConfirmReturnsRedirectToActionResult()
        {
            var dish = await _dbContext.Dishes.FirstOrDefaultAsync(id => id.Id == 1);

            var result = await _controller.UnHideDishConfirm(dish.Id);

            Assert.That(result, Is.InstanceOf<RedirectToActionResult>());

        }

        [Test]
        public async Task AddDish_ShouldReturnAllDishesViewModel()
        {
            var dishId = 3;
            var result = await _controller.UnHideDishConfirm(dishId);

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task UnHideDishConfirmReturnsDishIsVisible()
        {
            // Arrange
            var dish = await _dbContext.Dishes.FirstOrDefaultAsync(id => id.Id == 1);
            dish.IsVisible = false;

            // Act
            var result = await _controller.UnHideDishConfirm(dish.Id);

            // Assert
            Assert.That(result, Is.InstanceOf<RedirectToActionResult>());
            Assert.That(dish.IsVisible, Is.True);
        }

        [Test]
        public async Task UnHideDishConfirmReturnsDishInMenuIsVisible()
        {
            // Arrange
            var dish = await _dbContext.Dishes.FirstOrDefaultAsync(id => id.Id == 1);
            var dishesInMenu = new DishesInMenu
            {
                IsVisible = false,
                DishId = dish.Id
            };

            await _dbContext.DishesInMenus.AddAsync(dishesInMenu);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _controller.UnHideDishConfirm(dish.Id);

            // Assert
            Assert.That(result, Is.InstanceOf<RedirectToActionResult>());
            Assert.That(dishesInMenu.IsVisible, Is.True);
        }

        [Test]
        public async Task UnHideDishConfirmReturnsDishInMenuOrdersIsVisible()
        {
            // Arrange
            var dish = await _dbContext.Dishes.FirstOrDefaultAsync(id => id.Id == 1);
            var dishesInMenu = new DishesInMenu
            {
                IsVisible = false,
                DishId = dish.Id,
                RequestMenuNumber = 1
            };

            await _dbContext.DishesInMenus.AddAsync(dishesInMenu);
            await _dbContext.SaveChangesAsync();

            var menuOrders = new MenuOrder
            {
                RequestMenuNumber = 1,
                IsVisible = false
            };
            await _dbContext.MenuOrders.AddAsync(menuOrders);
            await _dbContext.SaveChangesAsync();
            // Act
            var result = await _controller.UnHideDishConfirm(dish.Id);

            // Assert
            Assert.That(result, Is.InstanceOf<RedirectToActionResult>());
            Assert.That(menuOrders.IsVisible, Is.True);
        }

        [Test]
        public async Task GetEditDishReturnsViewWithModel()
        {
            var dishId = await _dbContext.Dishes
                .FirstOrDefaultAsync(id => id.Id == 1);

            // Act
            var result = await _controller.EditDish(dishId.Id);

            // Assert
            Assert.That(result, Is.InstanceOf<ViewResult>());

            var viewResult = result as ViewResult;
            Assert.That(viewResult.Model, Is.InstanceOf<AllDishesViewModel>());

            var model = viewResult.Model as AllDishesViewModel;

            Assert.That(model.Categories.Count(), Is.EqualTo(2));
            Assert.That(model.CategoryId, Is.EqualTo(1));

        }


        [Test]
        public async Task EditDish_RedirectToModelIsNotValid()
        {
            // Arrange
            var dishId = 1;
            var dish = await _dbContext.Dishes.FindAsync(dishId);

            var dishesInMenu = new DishesInMenu
            {
                DishId = dish.Id,
                RequestMenuNumber = 1
            };

            await _dbContext.DishesInMenus.AddAsync(dishesInMenu);
            await _dbContext.SaveChangesAsync();

            var menuOrdersCreated = new MenuOrder
            {
                RequestMenuNumber = 1
            };
            await _dbContext.MenuOrders.AddAsync(menuOrdersCreated);
            await _dbContext.SaveChangesAsync();

            // Simulate image file upload
            var models = new AllDishesViewModel
            {
                DishId = dishId,
                DishName = "Updated Dish Name",
                DishPrice = 10.99m,
                Description = "Updated Description",
                CategoryId = 2,
                ImageFile = Mock.Of<IFormFile>()
            };

            // Act
            var result = await _controller.EditDish(models, dishId);

            var viewResult = result as ViewResult;
            Assert.That(viewResult.Model, Is.InstanceOf<AllDishesViewModel>());

            var model = viewResult.Model as AllDishesViewModel;

            Assert.That(model.DishId, Is.EqualTo(1));
            ClassicAssert.AreEqual(model.DishName, dish.DishName);
            ClassicAssert.AreEqual(model.DishPrice, dish.Price);
            ClassicAssert.AreEqual(model.Description, dish.Description);
            ClassicAssert.AreEqual(model.CategoryId, dish.CategoryId);

        }
    }

}

