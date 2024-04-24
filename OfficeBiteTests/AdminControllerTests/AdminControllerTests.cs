using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework.Legacy;
using OfficeBite.Controllers;
using OfficeBite.Core.Models.AdminModels;
using OfficeBite.Core.Services.Contracts;
using OfficeBite.Infrastructure.Data.Common;

namespace OfficeBiteTests.AdminControllerTests
{
    [TestFixture]
    public class AdminControllerTests
    {
        private Mock<IAdminService> adminServiceMock;
        private AdminController controller;
        private Mock<IUserStore<IdentityUser>> userStoreMock;
        private Mock<IRoleStore<IdentityRole>> roleStoreMock;
        private Mock<IRepository> repositoryMock;
        private Mock<UserManager<IdentityUser>> userManagerMock;
        private Mock<RoleManager<IdentityRole>> roleManagerMock;


        [TearDown]
        public void TearDown()
        {
            adminServiceMock.Reset();
            controller.Dispose();
            userStoreMock.Reset();
            roleStoreMock.Reset();
            repositoryMock.Reset();
        }
        [SetUp]
        public void Setup()
        {
            adminServiceMock = new Mock<IAdminService>();
            controller = new AdminController(adminServiceMock.Object);
            userStoreMock = new Mock<IUserStore<IdentityUser>>();
            roleStoreMock = new Mock<IRoleStore<IdentityRole>>();
            repositoryMock = new Mock<IRepository>();
            userManagerMock = new Mock<UserManager<IdentityUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
            roleManagerMock = new Mock<RoleManager<IdentityRole>>(roleStoreMock.Object, null, null, null, null);
        }

        [Test]
        public async Task Admin_ReturnsViewWithModel()
        {
            var expectedModel = new AdminPanelViewModel();
            adminServiceMock.Setup(s => s.Admin()).ReturnsAsync(expectedModel);


            var result = await controller.Admin() as ViewResult;

            ClassicAssert.IsNotNull(result);
            ClassicAssert.AreEqual(expectedModel, result.Model);
        }

        [Test]
        public async Task GetRoleAsync_ReturnsListOfRoles()
        {
            var expectedRoles = new List<RoleViewModel>();
            adminServiceMock.Setup(s => s.GetUserRoles()).ReturnsAsync(expectedRoles);


            var result = await controller.GetRoleAsync();


            ClassicAssert.IsNotNull(result);
            ClassicAssert.AreEqual(expectedRoles, result);
        }
        [Test]
        public async Task GetUsersAsync_ReturnsListOfUsers()
        {

            var expectedUsers = new List<UsersViewModel>
            {
                new UsersViewModel { UserId = "1", UserName = "user1", FullName = "User One", Email = "user1@example.com", RoleName = "Role1" },
                new UsersViewModel { UserId = "2", UserName = "user2", FullName = "User Two", Email = "user2@example.com", RoleName = "Role2" }

            };
            adminServiceMock.Setup(s => s.GetUsers()).ReturnsAsync(expectedUsers);


            var result = await controller.GetUsersAsync();


            ClassicAssert.IsNotNull(result);
            ClassicAssert.AreEqual(expectedUsers.Count, result.Count);

        }

        [Test]
        public async Task AssignRole_ValidModel_SuccessfulAssignment()
        {
            var model = new AdminPanelViewModel
            {
                UserId = "1",
                RoleId = "1"
            };

            var adminServiceMock = new Mock<IAdminService>();
            adminServiceMock.Setup(s =>
                s.AssignRole(model)).Returns(Task.CompletedTask);

            controller = new AdminController(adminServiceMock.Object);

            var result = await controller.AssignRole(model);

            adminServiceMock.Verify(s =>
                s.AssignRole(model), Times.Once);
        }

        //[Test]
        //public async Task AssignRole_UserNotFound_NoAssignment()
        //{
        //    // Arrange
        //    var model = new AdminPanelViewModel { UserId = "nonexistent-user-id", RoleId = "1" };
        //    userManagerMock.Setup(m => 
        //        m.FindByIdAsync("nonexistent-user-id")).
        //        ReturnsAsync((IdentityUser)null);
        //    roleManagerMock.Setup(m => 
        //        m.FindByIdAsync("1"))
        //        .ReturnsAsync((IdentityRole)null);

        //    // Act
        //    var adminService = new AdminService(userManagerMock.Object, roleManagerMock.Object, repositoryMock.Object);
        //    await adminService.AssignRole(model);

        //    // Assert
        //    userManagerMock.Verify(m => m.FindByIdAsync("nonexistent-user-id"), Times.Once);
        //    roleManagerMock.Verify(m => m.FindByIdAsync("1"), Times.Once);
        //}

        //[Test]
        //public async Task AssignRole_RoleNotFound_NoAssignment()
        //{
        //    // Arrange
        //    var model = new AdminPanelViewModel { UserId = "1", RoleId = "nonexistent-role-id" };
        //    var user = new IdentityUser();
        //    userManagerMock.Setup(m => m.FindByIdAsync("1")).ReturnsAsync(user);
        //    roleManagerMock.Setup(m => m.FindByIdAsync("nonexistent-role-id")).ReturnsAsync((IdentityRole)null);

        //    // Act
        //    await adminServiceMock.Object.AssignRole(model);

        //    // Assert
        //    userManagerMock.Verify(m => m.FindByIdAsync("1"), Times.Once);
        //    roleManagerMock.Verify(m => m.FindByIdAsync("nonexistent-role-id"), Times.Once);
        //    userManagerMock.VerifyNoOtherCalls();
        //    roleManagerMock.VerifyNoOtherCalls();
        //}


        //[Test]
        //public async Task DeleteRole_ValidModel_SuccessfulDeletion()
        //{
        //    var model = new AdminPanelViewModel
        //    {

        //    };


        //    var result = await controller.DeleteRole(model) 
        //        as NotImplementedResult;

        //    ClassicAssert.IsNotNull(result);

        //}

    }
}