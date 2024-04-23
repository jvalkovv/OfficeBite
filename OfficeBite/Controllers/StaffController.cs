using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeBite.Core.Services.Contracts;

namespace OfficeBite.Controllers
{
    [Authorize(Roles = "Staff")]
    public class StaffController : Controller
    {
        private readonly IStaffService staffService;

        public StaffController(IStaffService _staffService)
        {
            this.staffService = _staffService;
        }

        [HttpGet]
        public async Task<IActionResult> AllOrders()
        {
            var orders = await staffService.AllOrders();

            return View(orders);
        }

        [HttpPost]
        public async Task<IActionResult> OrderView(int id, string userId, DateTime date)
        {
            var model = await staffService.OrderView(id, userId, date);

            return View(model);
        }

        //TODO... eventually to implement functionality that staff to can delete order .
        //var orderToComplete = await dbContext.Orders
        //        .Where(order => order.SelectedDate == selectedDate && order.UserAgent.Username == username && order.UserAgentId == userId)
        //        .AsNoTracking()
        //        .Select(o => new StaffAllOrdersViewModel
        //        {
        //            OrderId = o.Id,
        //            OrderName = o.Name,
        //            CustomerFirstName = o.UserAgent.FirstName,
        //            CustomerLastName = o.UserAgent.LastName,
        //            CustomerUsername = o.UserAgent.Username,
        //            TotalPrice = o.MenuOrder.TotalPrice,
        //            LunchDate = o.SelectedDate,
        //            DateOrderCreated = o.OrderPlacedOnDate,
        //            IsEaten = o.IsEaten
        //        })
        //        .ToListAsync();


        //    return View(orderToComplete);

        [HttpPost]
        public async Task<IActionResult> OrderComplete(DateTime selectedDate, string username, string userId, int orderId)
        {
            var orderToComplete = await staffService.OrderComplete(selectedDate, username, userId, orderId);

            return View(orderToComplete);
        }

        [HttpPost]
        public async Task<IActionResult> CompleteOrderConfirm(DateTime selectedDate, int orderId, string userId)
        {
            await staffService.CompleteOrderConfirm(selectedDate, orderId, userId);

            return RedirectToAction(nameof(AllOrders));
        }
    }

}
