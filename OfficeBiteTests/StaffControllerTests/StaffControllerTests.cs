using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using OfficeBite.Core.Services;
using OfficeBite.Core.Services.Contracts;
using OfficeBite.Infrastructure.Data;
using OfficeBite.Infrastructure.Data.Common;
using OfficeBite.Infrastructure.Data.Models;

namespace OfficeBiteTests.StaffControllerTests
{
    [TestFixture]
    public class StaffControllerTests
    {
        private IStaffService staffService;
        private Mock<IRepository> repositoryMock;
        private OfficeBiteDbContext dbContext;

        [SetUp]
        public void Setup()
        {
            repositoryMock = new Mock<IRepository>();

            var options = new DbContextOptionsBuilder<OfficeBiteDbContext>()
                .UseInMemoryDatabase(databaseName: "Test_Staff_Database")
                .Options;

            dbContext = new OfficeBiteDbContext(options);

            repositoryMock.Setup(repo => repo.AllReadOnly<Order>())
                .Returns(dbContext.Orders);
            repositoryMock.Setup(repo => repo.AllReadOnly<MenuOrder>())
                .Returns(dbContext.MenuOrders);
            repositoryMock.Setup(repo => repo.AllReadOnly<DishesInMenu>())
                .Returns(dbContext.DishesInMenus);

            staffService = new StaffService(repositoryMock.Object);
        }

        [TearDown]
        public void TearDown()
        {

            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

        }

        [Test]
        public async Task AllOrders_ShouldReturnAllOrdersGroupedAndSortedByDate()
        {
            var orders = new List<Order>
            {
                new Order { Name = "Order 1", MenuOrderRequestNumber = 1, UserAgentId = "user1",
                    SelectedDate = DateTime.Now.Date, OrderPlacedOnDate = DateTime.Now, IsEaten = false },
                new Order { Name = "Order 2", MenuOrderRequestNumber = 2, UserAgentId = "user2",
                    SelectedDate = DateTime.Now.Date.AddDays(1), OrderPlacedOnDate = DateTime.Now.AddDays(1), IsEaten = false },
                new Order { Name = "Order 3", MenuOrderRequestNumber = 1, UserAgentId = "user1",
                    SelectedDate = DateTime.Now.Date, OrderPlacedOnDate = DateTime.Now, IsEaten = false },
            };
            var menuOrders = new List<MenuOrder>
            {
                new MenuOrder { RequestMenuNumber = 1, TotalPrice = 10 },
                new MenuOrder { RequestMenuNumber = 2, TotalPrice = 15 },
            };

            var userAgents = new List<UserAgent>
            {
                new UserAgent { UserId = "user1" },
                new UserAgent { UserId = "user2" }
            };
            await dbContext.UserAgents.AddRangeAsync(userAgents);
            await dbContext.Orders.AddRangeAsync(orders);
            await dbContext.MenuOrders.AddRangeAsync(menuOrders);
            await dbContext.SaveChangesAsync();


            var result = await staffService.AllOrders();


            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].MenuToOrderId, Is.EqualTo(1));
            Assert.That(result[1].MenuToOrderId, Is.EqualTo(2));
        }

        [Test]
        public async Task OrderView_ShouldReturnOrderViewModel()
        {
            var orders = new List<Order>
            {
                new Order { Name = "Order 1", MenuOrderRequestNumber = 1, UserAgentId = "user1",
                    SelectedDate = DateTime.Now.Date, OrderPlacedOnDate = DateTime.Now, IsEaten = false },
                new Order { Name = "Order 2", MenuOrderRequestNumber = 2, UserAgentId = "user2",
                    SelectedDate = DateTime.Now.Date.AddDays(1), OrderPlacedOnDate = DateTime.Now.AddDays(1), IsEaten = false },
                new Order { Name = "Order 3", MenuOrderRequestNumber = 1, UserAgentId = "user1",
                    SelectedDate = DateTime.Now.Date, OrderPlacedOnDate = DateTime.Now, IsEaten = false },
            };
            var menuOrders = new List<MenuOrder>
            {
                new MenuOrder { RequestMenuNumber = 1, TotalPrice = 10 },
                new MenuOrder { RequestMenuNumber = 2, TotalPrice = 15 },
            };

            var dishesInMenus = new List<DishesInMenu>
            {
                new DishesInMenu { Id = 1, IsVisible = true, DishId = 1, RequestMenuNumber = 1 },
                new DishesInMenu { Id = 2, IsVisible = true, DishId = 2, RequestMenuNumber = 2 },
            };
            var userAgents = new List<UserAgent>
            {
                new UserAgent { UserId = "user1", FirstName = "John", LastName = "Doe", Username = "johnd"},
                new UserAgent { UserId = "user2" }
            };

            await dbContext.DishesInMenus.AddRangeAsync(dishesInMenus);
            await dbContext.UserAgents.AddRangeAsync(userAgents);
            await dbContext.Orders.AddRangeAsync(orders);
            await dbContext.MenuOrders.AddRangeAsync(menuOrders);
            await dbContext.SaveChangesAsync();

            var order = dbContext.Orders.FirstAsync();
            var user = dbContext.UserAgents.FirstAsync();
            var date = order.Result.SelectedDate;
       
            var result = await staffService.OrderView(order.Result.Id, user.Result.UserId, date);


            Assert.That(result, Is.Not.Null);
            Assert.That(result.FirstName, Is.EqualTo("John"));
            Assert.That(result.LastName, Is.EqualTo("Doe"));
            Assert.That(result.CustomerUsername, Is.EqualTo("johnd"));
            Assert.That(result.SelectedDate, Is.EqualTo(date));
            Assert.That(result.TotalSum, Is.EqualTo(10));
            Assert.That(result.RequestMenuNumber, Is.EqualTo(1));
        }

        [Test]
        public async Task OrderComplete_ShouldReturnOrderViewModel()
        {
            var selectedDate = DateTime.Now.Date;
            var username = "johnd";
            var userId = "user1";
            var orderId = 1;
            var orders = new List<Order>
            {
                new Order { Id = orderId, Name = "Order 1", UserAgent = new UserAgent 
                    { UserId = userId, FirstName = "John", LastName = "Doe", Username = "johnd" }, 
                    MenuOrder = new MenuOrder { RequestMenuNumber = 1, TotalPrice = 10 }, 
                    SelectedDate = selectedDate, OrderPlacedOnDate = DateTime.Now, IsEaten = false },
            };
            dbContext.Orders.AddRange(orders);
            await dbContext.SaveChangesAsync();

        
            var result = await staffService.OrderComplete(selectedDate, username, userId, orderId);


            Assert.That(result.OrderId, Is.EqualTo(orderId));
            Assert.That(result.OrderName, Is.EqualTo("Order 1"));
            Assert.That(result.CustomerFirstName, Is.EqualTo("John"));
            Assert.That(result.CustomerLastName, Is.EqualTo("Doe"));
            Assert.That(result.CustomerUsername, Is.EqualTo("johnd"));
            Assert.That(result.LunchDate, Is.EqualTo(selectedDate));
            Assert.That(result.TotalPrice, Is.EqualTo(10));
            Assert.That(result.MenuToOrderId, Is.EqualTo(1));
        }

    }
}
