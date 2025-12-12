using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RecipeSharingApp.Models
{
    public class DietaryFlag
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }   // Veg, Non-Veg, All

        public bool IsActive { get; set; } = true;

        public ICollection<Recipe> Recipes { get; set; }
    }
}
