using System;
using System.ComponentModel.DataAnnotations;

namespace RecipeSharingApp.Models
{
    public class MealPlanItem
    {
        public int Id { get; set; }

        public int MealPlanId { get; set; }
        public MealPlan MealPlan { get; set; }

        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }

        public DateTime PlannedDate { get; set; }

        public int MealTypeId { get; set; }
        public MealType MealType { get; set; }

        [MaxLength(150)]
        public string Notes { get; set; }
    }
}
