//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.ViewFeatures;
//using Microsoft.EntityFrameworkCore;
//using Moq;
//using NUnit.Framework.Legacy;
//using OfficeBite.Controllers;
//using OfficeBite.Core.Extensions.Interfaces;
//using OfficeBite.Core.Models.CategoryModels;
//using OfficeBite.Core.Models.DishModels;
//using OfficeBite.Core.Models.MenuModels;
//using OfficeBite.Core.Services.Contracts;
//using OfficeBite.Infrastructure.Data;
//using OfficeBite.Infrastructure.Data.Models;
//using OfficeBite.Infrastructure.Extensions.InterfaceForTest;

//namespace OfficeBiteTests.MenuControllerTests
//{
//    [TestFixture]
//    public class MenuControllerTests
//    {
//        private MenuController _controller;
//        private DbContextOptions<OfficeBiteDbContext> _options;
//        private IMenuService menuServiceMock;
//        private Mock<IHelperMethods> helperMethodsMock;

//        private OfficeBiteDbContext CreateDbContext(List<DishCategory> categories, List<Dish> dishes, List<MenuType> menuTypes)
//        {
//            var options = new DbContextOptionsBuilder<OfficeBiteDbContext>()
//                .UseInMemoryDatabase(databaseName: "TestDatabase")
//                .Options;
//            var dbContext = new OfficeBiteDbContext(options);

//            dbContext.DishCategories.AddRange(categories);
//            dbContext.Dishes.AddRange(dishes);
//            dbContext.MenuTypes.AddRange(menuTypes);
//            dbContext.SaveChanges();

//            return dbContext;
//        }

//        [TearDown]
//        public void TearDown()
//        {
//            menuServiceMock.Database.EnsureDeleted();
//            dbContext.Dispose();
//            _controller.Dispose();

//        }
//        [SetUp]
//        public void Setup()
//        {

//            var categories = new List<DishCategory>
//            {
//                new DishCategory { Id = 1, Name = "Category 1" },
//                new DishCategory { Id = 2, Name = "Category 2" }
//            };
//            var dishes = new List<Dish>
//            {
//                new Dish { Id = 3, DishName = "Dish 1", Price = 10, CategoryId = 1 },
//                new Dish { Id = 4, DishName = "Dish 2", Price = 15, CategoryId = 2 }
//            };
//            var menuTypes = new List<MenuType>
//            {
//                new MenuType { Id = 5, Name = "Type 1" },
//                new MenuType { Id = 6, Name = "Type 2" }
//            };

//            dbContext = CreateDbContext(categories, dishes, menuTypes);

//            // Create a mock of the IHelperMethods interface
//            helperMethodsMock = new Mock<IHelperMethods>();
//            helperMethodsMock.Setup(m => m.GetDishesAsync()).ReturnsAsync(new List<DishViewModel>
//            {
//                new DishViewModel { DishId = 3, DishName = "Mock Dish 1", DishPrice = 10 },
//                new DishViewModel { DishId = 4, DishName = "Mock Dish 2", DishPrice = 15 }
//            });
//            helperMethodsMock.Setup(m => m.GetCategoryAsync()).ReturnsAsync(categories.Select(c => new CategoryViewModel { Id = c.Id, Name = c.Name }));

//            helperMethodsMock.Setup(m => m.GetMenuTypesAsync())
//                .ReturnsAsync(menuTypes.Select(m => new MenuTypeViewModel { MenuTypeId = m.Id, MenuName = m.Name }).ToList());


//            // Inject the mock into the controller
//            _controller = new MenuController(menuServiceMock);
//        }



//        [Test]
//        public async Task MenuList_ReturnsViewResult_WithMenuViewModel()
//        {
//            // Act
//            var result = await _controller.MenuList();

//            // Assert
//            Assert.That(result, Is.InstanceOf<ViewResult>());

//            var viewResult = result as ViewResult;
//            Assert.That(viewResult.Model, Is.InstanceOf<MenuViewModel>());

//            var model = viewResult.Model as MenuViewModel;
//            Assert.That(model.Categories.Count(), Is.EqualTo(2));
//            Assert.That(model.Dishes.Count(), Is.EqualTo(2));
//        }

