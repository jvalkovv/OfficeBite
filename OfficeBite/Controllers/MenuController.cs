using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeBite.Core.Models.MenuModels;
using OfficeBite.Core.Services.Contracts;


namespace OfficeBite.Controllers
{
    [Authorize(Roles = "Admin, Manager, Staff, Employee")]
    public class MenuController : Controller
    {
        private readonly IMenuService menuService;
        public MenuController(IMenuService menuService)
        {
            this.menuService = menuService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> MenuList()
        {
            var model = await menuService.GetMenuListAsync();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> MenuDailyList()
        {
            if (TempData.ContainsKey("OrderExistsError"))
            {
                ViewData["OrderExistsError"] = TempData["OrderExistsError"];
            }

            var model = await menuService.GetMenuDailyListAsync();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> MenuDailyList(MenuDailyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid");
            }

            var result = await menuService.MenuDailyList(model);

            return View(result);
        }


        [Authorize(Roles = "Staff")]
        [HttpGet]
        public async Task<IActionResult> AddDishToMenu()
        {
            var model = await menuService.GetMenuAddDishToMenuListAsync();
            return View(model);
        }


        [Authorize(Roles = "Staff")]
        [HttpPost]
        public async Task<IActionResult> AddDishToMenu(AddDishToMenuViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await menuService.AddDishToMenuListAsync(model);
                return View(result);
            }

            var notValid = await menuService.GetMenuAddDishToMenuListAsync();

            return View(notValid);
        }
    }
}
