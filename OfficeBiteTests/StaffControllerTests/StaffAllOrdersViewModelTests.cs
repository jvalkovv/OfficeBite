using NUnit.Framework;
using NUnit.Framework.Legacy;
using OfficeBite.Core.Models.StaffModels;

namespace OfficeBiteTests.StaffControllerTests
{
    [TestFixture]
    public class StaffAllOrdersViewModelTests
    {
        [Test]
        public void StaffAllOrdersViewModel_PropertiesInitializedCorrectly()
        {
            // Arrange
            var orderId = 1;
            var orderName = "Test Order";
            var menuToOrderId = 2;
            var customerIdentifier = "123456";
            var customerFirstName = "John";
            var customerLastName = "Doe";
            var customerUsername = "johndoe";
            var totalPrice = 25.50m;
            var lunchDate = DateTime.Today;
            var dateOrderCreated = DateTime.Now;
            var isEaten = false;

            // Act
            var viewModel = new StaffAllOrdersViewModel
            {
                OrderId = orderId,
                OrderName = orderName,
                MenuToOrderId = menuToOrderId,
                CustomerIdentifier = customerIdentifier,
                CustomerFirstName = customerFirstName,
                CustomerLastName = customerLastName,
                CustomerUsername = customerUsername,
                TotalPrice = totalPrice,
                LunchDate = lunchDate,
                DateOrderCreated = dateOrderCreated,
                IsEaten = isEaten
            };

            // Assert
            Assert.That(viewModel.OrderId, Is.EqualTo(orderId));
            Assert.That(viewModel.OrderName, Is.EqualTo(orderName));
            Assert.That(viewModel.MenuToOrderId, Is.EqualTo(menuToOrderId));
            Assert.That(viewModel.CustomerIdentifier, Is.EqualTo(customerIdentifier));
            Assert.That(viewModel.CustomerFirstName, Is.EqualTo(customerFirstName));
            Assert.That(viewModel.CustomerLastName, Is.EqualTo(customerLastName));
            Assert.That(viewModel.CustomerUsername, Is.EqualTo(customerUsername));
            Assert.That(viewModel.TotalPrice, Is.EqualTo(totalPrice));
            Assert.That(viewModel.LunchDate, Is.EqualTo(lunchDate));
            Assert.That(viewModel.DateOrderCreated, Is.EqualTo(dateOrderCreated));
            Assert.That(viewModel.IsEaten, Is.EqualTo(isEaten));
        }
    }
}

