using Microsoft.EntityFrameworkCore;
using OfficeBite.Core.Models.DishModels;
using OfficeBite.Core.Models.StaffModels;
using OfficeBite.Core.Services.Contracts;
using OfficeBite.Infrastructure.Data.Common;
using OfficeBite.Infrastructure.Data.Models;

namespace OfficeBite.Core.Services
{
    public class StaffService : IStaffService
    {
        private readonly IRepository repository;

        public StaffService(IRepository _repository)
        {
            repository = _repository;
        }

        public async Task<List<StaffAllOrdersViewModel>> AllOrders()
        {

            var model = await repository.AllReadOnly<Order>()
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
                    IsEaten = o.IsEaten
                })
                .ToListAsync();

            var priceInOrder = await repository.AllReadOnly<MenuOrder>()
                .Select(p => new StaffAllOrdersViewModel()
                {
                    MenuToOrderId = p.RequestMenuNumber,
                    TotalPrice = p.TotalPrice,
                })
                .ToListAsync();

            var groupedOrders = model.GroupBy(o => new
            { o.CustomerIdentifier, o.MenuToOrderId, o.LunchDate });

            var groupedModel = new List<StaffAllOrdersViewModel>();

            foreach (var group in groupedOrders)
            {
                var groupModel = group.FirstOrDefault();
                var totalPriceModel = priceInOrder.FirstOrDefault(p => groupModel != null && p.MenuToOrderId == groupModel.MenuToOrderId);

                if (groupModel != null)
                {
                    if (totalPriceModel != null)
                    {
                        groupModel.TotalPrice = totalPriceModel.TotalPrice;
                    }
                    groupedModel.Add(groupModel);
                }
            }

            return groupedModel.OrderBy(l => l.LunchDate).ToList();
        }

        public async Task<StaffOrderViewModel?> OrderView(int id, string userId, DateTime date)
        {

            var orders = await repository.AllReadOnly<Order>()
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
                    RequestMenuNumber = ov.MenuOrderRequestNumber,
                    MenuItems = ov.MenuOrder.DishesInMenus
                        .Select(d => new DishViewModel
                        {
                            DishName = d.Dish.DishName,
                            DishPrice = d.Dish.Price,

                        })
                })
                .FirstOrDefaultAsync();

            return orders;
        }

        public async Task<StaffAllOrdersViewModel?> OrderComplete(DateTime selectedDate, string username, string userId,
            int orderId)
        {
            var orderToComplete = await repository.AllReadOnly<Order>()
                .Where(order => order.SelectedDate == selectedDate &&
                                order.UserAgent.Username == username &&
                                order.UserAgentId == userId)
                .AsNoTracking()
                .Select(o => new StaffAllOrdersViewModel
                {
                    OrderId = o.Id,
                    OrderName = o.Name,
                    CustomerFirstName = o.UserAgent.FirstName,
                    CustomerLastName = o.UserAgent.LastName,
                    CustomerUsername = o.UserAgent.Username,
                    CustomerIdentifier = o.UserAgent.UserId,
                    TotalPrice = o.MenuOrder.TotalPrice,
                    LunchDate = o.SelectedDate,
                    DateOrderCreated = o.OrderPlacedOnDate,
                    IsEaten = o.IsEaten,
                    MenuToOrderId = o.MenuOrderRequestNumber
                })
                .FirstOrDefaultAsync();

            return orderToComplete;
        }

        public async Task CompleteOrderConfirm(DateTime selectedDate, int orderId, string userId)
        {
            var orderToCompleteConfirm = await repository.All<Order>().Where(o =>
                o.SelectedDate == selectedDate && o.MenuOrderRequestNumber == orderId && o.UserAgentId == userId)
                .ToListAsync();

            foreach (var lineOrder in orderToCompleteConfirm)
            {
                lineOrder.IsEaten = true;
            }

            await repository.SaveChangesAsync();
        }
    }
}
