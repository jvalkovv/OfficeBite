using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeBite.Core.Models.DishModels;
using OfficeBite.Extensions;
using OfficeBite.Infrastructure.Data;
using System.Security.Claims;

namespace OfficeBite.Controllers
{
    [Authorize(Roles = "Admin, Manager, Staff")]
    public class DishController : Controller
    {
        private readonly HelperMethods helperMethods;
        private readonly OfficeBiteDbContext dbContext;

        public DishController(HelperMethods _helperMethods, OfficeBiteDbContext context)
        {
            helperMethods = _helperMethods;
            dbContext = context;
        }

        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        }

        [HttpGet]
        public async Task<IActionResult> AllDishes()
        {
            var userId = GetUserId();

            var userInDb = await dbContext.UserAgents
                .FirstOrDefaultAsync(ua => ua.UserId == userId);

            if (userInDb == null)
            {
                return RedirectToPage("/Areas/Identity/Pages/Account/AccessDenied");
            }

            var model = new AllDishesViewModel
            {
                Categories = await helperMethods.GetCategoryAsync(),
                Dishes = await helperMethods.GetDishesAsync()
            };


            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> HideDish(int dishId)
        {
            var dishToHide = await dbContext.Dishes
                .Where(d => d.Id == dishId)
                .AsNoTracking()
                .Select(d => new DishViewModel()
                {
                    DishId = d.Id,
                    DishName = d.DishName,
                    DishPrice = d.Price,
                    Description = d.Description,
                    ImageUrl = d.ImageUrl,
                    IsVisible = d.IsVisible
                })
                .FirstOrDefaultAsync();

            return View(dishToHide);
        }

        [HttpPost]
        public async Task<IActionResult> HideDishConfirm(int dishId)
        {
            var dishToHide = await dbContext.Dishes.FindAsync(dishId);

            if (dishToHide == null)
            {
                return NotFound();
            }

            dishToHide.IsVisible = false;
            var allDishInOrders = dbContext.DishesInMenus.Where(d => d.DishId == dishToHide.Id);
            foreach (var currDish in allDishInOrders)
            {
                currDish.IsVisible = false;
            }

            var menuOrders = await dbContext.MenuOrders
                .Where(m => dbContext.DishesInMenus
                    .Any(d => d.RequestMenuNumber == m.RequestMenuNumber && d.DishId == dishToHide.Id))
                .ToListAsync();


            foreach (var menuOrder in menuOrders)
            {
                menuOrder.IsVisible = false;
            }

            await dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(AllDishes));
        }

        [HttpGet]
        public async Task<IActionResult> AllHiddenDishes()
        {
            var userId = GetUserId();

            var userInDb = await dbContext.UserAgents
                .FirstOrDefaultAsync(ua => ua.UserId == userId);


            if (userInDb == null)
            {
                return RedirectToPage("/Areas/Identity/Pages/Account/AccessDenied");
            }


            var model = new AllDishesViewModel();
            model.Categories = await helperMethods.GetCategoryAsync();
            model.Dishes = helperMethods.GetDishesAsync().Result.Where(d => d.IsVisible == false);


            return View(model);
        }
    }
}
