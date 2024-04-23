using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeBite.Core.Models.DishModels;
using OfficeBite.Core.Services.Contracts;

namespace OfficeBite.Controllers
{
    [Authorize(Roles = "Admin, Manager, Staff")]
    public class DishController : Controller
    {
        private readonly IDishService dishService;

        public DishController(IDishService _dishService)
        {
            dishService = _dishService;
        }

        [HttpGet]
        public async Task<IActionResult> AllDishes()
        {
            var model = await dishService.GetAllDishes();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AllHiddenDishes()
        {
            var model = await dishService.GetAllHiddenDishes();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> HideDish(int dishId)
        {
            var dishToHide = await dishService.HideDish(dishId);
            return View(dishToHide);
        }

        [HttpPost]
        public async Task<IActionResult> HideDishConfirm(int dishId)
        {
            if (dishId == null)
            {
                return NotFound();
            }

            await dishService.HideDishConfirm(dishId);

            return RedirectToAction(nameof(AllDishes));
        }

        [HttpGet]
        public async Task<IActionResult> UnHideDish(int dishId)
        {
            var dishToUnHide = await dishService.UnHideDish(dishId);
            return View(dishToUnHide);
        }

        [HttpPost]
        public async Task<IActionResult> UnHideDishConfirm(int dishId)
        {
            await dishService.UnHideDishConfirm(dishId);
            return RedirectToAction(nameof(AllDishes));
        }


        [HttpGet]
        public async Task<IActionResult> EditDish(int dishId)
        {
            var model = await dishService.EditDish(dishId);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditDish(AllDishesViewModel model, int dishId)
        {
            var result = await dishService.EditDish(model, dishId);

            if (result.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile",
                    "Моля, качете изображение във формат .jpg, .jpeg или .png");
                return View(model);

            }
            return RedirectToAction(nameof(AllDishes));
        }

        [HttpGet]
        public async Task<IActionResult> AddDish()
        {
            var model = await dishService.AddDish();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddDish(AllDishesViewModel model)
        {
            var result = await dishService.AddDish(model);

            if (result.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile",
                    "Моля, качете изображение във формат .jpg, .jpeg или .png");
                return View(model);

            }
            return RedirectToAction(nameof(AllDishes));
        }


        [HttpGet]
        public async Task<IActionResult> DeleteDish(int dishId)
        {
            var dishToDelete = await dishService.DeleteDish(dishId);
            return View(dishToDelete);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteDishConfirm(int dishId)
        {
            await dishService.DeleteDishConfirm(dishId);
            return RedirectToAction(nameof(AllHiddenDishes));
        }
    }
}
