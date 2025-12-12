using System;

namespace RecipeSharingApp.Models
{
    public class Favorite
    {
        // composite key (UserId, RecipeId) will be configured in DbContext

        public int UserId { get; set; }
        public User User { get; set; }

        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    }
}
