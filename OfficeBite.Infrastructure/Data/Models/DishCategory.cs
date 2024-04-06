using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace OfficeBite.Infrastructure.Data.Models
{
    public class DishCategory
    {
        [Comment("Dish Category Identifier")]
        [Key]
        public int Id { get; set; }

        [Comment("Name of Category")]
        [Required]
        public string Name { get; set; } = string.Empty;

        public IEnumerable<Dish> Dishes { get; set; } = new List<Dish>();
    }
}