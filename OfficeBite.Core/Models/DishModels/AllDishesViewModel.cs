using Microsoft.AspNetCore.Http;
using OfficeBite.Core.Models.CategoryModels;
using System.ComponentModel.DataAnnotations;
using static OfficeBite.Infrastructure.Data.Constant.DishConstants.DishConstants;
using static OfficeBite.Core.ErrorMessages.ErrorMessage;

namespace OfficeBite.Core.Models.DishModels
{
    public class AllDishesViewModel
    {
        public int DishId { get; set; }

        [Display(Name = "Име на ястието")]
        [Required(ErrorMessage = IsRequireErrorMessage)]
        [StringLength(DishNameMaximumLength,
            MinimumLength = DishNameMinimumLength,
            ErrorMessage = StringLengthErrorMessage)]
        public string DishName { get; set; } = string.Empty;


        [Display(Name = "Продажна цена на ястието")]
        [Required(ErrorMessage = IsRequireErrorMessage)]
        [Range(DishPriceMinimumValue, DishPriceMaximumValue, ErrorMessage = PriceValueErrorMessage)]
        public decimal DishPrice { get; set; }


        [Display(Name = "Описание на ястието")]
        [Required(ErrorMessage = IsRequireErrorMessage)]
        [StringLength(DishDescriptionMaximumLength,
            MinimumLength = DishDescriptionMinimumLength,
            ErrorMessage = StringLengthErrorMessage)]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Изберете категория на ястието")]
        public int CategoryId { get; set; }

        public bool IsVisible { get; set; }

        [Display(Name = "Качете снимка на ястието")]
        [Required(ErrorMessage = IsRequireErrorMessage)]

        public IFormFile ImageFile { get; set; } = null!;

        public IEnumerable<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();
        public IEnumerable<DishViewModel> Dishes { get; set; } = new List<DishViewModel>();
    }

}