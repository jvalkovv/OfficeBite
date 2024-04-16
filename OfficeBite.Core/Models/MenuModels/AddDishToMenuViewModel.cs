using OfficeBite.Core.Models.DishModels;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static OfficeBite.Core.ErrorMessages.ErrorMessage;
using static OfficeBite.Infrastructure.Data.Constant.MenuConstants.MenuConstants;
namespace OfficeBite.Core.Models.MenuModels
{
    public class AddDishToMenuViewModel
    {
        [DisplayName("Тип на Менюто")]
        [Required(ErrorMessage = IsRequireErrorMessage)]
        public int MenuTypeId { get; set; }

        [DisplayName("Име на менюто")]
        [Required(ErrorMessage = IsRequireErrorMessage)]
        [StringLength(MenuOrderNameMaximumLength,
            MinimumLength = MenuOrderNameMinimumLength,
            ErrorMessage = StringLengthErrorMessage)]
        public string MenuName { get; set; } = string.Empty;

        [DisplayName("Описание на менюто")]
        [Required(ErrorMessage = IsRequireErrorMessage)]
        [StringLength(MenuOrderDescriptionMaximumLength,
            MinimumLength = MenuOrderDescriptionMinimumLength,
            ErrorMessage = StringLengthErrorMessage)]
        public string Description { get; set; } = string.Empty;

        public List<DishViewModel> AllDishes { get; set; } = new List<DishViewModel>();

        public List<MenuTypeViewModel> AllMenuTypes { get; set; } = new List<MenuTypeViewModel>();

        [DisplayName("Избрани ястия")]
        [Required(ErrorMessage = IsRequireErrorMessage)]
        public List<int> SelectedDishes { get; set; } = new List<int>();

        [DisplayName("Избрани дати")]
        public List<string> SelectedDates { get; set; } = new List<string>();

        public int RequestMenuNumber { get; set; }
    }
}
