using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeBite.Core.Services.Contracts;

namespace OfficeBite.Controllers
{
    [Authorize(Roles = "Admin, Manager, Staff, Employee")]
    public class OrderController : Controller
    {
        private readonly IOrderService orderService;

        public OrderController(IOrderService _orderService)
        {
            orderService = _orderService;
        }

        [HttpGet]

        public async Task<IActionResult> AddToOrder()
        {
            var model = await orderService.AddToOrder();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddToOrder(int requestMenuNumber)
        {
            try
            {
                await orderService.AddToOrder(requestMenuNumber);
                return RedirectToAction("MenuDailyList", "Menu");
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message == "Invalid date")
                {
                    TempData["OrderExistsError"] = "Потребителят вече има поръчка за тази дата.";
                }
                else if (ex.Message == "Invalid order")
                {
                    return RedirectToPage("/Areas/Identity/Pages/Account/AccessDenied");
                }
            }

            return RedirectToAction("MenuDailyList", "Menu");
        }
    }
}