//        [Test]
//        public async Task MenuDailyList_ReturnsViewResult_WithMenuDailyViewModel()
//        {
//            // Arrange
//            var tempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());
//            tempData["OrderExistsError"] = "Your error message";
//            _controller.TempData = tempData;

//            // Act
//            var result = await _controller.MenuDailyList();

//            // Assert
//            Assert.That(result, Is.InstanceOf<ViewResult>());

//            var viewResult = result as ViewResult;
//            Assert.That(viewResult.Model, Is.InstanceOf<MenuDailyViewModel>());

//            var model = viewResult.Model as MenuDailyViewModel;
//            Assert.That(model.Dishes.Count(), Is.EqualTo(2));
//            Assert.That(model.Categories.Count(), Is.EqualTo(2));
//            Assert.That(model.MenuTypes.Count(), Is.EqualTo(2));
//        }

//        [Test]
//        public async Task AddDishToMenu_ReturnsViewResult_WithAddDishToMenuViewModel()
//        {
//            // Arrange
//            var dishes = new List<DishViewModel>
//            {
//                new DishViewModel { DishId = 1, DishName = "Dish 1", DishPrice = 10 },
//                new DishViewModel { DishId = 2, DishName = "Dish 2", DishPrice = 15 }
//            };

//            var menuTypes = new List<MenuTypeViewModel>
//            {
//                new MenuTypeViewModel { MenuTypeId = 1, MenuName = "Menu Type 1" },
//                new MenuTypeViewModel { MenuTypeId = 2, MenuName = "Menu Type 2" }
//            };

//            helperMethodsMock.Setup(m => m.GetDishForMenuAsync()).ReturnsAsync(dishes);
//            helperMethodsMock.Setup(m => m.GetMenuTypesAsync()).ReturnsAsync(menuTypes);

//            // Act
//            var result = await _controller.AddDishToMenu();

//            // Assert
//            Assert.That(result, Is.InstanceOf<ViewResult>());

//            var viewResult = result as ViewResult;
//            Assert.That(viewResult.ViewName, Is.Null.Or.Empty);
//            Assert.That(viewResult.Model, Is.InstanceOf<AddDishToMenuViewModel>());

//            var model = viewResult.Model as AddDishToMenuViewModel;
//            Assert.That(model.AllDishes.Count, Is.EqualTo(2));
//            Assert.That(model.AllMenuTypes.Count, Is.EqualTo(2));
//        }


//        [Test]
//        public async Task AddDishToMenu_ReturnsRedirectToActionResult_WhenModelStateIsValid()
//        {
//            // Arrange
//            var model = new AddDishToMenuViewModel
//            {
//                MenuTypeId = 1,
//                SelectedDishes = new List<int> { 3, 4 },
//                SelectedDates = new List<string> { "2024-04-14" }
//            };

//            // Act
//            var result = await _controller.AddDishToMenu(model);

//            // Assert
//            Assert.That(result, Is.InstanceOf<RedirectToActionResult>());
//            var redirectResult = result as RedirectToActionResult;
//            Assert.That(redirectResult.ActionName, Is.EqualTo("AllDishes"));
//            Assert.That(redirectResult.ControllerName, Is.EqualTo("Dish"));
//        }

//        [Test]
//        public async Task AddDishToMenu_SavesDishesInMenu_WhenDishAndMenuTypeExist()
//        {
//            var model = new AddDishToMenuViewModel
//            {
//                MenuTypeId = 5,
//                SelectedDishes = new List<int> { 3, 4 },
//                SelectedDates = new List<string> { "2024-04-14" },
//                RequestMenuNumber = 1
//            };

//            // Act
//            var result = await _controller.AddDishToMenu(model);

//            // Assert
//            // Check if dishes are added to the DishesInMenu table

//            foreach (var dishId in model.SelectedDishes)
//            {
//                var addedDishMenu = await dbContext.DishesInMenus.FirstOrDefaultAsync(d => d.DishId == dishId);
//               ClassicAssert.IsNotNull(addedDishMenu);
//               ClassicAssert.AreEqual(model.RequestMenuNumber, addedDishMenu.RequestMenuNumber);
//            }

