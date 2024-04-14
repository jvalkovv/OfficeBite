using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeBite.Core.Models.DishModels;
using OfficeBite.Extensions;
using OfficeBite.Infrastructure.Data;
using OfficeBite.Infrastructure.Data.Models;
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


        [HttpGet]
        public async Task<IActionResult> HideDish(int id)
        {
            var dishToHide = await dbContext.Dishes
                .Where(d => d.Id == id)
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
        public async Task<IActionResult> UnHideDish(int dishId)
        {

            var dishToUnHide = await dbContext.Dishes
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


            return View(dishToUnHide);
        }


        [HttpPost]
        public async Task<IActionResult> UnHideDishConfirm(int dishId)
        {
            var dishToHide = await dbContext.Dishes.FindAsync(dishId);

            if (dishToHide == null)
            {
                return NotFound();
            }

            dishToHide.IsVisible = true;
            var allDishInOrders = dbContext.DishesInMenus.Where(d => d.DishId == dishToHide.Id);
            foreach (var currDish in allDishInOrders)
            {
                currDish.IsVisible = true;
            }

            var menuOrders = await dbContext.MenuOrders
                .Where(m => dbContext.DishesInMenus
                    .Any(d => d.RequestMenuNumber == m.RequestMenuNumber && d.DishId == dishToHide.Id))
                .ToListAsync();


            foreach (var menuOrder in menuOrders)
            {
                menuOrder.IsVisible = true;
            }

            await dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(AllDishes));
        }

        // TODO...  Dereference of a possibly null reference.
        [HttpGet]
        public async Task<IActionResult> EditDish(int id)
        {
            var dish = await dbContext.Dishes.FindAsync(id);

            var model = new AllDishesViewModel()
            {
                DishName = dish.DishName,
                DishPrice = dish.Price,
                Description = dish.Description,
                CategoryId = dish.CategoryId
            };

            model.Categories = await helperMethods.GetCategoryAsync();

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> EditDish(AllDishesViewModel model, int dishId)
        {
            var dish = await dbContext.Dishes.FindAsync(dishId);

            if (ModelState.IsValid)
            {
                // TODO...  Dereference of a possibly null reference.
                var menuOrders = await dbContext.MenuOrders
                    .Where(m => dbContext.DishesInMenus
                        .Any(d => d.RequestMenuNumber == m.RequestMenuNumber &&
                                  d.DishId == dish.Id))
                    .ToListAsync();

                foreach (var menuOrder in menuOrders)
                {
                    menuOrder.TotalPrice = menuOrder.TotalPrice - dish.Price + model.DishPrice;
                }


                dish.DishName = model.DishName;
                dish.Price = model.DishPrice;
                dish.Description = model.Description;
                dish.CategoryId = model.CategoryId;

                if (model.ImageFile.Length > 0)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                    var fileExtension = Path.GetExtension(model.ImageFile.FileName).ToLower();

                    if (allowedExtensions.Contains(fileExtension))
                    {
                        var fileName = model.ImageFile.FileName;
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await model.ImageFile.CopyToAsync(stream);
                        }

                        dish.ImageUrl = "/img/" + fileName;
                    }
                    else
                    {
                        ModelState.AddModelError("ImageFile",
                            "Моля, качете изображение във формат .jpg, .jpeg или .png");
                        model.Categories = await helperMethods.GetCategoryAsync();
                        model.Dishes = await helperMethods.GetDishesAsync();
                        return View(model);
                    }
                }


                await dbContext.SaveChangesAsync();

                return RedirectToAction(nameof(AllDishes));
            }


            model.Categories = await helperMethods.GetCategoryAsync();
            model.Dishes = await helperMethods.GetDishesAsync();
            return View(model);

        }


        [HttpGet]
        public async Task<IActionResult> AddDish()
        {
            var userId = GetUserId();

            var userInDb = await dbContext.UserAgents
                .FirstOrDefaultAsync(ua => ua.UserId == userId);

            if (userInDb == null)
            {
                return BadRequest("You haven't permission for this ACTION");
            }


            var model = new AllDishesViewModel();
            model.Categories = await helperMethods.GetCategoryAsync();
            model.Dishes = await helperMethods.GetDishesAsync();


            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddDish(AllDishesViewModel model)
        {
            if (model.ImageFile.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                var fileExtension = Path.GetExtension(model.ImageFile.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("ImageFile", "Моля, качете изображение във формат .jpg, .jpeg или .png");
                    model.Categories = await helperMethods.GetCategoryAsync();
                    model.Dishes = await helperMethods.GetDishesAsync();
                    return View(model);
                }
            }
            if (ModelState.IsValid)
            {
                var dish = new Dish
                {
                    DishName = model.DishName,
                    Price = (decimal)model.DishPrice,
                    Description = model.Description,
                    CategoryId = model.CategoryId,
                    IsVisible = true

                };

                if (model.ImageFile.Length > 0)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                    var fileExtension = Path.GetExtension(model.ImageFile.FileName).ToLower();

                    if (allowedExtensions.Contains(fileExtension))
                    {
                        var fileName = model.ImageFile.FileName;
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await model.ImageFile.CopyToAsync(stream);
                        }

                        dish.ImageUrl = "/img/" + fileName;
                    }
                    else
                    {
                        ModelState.AddModelError("ImageFile", "Моля, качете изображение във формат .jpg, .jpeg или .png");
                        model.Categories = await helperMethods.GetCategoryAsync();
                        model.Dishes = await helperMethods.GetDishesAsync();
                        return View(model);
                    }
                }

                dbContext.Dishes.Add(dish);
                await dbContext.SaveChangesAsync();

                return RedirectToAction(nameof(AllDishes));
            }

            model.Categories = await helperMethods.GetCategoryAsync();
            model.Dishes = await helperMethods.GetDishesAsync();
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> DeleteDish(int dishId)
        {
            var dishToDelete = await dbContext.Dishes
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

            return View(dishToDelete);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteDishConfirm(int dishId)
        {
            var dishToDelete = await dbContext.Dishes.FindAsync(dishId);

            if (dishToDelete == null)
            {
                return NotFound();
            }

            dbContext.Dishes.Remove(dishToDelete);
            await dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(AllHiddenDishes));
        }
    }
}
