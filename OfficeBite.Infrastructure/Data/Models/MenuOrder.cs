using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static OfficeBite.Infrastructure.Data.Constant.MenuConstants.MenuConstants;
namespace OfficeBite.Infrastructure.Data.Models
{
    public class MenuOrder
    {

        [Comment("Menu Order id")]
        [Key]
        public int Id { get; set; }

        [Comment("Menu name")]
        [MaxLength(MenuOrderNameMaximumLength)]
        public string MenuName { get; set; } = string.Empty;

        [Comment("Menu order Identifier with other tables")]
        [Required]
        public int RequestMenuNumber { get; set; }

        [Comment("Menu for date")]
        [Required]
        public DateTime SelectedMenuDate { get; set; }

        [Comment("Total price of selected menu")]
        [Required]
        public decimal TotalPrice { get; set; }

        [Comment("Description of selected menu")]
        [MaxLength(MenuOrderDescriptionMaximumLength)]
        public string Description { get; set; } = string.Empty;

        [Comment("Visibility  of menu")]
        [Required]
        public bool IsVisible { get; set; } = true;


        [Comment("Menu type identifier")]
        [Required]
        public int MenuTypeId { get; set; }

        [Comment("Menu types")]
        [ForeignKey(nameof(MenuTypeId))]
        public MenuType MenuType { get; set; } = null!;
        public ICollection<DishesInMenu> DishesInMenus { get; set; } = new List<DishesInMenu>();
    }
}