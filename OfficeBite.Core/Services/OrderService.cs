using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OfficeBite.Core.Extensions.Interfaces;
using OfficeBite.Core.Models.OrderModels;
using OfficeBite.Core.Services.Contracts;
using OfficeBite.Infrastructure.Data.Common;
using OfficeBite.Infrastructure.Data.Models;

namespace OfficeBite.Core.Services
{
    public class OrderService : IOrderService
    {
        private readonly IRepository repository;
        private readonly IHelperMethods helperMethods;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IHttpContextAccessor httpContextAccessor;

        public OrderService(IRepository _repository, IHelperMethods _helperMethods,
            UserManager<IdentityUser> _userManager, IHttpContextAccessor _httpContextAccessor)
        {
            repository = _repository;
            helperMethods = _helperMethods;
            userManager = _userManager;
            httpContextAccessor = _httpContextAccessor;
        }

        public async Task<AddOrderViewModel> AddToOrder()
        {
            var model = new AddOrderViewModel();
            model.AllDishes = await helperMethods.GetDishForMenuAsync();
            model.AllMenuTypes = await helperMethods.GetMenuTypesAsync();


            return model;
        }

        public async Task<IdentityUser> GetCurrentUserIdAsync()
        {
            var user = await userManager.GetUserAsync(httpContextAccessor.HttpContext?.User);
            return user;
        }

        public async Task AddToOrder(int requestMenuNumber)
        {
            var currOrder = await repository.All<DishesInMenu>()
                .Where(i => i.MenuOrder.RequestMenuNumber == requestMenuNumber)
                .Include(m => m.MenuOrder)
                .Include(d => d.Dish)
                .Include(dishMenuToOrder => dishMenuToOrder.MenuOrder.MenuType)
                .ToListAsync();

            if (currOrder == null || currOrder.Count <= 0)
            {
                throw new InvalidDataException("Invalid order");
            }

            var dishMenu = await repository.All<DishesInMenu>()
                .Where(d => d.RequestMenuNumber == requestMenuNumber)
                .FirstOrDefaultAsync();

            if (dishMenu != null)
            {
                var selectedDate = dishMenu.MenuOrder.SelectedMenuDate.Date;

                var currUser = GetCurrentUserIdAsync().Result;
                var userId = currUser.Id;

                var isUserExist = await repository.AllReadOnly<UserAgent>()
                    .FirstOrDefaultAsync(u => u.UserId == userId);


                if (isUserExist != null)
                {
                    var requestForDate = await repository.AllReadOnly<Order>()
                        .AnyAsync(o => o.UserAgentId == userId && o.SelectedDate == selectedDate);

                    if (requestForDate)
                    {
                        throw new InvalidOperationException("Invalid date");
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
                                UserAgentId = userId,
                                MenuOrderRequestNumber = dish.RequestMenuNumber
                            };

                            await repository.AddAsync<Order>(orderEntity);
                        }

                        await repository.SaveChangesAsync();


                        var orders = await repository.AllReadOnly<Order>()
                            .Where(o => o.UserAgentId == userId && o.SelectedDate == selectedDate)
                            .ToListAsync();

                        foreach (var order in orders)
                        {
                            var orderHistory = new OrderHistory
                            {
                                UserAgentId = userId,
                                MenuOrderRequestNumber = order.MenuOrderRequestNumber,
                                OrderId = order.Id
                            };
                            await repository.AddAsync<OrderHistory>(orderHistory);
                        }

                        await repository.SaveChangesAsync();
                    }
                }

            }


        }
    }
}

