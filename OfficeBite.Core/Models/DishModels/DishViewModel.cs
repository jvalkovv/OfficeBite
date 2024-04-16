using System.ComponentModel.DataAnnotations;
using static OfficeBite.Infrastructure.Data.Constant.DishConstants.DishConstants;
using static OfficeBite.Core.ErrorMessages.ErrorMessage;

namespace OfficeBite.Core.Models.DishModels
{
    public class DishViewModel
    {
        public int DishId { get; set; }

        [Required(ErrorMessage = IsRequireErrorMessage)]
        [StringLength(DishNameMaximumLength,
            MinimumLength = DishNameMinimumLength,
            ErrorMessage = StringLengthErrorMessage)]
        public string DishName { get; set; } = string.Empty;

        [Required(ErrorMessage = IsRequireErrorMessage)]
        [DataType(DataType.Currency)]
        [Range((double)DishPriceMinimumValue, (double)DishPriceMaximumValue,
            ErrorMessage = PriceValueErrorMessage)]

        public decimal DishPrice { get; set; }

        [Required(ErrorMessage = IsRequireErrorMessage)]
        [StringLength(DishDescriptionMaximumLength,
            MinimumLength = DishDescriptionMinimumLength,
            ErrorMessage = StringLengthErrorMessage)]
        public string Description { get; set; } = string.Empty;

        public bool IsVisible { get; set; }

        public string ImageUrl { get; set; } = string.Empty;

        public int CategoryId { get; set; }

        public int MenuTypeId { get; set; }

        public int RequestMenuNumber { get; set; }

    }
}