//        }
//        [Test]
//        public async Task AddDishToMenu_ReturnsViewResult_WhenModelIsInvalid()
//        {
//            // Arrange: Pass a valid model with null SelectedDates
//            var model = new AddDishToMenuViewModel
//            {
//                MenuTypeId = 1,
//                SelectedDishes = new List<int> { 3, 4 },
//                SelectedDates = null,  
//                RequestMenuNumber = 1
//            };

//            // Act: Call the controller action
//            var result = await _controller.AddDishToMenu(model);

//            // Assert: Verify that the controller returns a ViewResult
//            Assert.That(result, Is.InstanceOf<ViewResult>());
//            var viewResult = result as ViewResult;

//            // Assert: Verify that the view name is null or empty
//            Assert.That(viewResult.ViewName, Is.Null.Or.Empty);

//        }

//        [Test]
//        public async Task MenuDailyList_ReturnsBadRequest_WhenModelStateIsInvalid()
//        {
//            DateTime yesterday = DateTime.Today.AddDays(-1);

//            var validModel = new MenuDailyViewModel { SelectedDate = yesterday };

//            // Act: Call the controller action
//            var result = await _controller.MenuDailyList(validModel);

//            // Assert: Verify the content of the JSON response
//            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
//            var badRequestResult = result as BadRequestObjectResult;
//            ClassicAssert.AreEqual("Invalid date for order", badRequestResult.Value);
//        }
//        [Test]
//        public async Task MenuDailyList_ReturnsViewResult_WhenCurrentTimeIsBefore11AM_OrSelectedDateIsInTheFuture()
//        {
//            // Arrange: Pass a valid model
//            var validModel = new MenuDailyViewModel { SelectedDate = DateTime.Today.AddDays(1) };
//            var dateForOrder = validModel.SelectedDate.Date;

//            // Add a dish to the menu for the selected date (in the future)
//            var model = new AddDishToMenuViewModel
//            {
//                MenuTypeId = 5,
//                SelectedDishes = new List<int> { 3, 4 },
//                SelectedDates = new List<string> { dateForOrder.Date.ToString() },
//                RequestMenuNumber = 1
//            };
//            var dateResult = await _controller.AddDishToMenu(model);

//            // Act: Call the controller action MenuDailyList
//            var result = await _controller.MenuDailyList(validModel);

//            // Assert: Verify that the controller returns a ViewResult
//            Assert.That(result, Is.InstanceOf<ViewResult>());
//            var viewResult = result as ViewResult;

//        }
//        private void SetCurrentDateTime(DateTime fakeNow)
//        {
//            var dateTimeWrapperMock = new Mock<IDateTimeNowWrapper>();
//            dateTimeWrapperMock.Setup(m => m.Now).Returns(fakeNow);
//            _controller.SetDateTimeWrapper(dateTimeWrapperMock.Object);
//        }
//        [Test]
//        public async Task MenuDailyList_ReturnsJsonResult_WhenCurrentTimeIsAfter11AM_AndSelectedDateIsToday()
//        {
//            var validModel = new MenuDailyViewModel { SelectedDate = DateTime.Today };
//            var dateForOrder = validModel.SelectedDate.Date;

//            DateTime fakeNow = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 12, 0, 0);

//            SetCurrentDateTime(fakeNow);

//            // Add a dish to the menu for the selected date (in the future)
//            var model = new AddDishToMenuViewModel
//            {
//                MenuTypeId = 5,
//                SelectedDishes = new List<int> { 3, 4 },
//                SelectedDates = new List<string> { dateForOrder.Date.ToString() },
//                RequestMenuNumber = 1
//            };
//            var dateResult = await _controller.AddDishToMenu(model);

//            // Act: Call the controller action MenuDailyList
//            var result = await _controller.MenuDailyList(validModel);

//            // Assert: Verify that the controller returns a JsonResult
//            Assert.That(result, Is.InstanceOf<JsonResult>());
//            var jsonResult = result as JsonResult;

//            // Assert: Verify the content of the JSON response
//            ClassicAssert.AreEqual("Менюто е АЛАМИНУТ", jsonResult.Value);
//        }
//    }
//}
