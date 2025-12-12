using System.Collections.Generic;
using RecipeSharingApp.Models;

namespace RecipeSharingApp.ViewModels
{
    public class RegisteredDashboardViewModel
    {
        // Stat cards
        public int TotalSaved { get; set; }         // placeholder static value
        public int TotalLiked { get; set; }
        public int TotalComments { get; set; }

        // Featured
        public Recipe LatestRecipe { get; set; }
        public Recipe MostLikedRecipe { get; set; }

        // Collections
        public List<MealType> MealTypes { get; set; } = new();
        public List<Recipe> RecentViewed { get; set; } = new();
        public List<Recipe> Top3Recipes { get; set; } = new();
    }
}
