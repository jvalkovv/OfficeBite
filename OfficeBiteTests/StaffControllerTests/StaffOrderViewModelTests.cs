using NUnit.Framework;
using OfficeBite.Core.Models.DishModels;
using OfficeBite.Core.Models.MenuModels;
using OfficeBite.Core.Models.StaffModels;

namespace OfficeBiteTests.StaffControllerTests
{
    [TestFixture]
    public class StaffOrderViewModelTests
    {
        [Test]
        public void StaffOrderViewModel_PropertiesInitializedCorrectly()
        {
            // Arrange
            var id = 1;
            var firstName = "John";
            var lastName = "Doe";
            var customerIdentifier = "123456";
            var customerUsername = "johndoe";
            var menuName = "Test Menu";
            var requestMenuNumber = 2;
            var menuOrders = new List<MenuViewModel> { new MenuViewModel
            {
                DishId = 1,
            }, new MenuViewModel
                {
                    DishId = 2,
                }
            };
            var menuItems = new List<DishViewModel> { new DishViewModel { DishId = 1, DishName = "Dish 1" }, new DishViewModel { DishId = 2, DishName = "Dish 2" } };
            var totalSum = 25.50m;
            var selectedDate = DateTime.Today;
            var details = "Test details";
            var comments = "Test comments";

            // Act
            var viewModel = new StaffOrderViewModel
            {
                Id = id,
                FirstName = firstName,
                LastName = lastName,
                CustomerIdentifier = customerIdentifier,
                CustomerUsername = customerUsername,
                MenuName = menuName,
                RequestMenuNumber = requestMenuNumber,
                MenuOrders = menuOrders,
                MenuItems = menuItems,
                TotalSum = totalSum,
                SelectedDate = selectedDate,
                Details = details,
                Comments = comments
            };

            // Assert
            Assert.That(viewModel.Id, Is.EqualTo(1));
            Assert.That(viewModel.FirstName, Is.EqualTo(firstName));
            Assert.That(viewModel.LastName, Is.EqualTo(lastName));
            Assert.That(viewModel.CustomerIdentifier, Is.EqualTo(customerIdentifier));
            Assert.That(viewModel.CustomerUsername, Is.EqualTo(customerUsername));
            Assert.That(viewModel.MenuName, Is.EqualTo(menuName));
            Assert.That(viewModel.RequestMenuNumber, Is.EqualTo(requestMenuNumber));
            Assert.That(viewModel.MenuOrders.Count(), Is.EqualTo(menuOrders.Count()));
            Assert.That(viewModel.MenuItems.Count(), Is.EqualTo(menuItems.Count()));
            Assert.That(viewModel.TotalSum, Is.EqualTo(totalSum));
            Assert.That(viewModel.SelectedDate, Is.EqualTo(selectedDate));
            Assert.That(viewModel.Details, Is.EqualTo(details));
            Assert.That(viewModel.Comments, Is.EqualTo(comments));

        }
    }
}
