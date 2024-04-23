using OfficeBite.Core.Models.MenuModels;

namespace OfficeBite.Core.Services.Contracts
{
    public interface IMenuService
    {
        Task<MenuViewModel> GetMenuListAsync();

        Task<MenuDailyViewModel> GetMenuDailyListAsync();

        Task<MenuDailyViewModel> MenuDailyList(MenuDailyViewModel model);

        Task<AddDishToMenuViewModel> GetMenuAddDishToMenuListAsync();

        Task<AddDishToMenuViewModel> AddDishToMenuListAsync(AddDishToMenuViewModel model);


    }
}
