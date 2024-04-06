using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace OfficeBite.Infrastructure.Data.Models
{
    public class MenuType
    {
        [Comment("Menu Type Identifier")]
        [Key]
        public int Id { get; set; }

        [Comment("Name of Menu type")]
        [Required]
        public string Name { get; set; } = string.Empty;

        public IEnumerable<DishesInMenu> DishesInMenus { get; set; } = new List<DishesInMenu>();
    }
}