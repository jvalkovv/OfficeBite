﻿using OfficeBite.Core.Models.CategoryModels;
using OfficeBite.Core.Models.DishModels;

namespace OfficeBite.Core.Models.MenuModels
{
    public class MenuDailyViewModel
    {
        public IEnumerable<IGrouping<int, DishViewModel>> GroupDishes { get; set; } = new List<IGrouping<int, DishViewModel>>();

        public IEnumerable<DishViewModel> Dishes { get; set; } = new List<DishViewModel>();

        public IEnumerable<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();

        public List<DateTime> DateTimes { get; set; } = new List<DateTime>();

        public DateTime SelectedDate { get; set; }

        public IEnumerable<MenuTypeViewModel> MenuTypes { get; set; } = new List<MenuTypeViewModel>();

        public ICollection<MenuForDateViewModel> MenuForDateViewModels { get; set; } = new List<MenuForDateViewModel>();
    }
}