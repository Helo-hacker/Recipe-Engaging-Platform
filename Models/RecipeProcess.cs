using System.ComponentModel.DataAnnotations;

namespace RecipeSharingApp.Models
{
    public class RecipeProcess
    {
        public int Id { get; set; }
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }

        [Required]
        public int StepNumber { get; set; }
        [Required]
        public string StepDescription { get; set; }
        public int StepTimeMinutes { get; set; }
        // [MaxLength(260)]
        // public string? ImagePath { get; set; }
    }
}
