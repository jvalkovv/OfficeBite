using Microsoft.EntityFrameworkCore;
using OfficeBite.Core.Extensions.Interfaces;
using OfficeBite.Core.Models.CategoryModels;
using OfficeBite.Core.Models.DishModels;
using OfficeBite.Core.Models.MenuModels;
using OfficeBite.Infrastructure.Data;

namespace OfficeBite.Core.Extensions
{
    public class HelperMethods : IHelperMethods
    {
        private readonly OfficeBiteDbContext dbContext;

        public HelperMethods(OfficeBiteDbContext _dbContext)
        {
            this.dbContext = _dbContext;
        }

        public async Task<IEnumerable<CategoryViewModel>> GetCategoryAsync()
        {
            return await dbContext.DishCategories
                .AsNoTracking()
                .Select(c => new CategoryViewModel()
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync();
        }
        public async Task<List<MenuTypeViewModel>> GetMenuTypesAsync()
        {
            return await dbContext.MenuTypes
                .AsNoTracking()
                .Select(m => new MenuTypeViewModel()
                {
                    MenuTypeId = m.Id,
                    MenuName = m.Name
                })
                .ToListAsync();
        }
        public async Task<List<DishViewModel>> GetDishForMenuAsync()
        {
            return await dbContext.Dishes
                .AsNoTracking()
                .Select(dm => new DishViewModel
                {
                    DishId = dm.Id,
                    DishName = dm.DishName,
                    Description = dm.Description,
                    DishPrice = dm.Price,
                    CategoryId = dm.CategoryId
                })
                .ToListAsync();
        }
        public async Task<List<DishViewModel>> GetDishesAsync()
        {
            return await dbContext.Dishes
                .AsNoTracking()
                .Select(d => new DishViewModel
                {
                    DishId = d.Id,
                    CategoryId = d.CategoryId,
                    DishName = d.DishName,
                    DishPrice = d.Price,
                    Description = d.Description,
                    ImageUrl = d.ImageUrl,
                    IsVisible = d.IsVisible
                })
                .ToListAsync();
        }

    }
}