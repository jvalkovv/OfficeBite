using Microsoft.EntityFrameworkCore;
using OfficeBite.Core.Extensions.Interfaces;
using OfficeBite.Core.Models.DishModels;
using OfficeBite.Core.Models.MenuModels;
using OfficeBite.Core.Services.Contracts;
using OfficeBite.Infrastructure.Data.Common;
using OfficeBite.Infrastructure.Data.Models;
using OfficeBite.Infrastructure.Extensions.InterfaceForTest;

namespace OfficeBite.Core.Services
{
    public class MenuService : IMenuService
    {
        private const int AlaminutMenuTypeId = 4;
        private readonly IHelperMethods helperMethods;
        private IDateTimeNowWrapper _dateTimeWrapper;
        private readonly IRepository repository;

        public MenuService(IRepository _repository, IHelperMethods helperMethods)
        {
            repository = _repository;
            this.helperMethods = helperMethods;
        }
        public void SetDateTimeWrapper(IDateTimeNowWrapper dateTimeWrapper)
        {
            _dateTimeWrapper = dateTimeWrapper;
        }
        public DateTime GetCurrentDateTime()
        {
            return _dateTimeWrapper?.Now ?? DateTime.Now;
        }
        private async Task<int> GeneratеRequestMenuNumberAsync()
        {
            var lastOrderMenuId = await repository.AllReadOnly<MenuOrder>()
                .OrderByDescending(m => m.RequestMenuNumber)
                .Select(m => m.RequestMenuNumber)
                .FirstOrDefaultAsync();


            lastOrderMenuId++;

            return lastOrderMenuId;
        }
        public async Task<MenuViewModel> GetMenuListAsync()
        {
            var model = new MenuViewModel();

            model.Dishes = await helperMethods.GetDishesAsync();
            model.Categories = await helperMethods.GetCategoryAsync();

            return model;
        }

        public async Task<MenuDailyViewModel> GetMenuDailyListAsync()
        {

            var model = new MenuDailyViewModel();

            model.Dishes = await helperMethods.GetDishesAsync();
            model.Categories = await helperMethods.GetCategoryAsync();
            model.MenuTypes = await helperMethods.GetMenuTypesAsync();

            return model;
        }

        public async Task<MenuDailyViewModel> MenuDailyList(MenuDailyViewModel model)
        {
            var selectedDate = model.SelectedDate;
            var currDateTime = GetCurrentDateTime();

            if (currDateTime.Date == selectedDate.Date && currDateTime.Hour < 11 ||
                currDateTime < selectedDate)
            {
                var dishInOrder = await repository.AllReadOnly<DishesInMenu>()
                    .Include(m => m.MenuOrder)
                    .Include(t => t.MenuOrder.MenuType)
                    .Include(d => d.Dish)
                    .Where(d => d.MenuOrder.SelectedMenuDate == selectedDate && d.IsVisible == true)
                    .Join(
                        repository.AllReadOnly<Dish>(),
                        dishToOrder => dishToOrder.DishId,
                        dish => dish.Id,
                        (dishToOrder, dish) => new DishViewModel
                        {
                            DishName = dish.DishName,
                            DishPrice = dish.Price,
                            Description = dish.Description,
                            ImageUrl = dish.ImageUrl,
                            MenuTypeId = dishToOrder.MenuOrder.MenuTypeId,
                            RequestMenuNumber = dishToOrder.RequestMenuNumber
                        })
                    .ToListAsync();



                var groupedDishes = dishInOrder.GroupBy(d => d.RequestMenuNumber);


                var priceInOrder = await repository.AllReadOnly<MenuOrder>()
                    .Select(p => new MenuForDateViewModel
                    {
                        RequestMenuNumber = p.RequestMenuNumber,
                        TotalPrice = p.TotalPrice,
                        Description = p.Description,
                        MenuName = p.MenuName

                    })
                    .ToListAsync();


                var viewModel = new MenuDailyViewModel
                {
                    SelectedDate = selectedDate,
                    GroupDishes = groupedDishes,
                    MenuForDateViewModels = priceInOrder

                };

                return viewModel;
            }
            if (currDateTime.Hour > 10 && currDateTime.Date == selectedDate.Date)
            {

                var dishInOrder = await repository.AllReadOnly<DishesInMenu>()
                    .Include(m => m.MenuOrder)
                    .Include(t => t.MenuOrder.MenuType)
                    .Include(d => d.Dish)
                    .Where(d => d.MenuOrder.SelectedMenuDate == selectedDate &&
                                d.IsVisible == true && d.MenuOrder.MenuTypeId == AlaminutMenuTypeId)
                    .Join(
                        repository.AllReadOnly<Dish>(),
                        dishToOrder => dishToOrder.DishId,
                        dish => dish.Id,
                        (dishToOrder, dish) => new DishViewModel
                        {
                            DishName = dish.DishName,
                            DishPrice = dish.Price,
                            Description = dish.Description,
                            ImageUrl = dish.ImageUrl,
                            MenuTypeId = dishToOrder.MenuOrder.MenuTypeId,
                            RequestMenuNumber = dishToOrder.RequestMenuNumber
                        })
                    .ToListAsync();


                var groupedDishes = dishInOrder.GroupBy(d => d.RequestMenuNumber);


                var priceInOrder = await repository.AllReadOnly<MenuOrder>()
                    .Select(p => new MenuForDateViewModel
                    {
                        RequestMenuNumber = p.RequestMenuNumber,
                        TotalPrice = p.TotalPrice,
                        Description = p.Description,
                        MenuName = p.MenuName

                    })
                    .ToListAsync();


                var viewModel = new MenuDailyViewModel
                {
                    SelectedDate = selectedDate,
                    GroupDishes = groupedDishes,
                    MenuForDateViewModels = priceInOrder

                };

                return viewModel;
            }

            return model;
        }

