using Microsoft.AspNetCore.Http;
using OfficeBite.Core.Models.CategoryModels;
using OfficeBite.Core.Models.DishModels;
using System.ComponentModel.DataAnnotations;

namespace OfficeBite.Core.Models.MenuModels
{
    public class MenuViewModel
    {
        public int DishId { get; set; }

        [Display(Name = "Име на ястието")]
        public string DishName { get; set; } = string.Empty;
        [Display(Name = "Цена на ястието")]
        public decimal DishPrice { get; set; }

        [Display(Name = "Снимка")]
        public IFormFile ImageFile { get; set; } = null!;
        public string Description { get; set; } = string.Empty;

        public int CategoryId { get; set; }

        public double Rating { get; set; }
        public DateTime SelectedDate { get; set; }
        public List<DateTime> DateTimes { get; set; } = new List<DateTime>();

        public IEnumerable<DishViewModel> Dishes { get; set; } = new List<DishViewModel>();

        public IEnumerable<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();

    }
}
