using OfficeBite.Core.Models.DishModels;
using OfficeBite.Core.Models.MenuModels;

namespace OfficeBite.Core.Models.StaffModels
{
    public class StaffOrderViewModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string CustomerIdentifier { get; set; } = string.Empty;

        public string CustomerUsername { get; set; } = string.Empty;

        public string MenuName { get; set; } = string.Empty;
        public int RequestMenuNumber { get; set; }
        public IEnumerable<MenuViewModel> MenuOrders { get; set; } = new List<MenuViewModel>();

        public IEnumerable<DishViewModel> MenuItems { get; set; } = new List<DishViewModel>();
        public decimal TotalSum { get; set; }

        public DateTime SelectedDate { get; set; }

        public string Details { get; set; } = string.Empty;

        public string Comments { get; set; } = string.Empty;
    }
}
