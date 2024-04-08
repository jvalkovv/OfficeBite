using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using OfficeBite.Core.Models.CategoryModels;

namespace OfficeBite.Core.Models.DishModels
{
    public class AllDishesViewModel
    {
        public int DishId { get; set; }

        [Display(Name = "Име на ястието")]
        public string DishName { get; set; } = string.Empty;

        [Display(Name = "Продажна цена на ястието")]
        public decimal DishPrice { get; set; }

        [Display(Name = "Описание на ястието")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Изберете категория на ястието")]
        public int CategoryId { get; set; }

        public bool IsVisible { get; set; }

        [Display(Name = "Качете снимка на ястието")]
        public IFormFile ImageFile { get; set; } = null!;

        public IEnumerable<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();
        public IEnumerable<DishViewModel> Dishes { get; set; } = new List<DishViewModel>();
    }

}