namespace OfficeBite.Core.Models.DishModels
{
    public class DishViewModel
    {
        public int DishId { get; set; }

        public string DishName { get; set; } = string.Empty;

        public decimal Price { get; set; }
        public string Description { get; set; } = string.Empty;

        public bool IsVisible { get; set; }


        public string ImageUrl { get; set; } = string.Empty;

        public int CategoryId { get; set; }

        public int MenuTypeId { get; set; }

        public int RequestMenuNumber { get; set; }

    }
}