        public async Task<AddDishToMenuViewModel> GetMenuAddDishToMenuListAsync()
        {
            var model = new AddDishToMenuViewModel();
            model.AllDishes = await helperMethods.GetDishForMenuAsync();
            model.AllMenuTypes = await helperMethods.GetMenuTypesAsync();


            return model;
        }

        public async Task<AddDishToMenuViewModel> AddDishToMenuListAsync(AddDishToMenuViewModel model)
        {
            var selectedDatesStrings = model.SelectedDates;

            if (selectedDatesStrings == null)
            {
                model.AllDishes = await helperMethods.GetDishForMenuAsync();
                model.AllMenuTypes = await helperMethods.GetMenuTypesAsync();
                return model;
            }

            if (selectedDatesStrings.Any(date => !string.IsNullOrWhiteSpace(date)))
            {
                var totalPrice = 0m;


                foreach (var dateStr in model.SelectedDates)
                {
                    var dates = dateStr.Split(',').Select(DateTime.Parse);

                    foreach (var date in dates)
                    {
                        var orderMenuId = await GeneratеRequestMenuNumberAsync();


                        foreach (var dishId in model.SelectedDishes)
                        {
                            var dish = await repository.GetByIdAsync<Dish>(dishId);
                            var menuType = await repository.GetByIdAsync<MenuType>(model.MenuTypeId);

                            if (dish != null && menuType != null)
                            {
                                var dishMenuToOrder = new DishesInMenu
                                {
                                    IsVisible = true,
                                    DishId = dish.Id,
                                    RequestMenuNumber = orderMenuId
                                };

                                await repository.AddAsync(dishMenuToOrder);


                                totalPrice += dish.Price;
                            }
                        }
                        var menuOrder = new MenuOrder
                        {
                            RequestMenuNumber = orderMenuId,
                            MenuName = model.MenuName,
                            Description = model.Description,
                            TotalPrice = totalPrice,
                            IsVisible = true,
                            MenuTypeId = model.MenuTypeId,
                            SelectedMenuDate = date
                        };

                        await repository.AddAsync(menuOrder);
                        await repository.SaveChangesAsync();
                        totalPrice = 0;
                    }

                }

            }

            model.AllDishes = await helperMethods.GetDishForMenuAsync();
            model.AllMenuTypes = await helperMethods.GetMenuTypesAsync();

            return model;
        }

    }
}

