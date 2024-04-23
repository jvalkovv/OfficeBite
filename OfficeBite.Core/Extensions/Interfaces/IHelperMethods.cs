using OfficeBite.Core.Models.CategoryModels;
using OfficeBite.Core.Models.DishModels;
using OfficeBite.Core.Models.MenuModels;

namespace OfficeBite.Core.Extensions.Interfaces
{
    public interface IHelperMethods
    {
        Task<IEnumerable<CategoryViewModel>> GetCategoryAsync();
        Task<List<MenuTypeViewModel>> GetMenuTypesAsync();
        Task<List<DishViewModel>> GetDishForMenuAsync();
        Task<List<DishViewModel>> GetDishesAsync();
    }
}
