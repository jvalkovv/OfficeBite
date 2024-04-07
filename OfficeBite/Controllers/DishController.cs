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

        //// Взема ястието, което трябва, да се скрие от менюто.
        //[HttpGet]
        //public async Task<IActionResult> HideDish(int DishId)
        //{
        //    var dishToHide = await dbContext.Dishes
        //        .Where(d => d.Id == DishId)
        //        .AsNoTracking()
        //        .Select(d => new HideDishViewModel()
        //        {
        //            DishId = d.Id,
        //            DishName = d.DishName,
        //            DishPrice = d.Price,
        //            Description = d.Description,
        //            ImageUrl = d.ImageUrl,
        //            IsVisible = d.IsVisible
        //        })
        //        .FirstOrDefaultAsync();

        //    return View(dishToHide);
        //}

        //// Потвърждение за скриване на ястието
        //[HttpPost]
        //public async Task<IActionResult> HideDishConfirm(int DishId)
        //{
        //    var dishToHide = await dbContext.Dishes.FindAsync(DishId);

        //    if (dishToHide == null)
        //    {
        //        return NotFound();
        //    }

        //    dishToHide.IsVisible = false;
        //    var allDishInOrders = dbContext.DishMenuToOrders.Where(d => d.DishId == dishToHide.Id);
        //    foreach (var currDish in allDishInOrders)
        //    {
        //        currDish.IsVisible = false;
        //    }

        //    var menuOrders = await dbContext.MenuOrders
        //        .Where(m => dbContext.DishMenuToOrders
        //            .Any(d => d.RequestMenuNumber == m.RequestMenuNumber && d.DishId == dishToHide.Id))
        //        .ToListAsync();

        //    // Променете общата цена на всяка поръчка за менюто
        //    foreach (var menuOrder in menuOrders)
        //    {
        //        menuOrder.IsVisible = false;
        //    }

        //    await dbContext.SaveChangesAsync();

        //    return RedirectToAction(nameof(AllDishes));
        //}

        //// Показва всички скрити ястия в Административният панел
        //[HttpGet]
        //public async Task<IActionResult> AllHiddenDishes()
        //{
        //    var userId = GetUserId();

        //    var userInDb = await dbContext.UserAgents
        //        .FirstOrDefaultAsync(ua => ua.UserId == userId);



        //    if (userInDb == null)
        //    {
        //        return BadRequest("You haven't permission for this ACTION");
        //    }


        //    var model = new AllDishesViewModel();
        //    model.Categories = await helperMethods.GetCategoryAsync();
        //    model.Dishes = helperMethods.GetDishesAsync().Result.Where(d => d.IsVisible == false);


        //    return View(model);
        //}


        //// Взема ястието, което трябва, да се покаже в менюто.
        //[HttpGet]
        //public async Task<IActionResult> UnHideDish(int id)
        //{

        //    var dishToUnHide = await dbContext.Dishes
        //        .Where(d => d.Id == id)
        //        .AsNoTracking()
        //        .Select(d => new HideDishViewModel()
        //        {
        //            DishId = d.Id,
        //            DishName = d.DishName,
        //            DishPrice = d.Price,
        //            Description = d.Description,
        //            ImageUrl = d.ImageUrl,
        //            IsVisible = d.IsVisible
        //        })
        //        .FirstOrDefaultAsync();


        //    return View(dishToUnHide);
        //}

        //// Потвърждение за показване на ястието
        //[HttpPost]
        //public async Task<IActionResult> UnHideDishConfirm(int DishId)
        //{
        //    var dishToHide = await dbContext.Dishes.FindAsync(DishId);

        //    if (dishToHide == null)
        //    {
        //        return NotFound();
        //    }

        //    dishToHide.IsVisible = true;
        //    var allDishInOrders = dbContext.DishMenuToOrders.Where(d => d.DishId == dishToHide.Id);
        //    foreach (var currDish in allDishInOrders)
        //    {
        //        currDish.IsVisible = true;
        //    }

        //    var menuOrders = await dbContext.MenuOrders
        //        .Where(m => dbContext.DishMenuToOrders
        //            .Any(d => d.RequestMenuNumber == m.RequestMenuNumber && d.DishId == dishToHide.Id))
        //        .ToListAsync();

        //    // Променете общата цена на всяка поръчка за менюто
        //    foreach (var menuOrder in menuOrders)
        //    {
        //        menuOrder.IsVisible = true;
        //    }

        //    await dbContext.SaveChangesAsync();

        //    return RedirectToAction(nameof(AllDishes));
        //}

        //// Взема ястието, което трябва, да се редактира.
        //[HttpGet]
        //public async Task<IActionResult> EditDish(int id)
        //{
        //    var dish = await dbContext.Dishes.FindAsync(id);

        //    var model = new EditDishViewModel()
        //    {
        //        DishName = dish.DishName,
        //        DishPrice = dish.Price,
        //        Details = dish.Description,
        //        CategoryId = dish.CategoryId
        //    };

        //    model.Categories = await helperMethods.GetCategoryAsync();

        //    return View(model);
        //}

        //// Редактиране на ястиеро
        //[HttpPost]
        //public async Task<IActionResult> EditDish(EditDishViewModel model, int id)
        //{
        //    var dish = await dbContext.Dishes.FindAsync(id);

        //    if (ModelState.IsValid)
        //    {
        //        var allDishInOrders = dbContext.DishMenuToOrders.Where(d => d.DishId == dish.Id);
        //        foreach (var currDish in allDishInOrders)
        //        {
        //            currDish.Price = model.DishPrice;
        //        }

        //        var menuOrders = await dbContext.MenuOrders
        //            .Where(m => dbContext.DishMenuToOrders
        //                .Any(d => d.RequestMenuNumber == m.RequestMenuNumber &&
        //                          d.DishId == dish.Id))
        //            .ToListAsync();

        //        // Променете общата цена на всяка поръчка за менюто
        //        foreach (var menuOrder in menuOrders)
        //        {
        //            menuOrder.TotalPrice = menuOrder.TotalPrice - dish.Price + model.DishPrice;
        //        }


        //        dish.DishName = model.DishName;
        //        dish.Price = model.DishPrice;
        //        dish.Description = model.Details;
        //        dish.CategoryId = model.CategoryId;

        //        if (model.ImageFile.Length > 0)
        //        {
        //            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
        //            var fileExtension = Path.GetExtension(model.ImageFile.FileName).ToLower();

        //            if (allowedExtensions.Contains(fileExtension))
        //            {
        //                var fileName = model.ImageFile.FileName;
        //                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", fileName);

        //                using (var stream = new FileStream(filePath, FileMode.Create))
        //                {
        //                    await model.ImageFile.CopyToAsync(stream);
        //                }

        //                dish.ImageUrl = "/img/" + fileName;
        //            }
        //            else
        //            {
        //                ModelState.AddModelError("ImageFile",
        //                    "Моля, качете изображение във формат .jpg, .jpeg или .png");
        //                model.Categories = await helperMethods.GetCategoryAsync();
        //                model.Dishes = await helperMethods.GetDishesAsync();
        //                return View(model);
        //            }
        //        }


        //        await dbContext.SaveChangesAsync();

        //        return RedirectToAction(nameof(AllDishes));
        //    }


        //    model.Categories = await helperMethods.GetCategoryAsync();
        //    model.Dishes = await helperMethods.GetDishesAsync();
        //    return View(model);

        //}


        ////Добавяне на ястие в общото менюто
        //[HttpGet]
        //public async Task<IActionResult> AddDish()
        //{
        //    var userId = GetUserId();

        //    var userInDb = await dbContext.UserAgents
        //        .FirstOrDefaultAsync(ua => ua.UserId == userId);

        //    if (userInDb == null)
        //    {
        //        return BadRequest("You haven't permission for this ACTION");
        //    }


        //    var model = new AddDishViewModel();
        //    model.Categories = await helperMethods.GetCategoryAsync();
        //    model.Dishes = await helperMethods.GetDishesAsync();

        //    return View(model);
        //}

        ////Добавяне на ястие в общото менюто
        //[HttpPost]
        //public async Task<IActionResult> AddDish(AddDishViewModel model)
        //{
        //    if (model.ImageFile != null && model.ImageFile.Length > 0)
        //    {
        //        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
        //        var fileExtension = Path.GetExtension(model.ImageFile.FileName).ToLower();

        //        if (!allowedExtensions.Contains(fileExtension))
        //        {
        //            ModelState.AddModelError("ImageFile", "Моля, качете изображение във формат .jpg, .jpeg или .png");
        //            model.Categories = await helperMethods.GetCategoryAsync();
        //            model.Dishes = await helperMethods.GetDishesAsync();
        //            return View(model);
        //        }
        //    }
        //    if (ModelState.IsValid)
        //    {
        //        var dish = new Dish
        //        {
        //            DishName = model.DishName,
        //            Price = (decimal)model.DishPrice,
        //            Description = model.Details,
        //            CategoryId = model.CategoryId,
        //            IsVisible = true

        //        };

        //        if (model.ImageFile.Length > 0)
        //        {
        //            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
        //            var fileExtension = Path.GetExtension(model.ImageFile.FileName).ToLower();

        //            if (allowedExtensions.Contains(fileExtension))
        //            {
        //                var fileName = model.ImageFile.FileName;
        //                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", fileName);

        //                using (var stream = new FileStream(filePath, FileMode.Create))
        //                {
        //                    await model.ImageFile.CopyToAsync(stream);
        //                }

        //                dish.ImageUrl = "/img/" + fileName;
        //            }
        //            else
        //            {
        //                ModelState.AddModelError("ImageFile", "Моля, качете изображение във формат .jpg, .jpeg или .png");
        //                model.Categories = await helperMethods.GetCategoryAsync();
        //                model.Dishes = await helperMethods.GetDishesAsync();
        //                return View(model);
        //            }
        //        }

        //        dbContext.Dishes.Add(dish);
        //        await dbContext.SaveChangesAsync();

        //        return RedirectToAction(nameof(AllDishes));
        //    }

        //    model.Categories = await helperMethods.GetCategoryAsync();
        //    model.Dishes = await helperMethods.GetDishesAsync();
        //    return View(model);
        //}

        //// Взема ястието, което трябва, да се изтрие.
        //[HttpGet]
        //public async Task<IActionResult> DeleteDish(int id)
        //{
        //    var dishToDelete = await dbContext.Dishes
        //        .Where(d => d.Id == id)
        //        .AsNoTracking()
        //        .Select(d => new RemoveDishViewModel
        //        {
        //            DishId = d.Id,
        //            DishName = d.DishName,
        //            DishPrice = d.Price,
        //            Description = d.Description,
        //            ImageUrl = d.ImageUrl,
        //            IsVisible = d.IsVisible
        //        })
        //        .FirstOrDefaultAsync();

        //    return View(dishToDelete);
        //}

        //// Изтриване на ястието.
        //[HttpPost]
        //public async Task<IActionResult> DeleteDishConfirm(int DishId)
        //{
        //    var dishToDelete = await dbContext.Dishes.FindAsync(DishId);

        //    if (dishToDelete == null)
        //    {
        //        return NotFound();
        //    }

        //    dbContext.Dishes.Remove(dishToDelete);
        //    await dbContext.SaveChangesAsync();

        //    return RedirectToAction(nameof(AllHiddenDishes));
        //}

    }
}
