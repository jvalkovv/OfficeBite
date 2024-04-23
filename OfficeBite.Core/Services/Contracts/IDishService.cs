using OfficeBite.Core.Models.DishModels;

namespace OfficeBite.Core.Services.Contracts
{
    public interface IDishService
    {
        Task<AllDishesViewModel> GetAllDishes();

        Task<AllDishesViewModel> GetAllHiddenDishes();

        Task<DishViewModel?> HideDish(int dishId);

        Task HideDishConfirm(int dishId);

        Task<DishViewModel?> UnHideDish(int dishId);

        Task UnHideDishConfirm(int dishId);

        Task<AllDishesViewModel> EditDish(int dishId);


        Task<AllDishesViewModel> EditDish(AllDishesViewModel model, int dishId);


        Task<AllDishesViewModel> AddDish();

        Task<AllDishesViewModel> AddDish(AllDishesViewModel model);

        Task<DishViewModel?> DeleteDish(int dishId);

        Task DeleteDishConfirm(int dishId);
    }
}
