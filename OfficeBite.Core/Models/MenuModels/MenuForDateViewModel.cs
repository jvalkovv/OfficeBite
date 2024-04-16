using OfficeBite.Core.Models.DishModels;
using System.ComponentModel;

namespace OfficeBite.Core.Models.MenuModels
{
    public class MenuForDateViewModel
    {
        [DisplayName("Име на менюто")]
        public string MenuName { get; set; } = string.Empty;

        [DisplayName("Описание на менюто")]
        public string Description { get; set; } = string.Empty;
        public int RequestMenuNumber { get; set; }
        public decimal TotalPrice { get; set; }
        public List<DishViewModel> Dishes { get; set; } = new List<DishViewModel>();
    }
}