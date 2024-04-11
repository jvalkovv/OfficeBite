using OfficeBite.Core.Models.DishModels;

namespace OfficeBite.Core.Models.MenuModels
{
    public class MenuForDateViewModel
    {
        public int RequestMenuNumber { get; set; }
        public decimal TotalPrice { get; set; }
        public string Description { get; set; } = string.Empty;
        public List<DishViewModel> Dishes { get; set; } = new List<DishViewModel>();
    }
}