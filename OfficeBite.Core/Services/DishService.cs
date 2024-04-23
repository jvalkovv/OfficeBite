using Microsoft.EntityFrameworkCore;
using OfficeBite.Core.Extensions.Interfaces;
using OfficeBite.Core.Models.DishModels;
using OfficeBite.Core.Services.Contracts;
using OfficeBite.Infrastructure.Data.Common;
using OfficeBite.Infrastructure.Data.Models;

namespace OfficeBite.Core.Services
{
    public class DishService : IDishService
    {
        private readonly IRepository repository;
        private readonly IHelperMethods helperMethods;

        public DishService(IRepository _repository, IHelperMethods _helperMethods)
        {
            repository = _repository;
            helperMethods = _helperMethods;
        }

        public async Task<AllDishesViewModel> GetAllDishes()
        {
            var model = new AllDishesViewModel
            {
                Categories = await helperMethods.GetCategoryAsync(),
                Dishes = await helperMethods.GetDishesAsync()
            };

            return model;
        }

        public async Task<AllDishesViewModel> GetAllHiddenDishes()
        {
            var model = new AllDishesViewModel();
            model.Categories = await helperMethods.GetCategoryAsync();
            model.Dishes = helperMethods.GetDishesAsync()
                .Result.Where(d => d.IsVisible == false);


            return model;
        }

        public async Task<DishViewModel?> HideDish(int dishId)
        {
            var dishToHide = await repository.All<Dish>()
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

            return dishToHide;
        }

        public async Task HideDishConfirm(int dishId)
        {
            var dishToHide = await repository.GetByIdAsync<Dish>(dishId);

            dishToHide.IsVisible = false;
            var allDishInOrders = repository.All<DishesInMenu>()
                .Where(d => d.DishId == dishToHide.Id);

            foreach (var currDish in allDishInOrders)
            {
                currDish.IsVisible = false;
            }

            var menuOrders = await repository.All<MenuOrder>()
                .Where(m => repository.All<DishesInMenu>()
                    .Any(d => d.RequestMenuNumber == m.RequestMenuNumber && d.DishId == dishToHide.Id))
                .ToListAsync();


            foreach (var menuOrder in menuOrders)
            {
                menuOrder.IsVisible = false;
            }

            await repository.SaveChangesAsync();

        }

        public async Task<DishViewModel?> UnHideDish(int dishId)
        {
            var dishToUnHide = await repository.All<Dish>()
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

            return dishToUnHide;
        }

        public async Task UnHideDishConfirm(int dishId)
        {
            var dishToUnHide = await repository.GetByIdAsync<Dish>(dishId);

            dishToUnHide.IsVisible = true;
            var allDishInOrders = repository.All<DishesInMenu>()
                .Where(d => d.DishId == dishToUnHide.Id);

            foreach (var currDish in allDishInOrders)
            {
                currDish.IsVisible = true;
            }

            var menuOrders = await repository.All<MenuOrder>()
                .Where(m => repository.All<DishesInMenu>()
                    .Any(d => d.RequestMenuNumber == m.RequestMenuNumber &&
                              d.DishId == dishToUnHide.Id))
                .ToListAsync();


            foreach (var menuOrder in menuOrders)
            {
                menuOrder.IsVisible = true;
            }

            await repository.SaveChangesAsync();
        }

        public async Task<AllDishesViewModel> EditDish(int dishId)
        {
            var dish = await repository.GetByIdAsync<Dish>(dishId);

            var model = new AllDishesViewModel()
            {
                DishId = dish.Id,
                DishName = dish.DishName,
                DishPrice = dish.Price,
                Description = dish.Description,
                CategoryId = dish.CategoryId
            };

            model.Categories = await helperMethods.GetCategoryAsync();

            return model;
        }

        public async Task<AllDishesViewModel> EditDish(AllDishesViewModel model, int dishId)
        {
            var dish = await repository.GetByIdAsync<Dish>(dishId);

            var menuOrders = await repository.All<MenuOrder>()
                .Where(m => repository.All<DishesInMenu>()
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


            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var fileExtension = System.IO.Path.GetExtension(model.ImageFile.FileName).ToLower();

            if (allowedExtensions.Contains(fileExtension))
            {
                var fileName = model.ImageFile.FileName;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", fileName);

                if (filePath != null)
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(stream);
                    }

                    dish.ImageUrl = "/img/" + fileName;
                }
                await repository.SaveChangesAsync();
            }
            else
            {
                model.ImageFile = null;
            }


            model.Categories = await helperMethods.GetCategoryAsync();
            model.Dishes = await helperMethods.GetDishesAsync();

            return model;
        }


        public async Task<AllDishesViewModel> AddDish()
        {
            var model = new AllDishesViewModel();
            model.Categories = await helperMethods.GetCategoryAsync();
            model.Dishes = await helperMethods.GetDishesAsync();

            return model;
        }

        public async Task<AllDishesViewModel> AddDish(AllDishesViewModel model)
        {
            var dish = new Dish
            {
                DishName = model.DishName,
                Price = (decimal)model.DishPrice,
                Description = model.Description,
                CategoryId = model.CategoryId,
                IsVisible = true

            };

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var fileExtension = System.IO.Path.GetExtension(model.ImageFile.FileName).ToLower();

            if (allowedExtensions.Contains(fileExtension))
            {
                var fileName = model.ImageFile.FileName;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", fileName);

                if (filePath != null)
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(stream);
                    }

                    dish.ImageUrl = "/img/" + fileName;
                }

                await repository.AddAsync<Dish>(dish);

                await repository.SaveChangesAsync();
            }
            else
            {
                model.ImageFile = null;
            }


            model.Categories = await helperMethods.GetCategoryAsync();
            model.Dishes = await helperMethods.GetDishesAsync();

            return model;
        }

        public async Task<DishViewModel?> DeleteDish(int dishId)
        {
            var dishToDelete = await repository.AllReadOnly<Dish>()
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

            return dishToDelete;
        }

        public async Task DeleteDishConfirm(int dishId)
        {
            var dishToDelete = await repository.GetByIdAsync<Dish>(dishId);

            await repository.DeleteAsync<Dish>(dishToDelete.Id);
            await repository.SaveChangesAsync();

        }
    }
}
