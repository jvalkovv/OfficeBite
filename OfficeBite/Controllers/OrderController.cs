using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeBite.Core.Models.OrderModels;
using OfficeBite.Extensions.Interfaces;
using OfficeBite.Infrastructure.Data;
using OfficeBite.Infrastructure.Data.Models;
using System.Security.Claims;

namespace OfficeBite.Controllers
{
    [Authorize(Roles = "Admin, Manager, Staff, Employee")]
    public class OrderController : Controller
    {
        private readonly OfficeBiteDbContext dbContext;
        private readonly IHelperMethods helperMethods;

        public OrderController(OfficeBiteDbContext _dbContext, IHelperMethods _helperMethods)
        {
            this.dbContext = _dbContext;
            this.helperMethods = _helperMethods;
        }
        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        }


        [HttpGet]

        public async Task<IActionResult> AddToOrder()
        {
            var model = new AddOrderViewModel();
            model.AllDishes = await helperMethods.GetDishForMenuAsync();
            model.AllMenuTypes = await helperMethods.GetMenuTypesAsync();


            return View(model);

        }

        // TODO... Redirect when it is successful 
        [HttpPost]
        public async Task<IActionResult> AddToOrder(int requestMenuNumber)
        {
            var currOrder = await dbContext.DishesInMenus
                .Where(i => i.MenuOrder.RequestMenuNumber == requestMenuNumber)
                .Include(m => m.MenuOrder)
                .Include(d => d.Dish)
                .Include(dishMenuToOrder => dishMenuToOrder.MenuOrder.MenuType)
                .ToListAsync();

            var dishMenu = await dbContext.DishesInMenus.Where(d => d.RequestMenuNumber == requestMenuNumber)
                .FirstOrDefaultAsync();

            var orderDates = await dbContext.Orders.ToListAsync();

            if (dishMenu != null)
            {
                var selectedDate = dishMenu.MenuOrder.SelectedMenuDate.Date;

                var currUser = GetUserId();

                var isUserExist = await dbContext.UserAgents
                    .FirstOrDefaultAsync(u => u.UserId == currUser);

                if (ModelState.IsValid)
                {
                    if (isUserExist != null)
                    {
                        var requestForDate = await dbContext.Orders
                            .AnyAsync(o => o.UserAgentId == currUser && o.SelectedDate == selectedDate);


                        if (requestForDate)
                        {
                            TempData["OrderExistsError"] = "Потребителят вече има поръчка за тази дата.";
                        }
                        else
                        {
                            foreach (var dish in currOrder)
                            {
                                var orderEntity = new Order
                                {
                                    Name = $"{dish.MenuOrder.MenuType.Name} Номер на менюто - {dish.RequestMenuNumber}",
                                    SelectedDate = dish.MenuOrder.SelectedMenuDate,
                                    OrderPlacedOnDate = DateTime.Now,
                                    IsEaten = false,
                                    Details = dish.Dish.Description,
                                    UserAgentId = currUser,
                                    MenuOrderRequestNumber = dish.RequestMenuNumber

                                };

                                await dbContext.Orders.AddAsync(orderEntity);
                            }
                            await dbContext.SaveChangesAsync();


                            var orders = await dbContext.Orders
                                .Where(o => o.UserAgentId == currUser && o.SelectedDate == selectedDate)
                                .ToListAsync();
                            foreach (var order in orders)
                            {
                                var orderHistory = new OrderHistory
                                {
                                    UserAgentId = currUser,
                                    MenuOrderRequestNumber = order.MenuOrderRequestNumber,
                                    OrderId = order.Id
                                };
                                await dbContext.OrderHistories.AddAsync(orderHistory);
                            }

                            await dbContext.SaveChangesAsync();
                        }

                        return RedirectToAction("MenuDailyList", "Menu");
                    }
                    return RedirectToPage("/Areas/Identity/Pages/Account/AccessDenied");
                }
                return BadRequest("Model is not valid");
            }

            return RedirectToPage("/Areas/Identity/Pages/Account/AccessDenied");
        }

    }
}
