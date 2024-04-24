using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework.Legacy;
using OfficeBite.Controllers;
using OfficeBite.Core.Extensions.Interfaces;
using OfficeBite.Core.Models.OrderModels;
using OfficeBite.Core.Services.Contracts;
using OfficeBite.Infrastructure.Data;
using OfficeBite.Infrastructure.Data.Models;

namespace OfficeBiteTests.OrderControllerTests
{
    public class OrderControllerTests
    {
        private OrderController _controller;
        private Mock<IHelperMethods> _helperMethodsMock;
        private Mock<IOrderService> _orderServiceMock;
        private OfficeBiteDbContext _dbContext;

        [SetUp]
        public void Setup()
        {
            _helperMethodsMock = new Mock<IHelperMethods>();
            _orderServiceMock = new Mock<IOrderService>();

            var dishes = new List<Dish>
            {
                new Dish { Id = 6, DishName = "Dish 6", Price = 10 },
                new Dish { Id = 7, DishName = "Dish 7", Price = 15 }
            };

            var categories = new List<DishCategory>
            {
                new DishCategory { Id = 6, Name = "Category 6" },
                new DishCategory { Id = 7, Name = "Category 7" }
            };

            var options = new DbContextOptionsBuilder<OfficeBiteDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _dbContext = new OfficeBiteDbContext(options);

            _dbContext.Dishes.AddRange(dishes);
            _dbContext.DishCategories.AddRange(categories);
            _dbContext.SaveChanges();

            var httpContext = new DefaultHttpContext();
            httpContext.RequestServices = new ServiceCollection().BuildServiceProvider();
            _controller = new OrderController(_orderServiceMock.Object)
            {
                TempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
            };
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
            _controller.Dispose();
        }

        [Test]
        public async Task AddToOrder_Get_ReturnsViewWithCorrectModel()
        {
            var model = new AddOrderViewModel { };

            _helperMethodsMock.Setup(m =>
                m.GetDishesAsync()).ReturnsAsync(model.AllDishes);


            var result = await _controller.AddToOrder();


            ClassicAssert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task AddToOrder_OrderRedirectToAction()
        {

            var userId = "adminuserId";
            var menuOrder = new MenuOrder { RequestMenuNumber = 1 };

            _orderServiceMock
                .Setup(o => o.AddToOrder(It.IsAny<int>()))
                .Returns(Task.FromResult(menuOrder));

            var result = await _controller.AddToOrder(01);

            ClassicAssert.IsNotNull(result);
            ClassicAssert.IsInstanceOf<RedirectToActionResult>(result);
        }

        [Test]
        public async Task AddToOrder_ServiceThrowsInvalidOrder_RedirectsToAccessDeniedPage()
        {

            _orderServiceMock
               .Setup(o => o.AddToOrder(It.IsAny<int>()))
               .ThrowsAsync(new InvalidOperationException("Invalid order"));

            // Act
            var result = await _controller.AddToOrder(1);


            ClassicAssert.IsInstanceOf<RedirectToPageResult>(result);
            var redirectResult = result as RedirectToPageResult;
            ClassicAssert.AreEqual("/Areas/Identity/Pages/Account/AccessDenied", redirectResult.PageName);
        }

        [Test]
        public async Task AddToOrder_ServiceThrowsInvalidDate_SetsTempDataAndRedirectsToMenuDailyList()
        {
            _orderServiceMock
                .Setup(o => o.AddToOrder(It.IsAny<int>()))
                .ThrowsAsync(new InvalidOperationException("Invalid date"));

            var tempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());


            var result = await _controller.AddToOrder(1);


            ClassicAssert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            ClassicAssert.AreEqual("MenuDailyList", redirectResult.ActionName);
            ClassicAssert.AreEqual("Menu", redirectResult.ControllerName);
            ClassicAssert.AreEqual("Потребителят вече има поръчка за тази дата.", _controller.TempData["OrderExistsError"]);
        }


        [Test]
        public async Task AddToOrder_ValidRequest_RedirectsToMenuDailyList()
        {

            _orderServiceMock
                .Setup(o => o.AddToOrder(It.IsAny<int>()))
                .Returns(Task.CompletedTask);


            var result = await _controller.AddToOrder(1);


            ClassicAssert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            ClassicAssert.AreEqual("MenuDailyList", redirectResult.ActionName);
            ClassicAssert.AreEqual("Menu", redirectResult.ControllerName);
        }

    }
}
