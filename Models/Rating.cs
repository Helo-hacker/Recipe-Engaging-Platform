using System;
using System.ComponentModel.DataAnnotations;

namespace RecipeSharingApp.Models
{
    public class Rating
    {
        public int Id { get; set; }

        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        [Range(1, 5)]
        public int Score { get; set; }

        [MaxLength(200)]
        public string Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
