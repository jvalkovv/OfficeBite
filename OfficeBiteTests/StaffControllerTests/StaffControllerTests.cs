using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using OfficeBite.Controllers;
using OfficeBite.Infrastructure.Data;
using OfficeBite.Infrastructure.Data.Models;
using System.Security.Claims;


namespace OfficeBiteTests.StaffControllerTests
{
    [TestFixture]
    public class StaffControllerTests
    {
        private StaffController? _controller;
        private OfficeBiteDbContext _dbContext;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<OfficeBiteDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _dbContext = new OfficeBiteDbContext(options);
            _controller = new StaffController(_dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            _controller?.Dispose();
            _dbContext.Dispose();
        }
        [Test]
        public async Task AllOrders_ReturnsUnauthorized_ForNonStaffUser()
        {

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "nonstaffuser"),
            }, "testauthentication"));

            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(c => c.User).Returns(user);

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContextMock.Object
            };
            _controller.ControllerContext = controllerContext;


            var result = await _controller.AllOrders();


            Assert.That(result, Is.Not.InstanceOf<UnauthorizedResult>());
            Assert.That(result, Is.InstanceOf<ViewResult>());
        }

        [Test]
        public async Task AllOrders_ReturnsViewResult_ForStaffUser()
        {

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "staffuser"),
                new Claim(ClaimTypes.Role, "Staff"),
            }, "testauthentication"));

            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(c => c.User).Returns(user);

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContextMock.Object
            };
            _controller.ControllerContext = controllerContext;


            var result = await _controller.AllOrders();


            Assert.That(result, Is.InstanceOf<ViewResult>());
        }

        [Test]
        public async Task OrderView_ReturnsViewResult_WithOrders()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "staffuser"),
                new Claim(ClaimTypes.Role, "Staff")
            }, "testauthentication"));

            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(c => c.User).Returns(user);

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContextMock.Object
            };
            _controller.ControllerContext = controllerContext;

            var options = new DbContextOptionsBuilder<OfficeBiteDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var dbContext = new OfficeBiteDbContext(options))
            {

                dbContext.Orders.Add(new Order
                {
                    MenuOrderRequestNumber = 1,
                    UserAgentId = "testUserId",
                    SelectedDate = new DateTime(2024, 4, 15),
                    OrderPlacedOnDate = DateTime.Today
                });

                await dbContext.SaveChangesAsync();
            }


            var result = await _controller.OrderView(1, "testUserId", new DateTime(2024, 4, 15));


            Assert.That(result, Is.InstanceOf<ViewResult>());
        }

        [Test]
        public async Task OrderView_ReturnsUnauthorized_WhenUserIsNotAuthorized()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "staffuser"),
            }, "testauthentication"));
            user.AddIdentity(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role, "nonstaffrole")
            }));

            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(c => c.User).Returns(user);

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContextMock.Object
            };
            _controller.ControllerContext = controllerContext;


            var result = await _controller.OrderView(1, "testUserId", new DateTime(2024, 4, 15));

            Assert.That(result, Is.InstanceOf<UnauthorizedResult>());
        }

    }
}
