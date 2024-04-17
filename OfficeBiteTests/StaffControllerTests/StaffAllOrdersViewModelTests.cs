using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            ClassicAssert.AreEqual(orderId, viewModel.OrderId);
            ClassicAssert.AreEqual(orderName, viewModel.OrderName);
            ClassicAssert.AreEqual(menuToOrderId, viewModel.MenuToOrderId);
            ClassicAssert.AreEqual(customerIdentifier, viewModel.CustomerIdentifier);
            ClassicAssert.AreEqual(customerFirstName, viewModel.CustomerFirstName);
            ClassicAssert.AreEqual(customerLastName, viewModel.CustomerLastName);
            ClassicAssert.AreEqual(customerUsername, viewModel.CustomerUsername);
            ClassicAssert.AreEqual(totalPrice, viewModel.TotalPrice);
            ClassicAssert.AreEqual(lunchDate, viewModel.LunchDate);
            ClassicAssert.AreEqual(dateOrderCreated, viewModel.DateOrderCreated);
            ClassicAssert.AreEqual(isEaten, viewModel.IsEaten);
        }
    }
}

