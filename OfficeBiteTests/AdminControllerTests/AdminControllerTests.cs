//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Moq;
//using OfficeBite.Controllers;
//using OfficeBite.Core.Models.AdminModels;
//using OfficeBite.Infrastructure.Data;
//using System.Security.Claims;
//using UserAgent = OfficeBite.Infrastructure.Data.Models.UserAgent;

//namespace OfficeBiteTests.AdminControllerTests
//{
//    [TestFixture]
//    public class AdminControllerTests
//    {
//        private AdminController _controller;
//        private OfficeBiteDbContext _dbContext;
//        private Mock<UserManager<IdentityUser>> _userManagerMock;
//        private Mock<RoleManager<IdentityRole>> _roleManagerMock;

//        [SetUp]
//        public void Setup()
//        {
//            var userStoreMock = new Mock<IUserStore<IdentityUser>>();
//            _userManagerMock = new Mock<UserManager<IdentityUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);

//            var roleStoreMock = new Mock<IRoleStore<IdentityRole>>();
//            _roleManagerMock = new Mock<RoleManager<IdentityRole>>(roleStoreMock.Object, null, null, null, null);

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

//            var roles = new List<IdentityRole>
//            {
//                new IdentityRole { Id = "AdminRoleId", Name = "Admin" },
//                new IdentityRole { Id = "ManagerRoleId", Name = "ManagerRole" },
//                new IdentityRole { Id = "NewRoleId", Name = "NewRole" }
//            };

//            _dbContext = CreateDbContext(users, roles, userAgents);

//            _userManagerMock.Setup(u => u.Users).Returns(users.AsQueryable());

//            _userManagerMock.Setup(u => u.FindByIdAsync(It.IsAny<string>()))
//                .ReturnsAsync((string userId) => users.FirstOrDefault(u => u.Id == userId));

//            _userManagerMock.Setup(u => u.GetRolesAsync(It.IsAny<IdentityUser>()))
//                .ReturnsAsync((IdentityUser user) =>
//                {
//                    var userRoles = _dbContext.UserRoles
//                        .Where(ur => ur.UserId == user.Id)
//                        .Join(_dbContext.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Name)
//                        .ToList();
//                    return userRoles;
//                });

//            _controller = new AdminController(_userManagerMock.Object, _roleManagerMock.Object, _dbContext);
//        }

//        private OfficeBiteDbContext CreateDbContext(List<IdentityUser> users, List<IdentityRole> roles, List<UserAgent> userAgents)
//        {
//            var options = new DbContextOptionsBuilder<OfficeBiteDbContext>()
//                .UseInMemoryDatabase(databaseName: "TestDatabase")
//                .Options;
//            var dbContext = new OfficeBiteDbContext(options);

//            dbContext.Users.AddRange(users);
//            dbContext.Roles.AddRange(roles);
//            dbContext.UserAgents.AddRange(userAgents);
//            dbContext.SaveChanges();

//            return dbContext;
//        }

//        [TearDown]
//        public void TearDown()
//        {
//            _dbContext.Database.EnsureDeleted();
//            _controller.Dispose();
//            _dbContext.Dispose();
//        }

//        [Test]
//        public async Task RoleToUser_AddSuccessfullyRoleAssignRole()
//        {
           
//            var newUser = _dbContext.Users.First(u => u.Id == "newuserId");
//            var newRole = _dbContext.Roles.First(r => r.Id == "NewRoleId");

//            _userManagerMock.Setup(u => u.FindByIdAsync(It.IsAny<string>()))
//                .ReturnsAsync((string userId) => _dbContext.Users.FirstOrDefault(u => u.Id == userId));

//            _roleManagerMock.Setup(r => r.FindByIdAsync(It.IsAny<string>()))
//                .ReturnsAsync((string roleId) => _dbContext.Roles.FirstOrDefault(r => r.Id == roleId));

