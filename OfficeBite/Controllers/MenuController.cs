using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeBite.Core.Models.DishModels;
using OfficeBite.Core.Models.MenuModels;
using OfficeBite.Extensions.Interfaces;
using OfficeBite.Infrastructure.Data;
using OfficeBite.Infrastructure.Data.Models;
using OfficeBite.Infrastructure.Extensions.InterfaceForTest;
using System.Security.Claims;


namespace OfficeBite.Controllers
{
    [Authorize(Roles = "Admin, Manager, Staff, Employee")]
    public class MenuController : Controller
    {
        private readonly OfficeBiteDbContext dbContext;
        private readonly IHelperMethods helperMethods;
        private IDateTimeNowWrapper _dateTimeWrapper;
        public void SetDateTimeWrapper(IDateTimeNowWrapper dateTimeWrapper)
        {
            _dateTimeWrapper = dateTimeWrapper;
        }
        public DateTime GetCurrentDateTime()
        {
            return _dateTimeWrapper?.Now ?? DateTime.Now;
        }
        public MenuController(OfficeBiteDbContext _dbContext, IHelperMethods _helperMethods)
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
            var model = new MenuDailyViewModel();

            model.Dishes = await helperMethods.GetDishesAsync();
            model.Categories = await helperMethods.GetCategoryAsync();
            model.MenuTypes = await helperMethods.GetMenuTypesAsync();


            return View(model);
        }

        //TODO... Trow exception if is not time for order ? 
        [HttpPost]
        public async Task<IActionResult> MenuDailyList(MenuDailyViewModel model)
        {
            var selectedDate = model.SelectedDate;
            var currDateTime = GetCurrentDateTime();

            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid");
            }

            if (currDateTime.Date == selectedDate.Date && currDateTime.Hour < 11 ||
                currDateTime < selectedDate)
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
                        MenuName = p.MenuName

                    })
                    .ToListAsync();


                var viewModel = new MenuDailyViewModel
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


            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddDishToMenu(AddDishToMenuViewModel model)
        {

            var selectedDatesStrings = model.SelectedDates;

            if (selectedDatesStrings == null)
            {
                model.AllDishes = await helperMethods.GetDishForMenuAsync();
                model.AllMenuTypes = await helperMethods.GetMenuTypesAsync();
                return View(model);
            }
            if (ModelState.IsValid)
            {

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

                            await dbContext.MenuOrders.AddAsync(menuOrder);
                            await dbContext.SaveChangesAsync();
                            totalPrice = 0;
                        }

                    }


                    return RedirectToAction("AllDishes", "Dish");
                }

            }

            model.AllDishes = await helperMethods.GetDishForMenuAsync();
            model.AllMenuTypes = await helperMethods.GetMenuTypesAsync();

            return View(model);
        }


    }
}
