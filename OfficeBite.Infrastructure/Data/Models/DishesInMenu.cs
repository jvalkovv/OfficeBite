using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OfficeBite.Infrastructure.Data.Models
{
    //TODO.. Data validation range
    public class DishesInMenu
    {
        [Comment("Dishes In Menu Identifier")]
        [Key]
        public int Id { get; set; }
        
        [Comment("Visibility  of dishes")]
        [Required]
        public bool IsVisible { get; set; } = true;

        public int DishId { get; set; }

        [ForeignKey(nameof(DishId))]
        public Dish Dish { get; set; } = null!;


        [Comment("Menu identifier")]
        [Required]
        public int RequestMenuNumber { get; set; }

        [Comment("Lunch menu")]
        [ForeignKey(nameof(RequestMenuNumber))]
        public MenuOrder MenuOrder { get; set; } = null!;
    }
}
