using System.ComponentModel.DataAnnotations;

namespace RecipeSharingApp.Models
{
    public class RecipeIngredient
    {
        public int Id { get; set; }

        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }       // "Tomato"

        [Required, MaxLength(50)]
        public string Quantity { get; set; }   // "2 cups", "1 tsp"

        public int SortOrder { get; set; }     // to keep order defined by creator
    }
}
