using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeBite.Extensions;
using OfficeBite.Infrastructure.Data;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using OfficeBite.Core.Models.DishModels;
using OfficeBite.Core.Models.MenuModels;
using OfficeBite.Infrastructure.Data.Models;

namespace OfficeBite.Controllers
{
    [Authorize(Roles = "Admin, Manager, Staff, Employee")]
    public class MenuController : Controller
    {
        private readonly OfficeBiteDbContext dbContext;
        private readonly HelperMethods helperMethods;

        public MenuController(OfficeBiteDbContext _dbContext, HelperMethods _helperMethods)
        {
            this.dbContext = _dbContext;
            this.helperMethods = _helperMethods;
        }
        private async Task<int> GeneratеRequestMenuNumberAsync()
        {
            var lastOrderMenuId = await dbContext.MenuOrders
                .OrderByDescending(m => m.RequestMenuNumber)
                .Select(m => m.RequestMenuNumber)
                .FirstOrDefaultAsync();


            lastOrderMenuId++;

            return lastOrderMenuId;
        }
        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> MenuList()
        {
            var model = new MenuViewModel();

            model.Dishes = await helperMethods.GetDishesAsync();
            model.Categories = await helperMethods.GetCategoryAsync();



            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> MenuDailyList()
        {
            if (TempData.ContainsKey("OrderExistsError"))
            {
                ViewData["OrderExistsError"] = TempData["OrderExistsError"];
            }
            var model = new DailyMenuViewModel();

            model.Dishes = await helperMethods.GetDishesAsync();
            model.Categories = await helperMethods.GetCategoryAsync();
            model.MenuTypes = await helperMethods.GetMenuTypesAsync();


            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> MenuDailyList(DailyMenuViewModel model)
        {
            var selectedDate = model.SelectedDate;
            var currDateTime = DateTime.Now;

            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid");
            }

            if (currDateTime.Hour < 11 || currDateTime < selectedDate)
            {
                var dishInOrder = await dbContext.DishesInMenus
                    .Include(m => m.MenuOrder)
                    .Include(t => t.MenuOrder.MenuType)
                    .Include(d => d.Dish)
                    .Where(d => d.MenuOrder.SelectedMenuDate == selectedDate && d.IsVisible == true)
                    .Join(
                        dbContext.Dishes,
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


                var priceInOrder = await dbContext.MenuOrders
                    .Select(p => new MenuForDateViewModel
                    {
                        RequestMenuNumber = p.RequestMenuNumber,
                        TotalPrice = p.TotalPrice,
                        Description = p.Description,
                    })
                    .ToListAsync();


                var viewModel = new DailyMenuViewModel
                {
                    SelectedDate = selectedDate,
                    GroupDishes = groupedDishes,
                    MenuForDateViewModels = priceInOrder
                };

                return View(viewModel);
            }
            if (currDateTime.Hour > 10 && currDateTime.Date == selectedDate.Date)
            {
                return Json("Менюто е АЛАМИНУТ");
            }

            return BadRequest("Invalid date for order");

        }


        [HttpGet]
        public async Task<IActionResult> AddDishToMenu()
        {
            var model = new AddDishToMenuViewModel();
            model.AllDishes = await helperMethods.GetDishForMenuAsync();
            model.AllMenuTypes = await helperMethods.GetMenuTypesAsync();

            model.RequestMenuNumber = await GeneratеRequestMenuNumberAsync();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddDishToMenu(AddDishToMenuViewModel model)
        {
            var orderMenuId = await GeneratеRequestMenuNumberAsync();
            var totalPrice = 0m;
            var description = string.Empty;
            var selectedDishesIds = model.SelectedDishes;

            if (ModelState.IsValid)
            {
                foreach (var date in model.SelectedDates)
                {
                    foreach (var dishId in selectedDishesIds)
                    {
                        var dish = await dbContext.Dishes.FindAsync(dishId);
                        var menuType = await dbContext.MenuTypes.FindAsync(model.MenuTypeId);

                        if (dish != null && menuType != null)
                        {
                            var dishMenuToOrder = new DishesInMenu
                            {
                                IsVisible = true,
                                DishId = dish.Id,
                                RequestMenuNumber = orderMenuId
                            };

                            await dbContext.DishesInMenus.AddAsync(dishMenuToOrder);

                            description += $" '{dish.Description}'; ";
                            totalPrice += dish.Price;
                        }

                    }

                }

                var selectedDates = model.SelectedDates;
                foreach (var date in selectedDates)
                {
                    var menuOrder = new MenuOrder
                    {
                        RequestMenuNumber = orderMenuId,
                        SelectedMenuDate = date,
                        TotalPrice = totalPrice,
                        Description = description,
                        IsVisible = true,
                        MenuTypeId = model.MenuTypeId
                    };
                    await dbContext.MenuOrders.AddAsync(menuOrder);

                }

                await dbContext.SaveChangesAsync();

                model.AllDishes = await helperMethods.GetDishForMenuAsync();
                model.AllMenuTypes = await helperMethods.GetMenuTypesAsync();

                return View(model);
            }

            model.AllDishes = await helperMethods.GetDishForMenuAsync();
            model.AllMenuTypes = await helperMethods.GetMenuTypesAsync();

            return View(model);
        }

    }
}
