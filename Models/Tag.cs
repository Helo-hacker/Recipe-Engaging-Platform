using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RecipeSharingApp.Models
{
    public class Tag
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }   // e.g. "Spicy", "Quick", "Dessert"

        public ICollection<RecipeTag> RecipeTags { get; set; }
    }


}
