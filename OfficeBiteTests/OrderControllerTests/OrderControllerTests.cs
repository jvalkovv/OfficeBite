//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Moq;
//using NUnit.Framework.Legacy;
//using OfficeBite.Controllers;
//using OfficeBite.Core.Extensions.Interfaces;
//using OfficeBite.Core.Models.CategoryModels;
//using OfficeBite.Core.Models.DishModels;
//using OfficeBite.Core.Models.OrderModels;
//using OfficeBite.Infrastructure.Data;
//using OfficeBite.Infrastructure.Data.Models;

//namespace OfficeBiteTests.OrderControllerTests
//{
//    public class OrderControllerTests
//    {
//        private OrderController? _controller;
//        private Mock<IHelperMethods> _helperMethodsMock;
//        private OfficeBiteDbContext _dbContext;

//        [SetUp]
//        public void Setup()
//        {

//            _helperMethodsMock = new Mock<IHelperMethods>();

//            var categories = new List<DishCategory>
//            {
//                new DishCategory { Id = 1, Name = "Category 1" },
//                new DishCategory { Id = 2, Name = "Category 2" }
//            };
//            var dishes = new List<Dish>
//            {
//                new Dish { Id = 1, DishName = "Dish 1", Price = 10, CategoryId = 1 },
//                new Dish { Id = 2, DishName = "Dish 2", Price = 15, CategoryId = 2 }
//            };
//            var menuTypes = new List<MenuType>
//            {
//                new MenuType { Id = 1, Name = "Type 1" },
//                new MenuType { Id = 2, Name = "Type 2" }
//            };
//            var menuOrder = new List<MenuOrder>
//            {
//                new MenuOrder { Id = 1, RequestMenuNumber = 1 },
//                new MenuOrder { Id = 2, RequestMenuNumber = 2 }
//            };

//            var users = new List<IdentityUser>
//            {
//                new IdentityUser { Id = "adminuserId", UserName = "adminuser" },
//                new IdentityUser { Id = "manageruserId", UserName = "manageruser" },
//                new IdentityUser { Id = "newuserId", UserName = "newuser" }
//            };

//            var userAgents = users.Select(user => new UserAgent
//            {
//                UserId = user.Id,
//                FirstName = "FirstName",
//                LastName = "LastName",
//                Username = user.UserName
//            }).ToList();

//            _helperMethodsMock = new Mock<IHelperMethods>();
//            _helperMethodsMock.Setup(m => m.GetDishesAsync()).ReturnsAsync(new List<DishViewModel>
//            {
//                new DishViewModel { DishId = 3, DishName = "Mock Dish 1", DishPrice = 10 },
//                new DishViewModel { DishId = 4, DishName = "Mock Dish 2", DishPrice = 15 }
//            });
//            _helperMethodsMock.Setup(m => m.GetCategoryAsync()).ReturnsAsync(categories.Select(c => new CategoryViewModel { Id = c.Id, Name = c.Name }));

//            _dbContext = CreateDbContext(dishes, categories, menuOrder, menuTypes, userAgents, users);

//            _controller = new OrderController(_dbContext, _helperMethodsMock.Object);

//        }

//        private OfficeBiteDbContext CreateDbContext(List<Dish> dishes, List<DishCategory> categories, List<MenuOrder> menuOrders, List<MenuType> menuTypes,
//            List<UserAgent> userAgents, List<IdentityUser> users)
//        {
//            var options = new DbContextOptionsBuilder<OfficeBiteDbContext>()
//                .UseInMemoryDatabase(databaseName: "TestDatabase")
//                .Options;
//            var dbContext = new OfficeBiteDbContext(options);

//            dbContext.Dishes.AddRange(dishes);
//            dbContext.DishCategories.AddRange(categories);
//            dbContext.UserAgents.AddRange(userAgents);
//            dbContext.MenuOrders.AddRange(menuOrders);
//            dbContext.Users.AddRange(users);
//            dbContext.MenuTypes.AddRange(menuTypes);
//            dbContext.SaveChanges();

//            return dbContext;
//        }
//        [TearDown]
//        public void TearDown()
//        {
//            _dbContext.Database.EnsureDeleted();
//            _dbContext.Dispose();
//            _controller.Dispose();
//        }

//        [Test]
//        public async Task AddToOrder_Get_ReturnsViewWithCorrectModel()
//        {
//            // Act
//            var result = await _controller.AddToOrder() as ViewResult;

//            // Assert
//            ClassicAssert.IsNotNull(result);
//            ClassicAssert.IsInstanceOf<AddOrderViewModel>(result.Model);
//        }


//        [Test]
//        public async Task AddToOrder_OrderCountLessThanZero_BadRequest()
//        {

//            var userId = _dbContext.Users.First(u => u.Id == "adminuserId");

//            var dishId = await _dbContext.Dishes.FirstOrDefaultAsync(id => id.Id == 1);
//            var orderId = await _dbContext.MenuOrders.FirstOrDefaultAsync(r => r.RequestMenuNumber == 1);

//            var menuOrders = await _dbContext.MenuOrders.FirstOrDefaultAsync(r => r.RequestMenuNumber == 1);

//            _dbContext.MenuOrders.AddRange(menuOrders);


//            var result = await _controller.AddToOrder(1);
//            // Act

//            // Assert
//            ClassicAssert.IsInstanceOf<BadRequestObjectResult>(result);
//        }
//    }
//}
