using System.ComponentModel.DataAnnotations;

namespace RecipeSharingApp.Models
{
    public class RecipeInstruction
    {
        public int Id { get; set; }
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }

        [Required]
        public string InstructionText { get; set; }
    }
}
