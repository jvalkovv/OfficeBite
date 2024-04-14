using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeBite.Core.Models.DishModels;
using OfficeBite.Core.Models.StaffModels;
using OfficeBite.Infrastructure.Data;

namespace OfficeBite.Controllers
{
    [Authorize(Roles = "Staff")]
    public class StaffController : Controller
    {
        private readonly OfficeBiteDbContext dbContext;

        public StaffController(OfficeBiteDbContext _dbContext)
        {
            this.dbContext = _dbContext;
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
            var isAuthorized = User.IsInRole("Staff");
            if (!isAuthorized)
            {
                return Unauthorized();
            }
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
