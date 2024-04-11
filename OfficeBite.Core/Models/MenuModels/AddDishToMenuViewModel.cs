using OfficeBite.Core.Models.DishModels;

namespace OfficeBite.Core.Models.MenuModels
{
    public class AddDishToMenuViewModel
    {
        public int MenuTypeId { get; set; }

        public string Description { get; set; } = string.Empty;

        public List<DishViewModel> AllDishes { get; set; } = new List<DishViewModel>();

        public List<MenuTypeViewModel> AllMenuTypes { get; set; } = new List<MenuTypeViewModel>();
        public List<int> SelectedDishes { get; set; } = new List<int>();

        public List<DateTime> SelectedDates { get; set; } = new List<DateTime>();

        public int MenuId { get; set; }

        public int RequestMenuNumber { get; set; }
    }
}
