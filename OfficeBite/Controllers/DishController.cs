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

        // Показва всички ястия в Административният панел
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
    }
}
