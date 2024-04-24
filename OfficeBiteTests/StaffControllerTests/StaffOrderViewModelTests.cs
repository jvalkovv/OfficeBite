using NUnit.Framework.Legacy;
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
            ClassicAssert.AreEqual(id, viewModel.Id);
            ClassicAssert.AreEqual(firstName, viewModel.FirstName);
            ClassicAssert.AreEqual(lastName, viewModel.LastName);
            ClassicAssert.AreEqual(customerIdentifier, viewModel.CustomerIdentifier);
            ClassicAssert.AreEqual(customerUsername, viewModel.CustomerUsername);
            ClassicAssert.AreEqual(menuName, viewModel.MenuName);
            ClassicAssert.AreEqual(requestMenuNumber, viewModel.RequestMenuNumber);
            ClassicAssert.AreEqual(menuOrders.Count(), viewModel.MenuOrders.Count());
            ClassicAssert.AreEqual(menuItems.Count(), viewModel.MenuItems.Count());
            ClassicAssert.AreEqual(totalSum, viewModel.TotalSum);
            ClassicAssert.AreEqual(selectedDate, viewModel.SelectedDate);
            ClassicAssert.AreEqual(details, viewModel.Details);
            ClassicAssert.AreEqual(comments, viewModel.Comments);
        }
    }
}
