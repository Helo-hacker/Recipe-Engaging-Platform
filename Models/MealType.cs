using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RecipeSharingApp.Models
{
    public class MealType
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }   // Breakfast, Lunch/Dinner, Snacks, Tea

        public bool IsActive { get; set; } = true;

        public ICollection<Recipe> Recipes { get; set; }
        public ICollection<MealPlanItem> MealPlanItems { get; set; }
    }
}
