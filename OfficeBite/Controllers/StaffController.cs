using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeBite.Core.Models.DishModels;
using OfficeBite.Core.Models.MenuModels;
using OfficeBite.Core.Models.StaffModels;
using OfficeBite.Extensions;
using OfficeBite.Infrastructure.Data;
using OfficeBite.Infrastructure.Data.Models;

namespace OfficeBite.Controllers
{
    public class StaffController : Controller
    {
        private readonly OfficeBiteDbContext dbContext;
        private readonly HelperMethods helperMethods;

        public StaffController(OfficeBiteDbContext _dbContext, HelperMethods _helperMethods)
        {
            this.dbContext = _dbContext;
            this.helperMethods = _helperMethods;
        }

        [HttpGet]
        public async Task<IActionResult> AllOrders()
        {
            var model = await dbContext.Orders
                .Include(user => user.UserAgent)
                .AsNoTracking()
                .Select(o => new StaffAllOrdersViewModel
                {
                    OrderName = o.Name,
                    MenuToOrderId = o.MenuOrderRequestNumber,
                    CustomerIdentifier = o.UserAgentId,
                    CustomerUsername = o.UserAgent.Username,
                    CustomerFirstName = o.UserAgent.FirstName,
                    CustomerLastName = o.UserAgent.LastName,
                    LunchDate = o.SelectedDate,
                    DateOrderCreated = o.OrderPlacedOnDate,
                    IsEaten = false
                })
                .ToListAsync();

            var priceInOrder = await dbContext.MenuOrders
                .Select(p => new StaffAllOrdersViewModel()
                {
                    MenuToOrderId = p.RequestMenuNumber,
                    TotalPrice = p.TotalPrice,
                })
                .ToListAsync();


            var groupedOrders = model.GroupBy(o => new { o.CustomerIdentifier, o.MenuToOrderId, o.LunchDate });

            var groupedModel = new List<StaffAllOrdersViewModel>();

            foreach (var group in groupedOrders)
            {
                var groupModel = group.FirstOrDefault();
                var totalPriceModel = priceInOrder.FirstOrDefault(p => p.MenuToOrderId == groupModel.MenuToOrderId);

                if (groupModel != null)
                {
                    if (totalPriceModel != null)
                    {
                        groupModel.TotalPrice = totalPriceModel.TotalPrice;
                    }
                    groupedModel.Add(groupModel);
                }
            }

            return View(groupedModel.OrderBy(e => e.LunchDate).ToList());

        }

        [HttpPost]
        public async Task<IActionResult> OrderView(int id, string userId, DateTime date)
        {
            var orders = await dbContext.Orders
                .Include(order => order.UserAgent)
                .Include(order => order.MenuInOrder)
                .Include(order => order.MenuOrder)
                .Where(od => od.MenuOrderRequestNumber == id && od.UserAgentId == userId && od.SelectedDate == date)
                .Select(ov => new StaffOrderViewModel
                {
                    FirstName = ov.UserAgent.FirstName,
                    LastName = ov.UserAgent.LastName,
                    CustomerUsername = ov.UserAgent.Username,
                    SelectedDate = ov.SelectedDate,
                    TotalSum = ov.MenuOrder.TotalPrice,
                    CustomerIdentifier = ov.UserAgentId,
                    MenuItems = ov.MenuOrder.DishesInMenus
                        .Select(d => new DishViewModel
                        {
                            DishName = d.Dish.DishName,
                            DishPrice = d.Dish.Price,
                            
                        })
                })
                .FirstOrDefaultAsync();


            return View(orders);
        }
    }
}