//            _userManagerMock.Setup(u => u.AddToRoleAsync(newUser, newRole.Name))
//                .ReturnsAsync(IdentityResult.Success);
//            _userManagerMock.Setup(u => u.GetRolesAsync(It.IsAny<IdentityUser>()))
//                .ReturnsAsync(new List<string> { "NewRoleId", "ManagerRole" });
       
//            var result = await _controller.AssignRole(new AdminPanelViewModel { UserId = newUser.Id, RoleId = newRole.Id });

       
//            Assert.That(result, Is.InstanceOf<RedirectToActionResult>());
//            var redirectResult = result as RedirectToActionResult;
//            Assert.That(redirectResult.ActionName, Is.EqualTo("Admin"));
//            _userManagerMock.Verify(u => u.AddToRoleAsync(newUser, newRole.Name), Times.Once);
//        }


//        [Test]
//        public async Task AssignRole_ReturnsBadRequest_WhenAddToRoleFails()
//        {
//            var model = new AdminPanelViewModel();
//            var user = new IdentityUser { Id = "userId" };
//            var role = new IdentityRole { Id = "roleId", Name = "RoleName" };

//            _userManagerMock.Setup(u => u.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(user);
//            _roleManagerMock.Setup(r => r.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(role);

//            var failedResult = IdentityResult.Failed(new IdentityError { Description = "Failed to assign role." });
//            _userManagerMock.Setup(um => um.AddToRoleAsync(user, role.Name)).ReturnsAsync(failedResult);

           
//            var actionResult = await _controller.AssignRole(model);


//            Assert.That(actionResult, Is.InstanceOf<BadRequestObjectResult>());
//        }

//        [Test]
//        public async Task ReturnsUnauthorized_ForNonAdminOrManagerUser()
//        {
//            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
//            {
//                new Claim(ClaimTypes.Name, "nonadminormanageruser"),
//                new Claim(ClaimTypes.Role, "nonadminormanagerrole"),
//            }, "auth"));

//            var httpContextMock = new Mock<HttpContext>();
//            httpContextMock.Setup(c => c.User).Returns(user);

//            var controllerContext = new ControllerContext()
//            {
//                HttpContext = httpContextMock.Object
//            };
//            _controller.ControllerContext = controllerContext;

//            var result = await _controller.Admin();

         
//            Assert.That(result, Is.InstanceOf<UnauthorizedResult>());
//        }

//        [Test]
//        public async Task ReturnsAuthorized_ForAdminOrManagerUser()
//        {
//            var adminUser = _dbContext.Users.First(u => u.Id == "adminuserId");
//            var adminRole = _dbContext.Roles.First(r => r.Name == "Admin");
//            var userAgent = _dbContext.UserAgents.First(u => u.IdentityUser.Id == "adminuserId");

          
//            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
//            {
//                new Claim(ClaimTypes.Name, adminUser.UserName),
//                new Claim(ClaimTypes.Role, adminRole.Name),
//                new Claim("UserAgent", userAgent.UserId),
//            }, "auth"));


//            var httpContextMock = new Mock<HttpContext>();
//            httpContextMock.Setup(c => c.User).Returns(user);

//            var controllerContext = new ControllerContext()
//            {
//                HttpContext = httpContextMock.Object
//            };
//            _controller.ControllerContext = controllerContext;

//            var result = await _controller.Admin();

//            Assert.That(result, Is.InstanceOf<ViewResult>());
//        }

//        [Test]
//        public async Task AssignRole_ReturnsNotFoundIfUserNotFound()
//        {
//            var model = new AdminPanelViewModel { UserId = "nonexistentuserid", RoleId = "1" };

//            var result = await _controller.AssignRole(model);

//            Assert.That(result, Is.InstanceOf<NotFoundResult>());
//        }

//        [Test]
//        public async Task AssignRole_ReturnsNotFoundIfRoleNotFound()
//        {
//            var model = new AdminPanelViewModel { UserId = "adminuserId", RoleId = "" };

//            var result = await _controller.AssignRole(model);

//            Assert.That(result, Is.InstanceOf<NotFoundResult>());
//        }
//    }
//}
