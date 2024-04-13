using OfficeBite.Core.Models.DishModels;
using OfficeBite.Core.Models.MenuModels;

namespace OfficeBite.Core.Models.OrderModels
{
    public class AddOrderViewModel
    {
        public int DishId { get; set; }
        public string DishName { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal TotalPrice { get; set; }

        public List<DishViewModel> AllDishes { get; set; } = new List<DishViewModel>();

        public List<MenuTypeViewModel> AllMenuTypes { get; set; } = new List<MenuTypeViewModel>();
    }
}
