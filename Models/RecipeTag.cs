using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
    
namespace RecipeSharingApp.Models
{
    public class RecipeTag
    {
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }

        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}