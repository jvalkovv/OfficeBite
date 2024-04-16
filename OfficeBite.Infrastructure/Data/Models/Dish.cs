using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static OfficeBite.Infrastructure.Data.Constant.DishConstants.DishConstants;

namespace OfficeBite.Infrastructure.Data.Models
{
    //TODO.. Data validation range
    public class Dish
    {
        [Comment("Dish Identifier")]
        [Key]
        public int Id { get; set; }

        [Comment("Name of dish")]
        [Required]
        [MaxLength(DishNameMaximumLength)]
        public string DishName { get; set; } = string.Empty;

        [Comment("Price of dish")]
        [Required]
        public decimal Price { get; set; }

        [Comment("Description of dish")]
        [Required]
        [MaxLength(DishDescriptionMaximumLength)]
        public string Description { get; set; } = string.Empty;

        [Comment("Image of dish")]
        [Required]
        public string ImageUrl { get; set; } = string.Empty;

        [Comment("Visibility  of dish")]
        [Required]
        public bool IsVisible { get; set; } = true;


        [Comment("Category identifier of dish")]
        [Required]
        public int CategoryId { get; set; }

        [Comment("Category of dish")]
        [ForeignKey(nameof(CategoryId))]
        public DishCategory DishCategory { get; set; } = null!;
    }
}
