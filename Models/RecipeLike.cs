namespace RecipeSharingApp.Models
{
    public class RecipeLike
    {
        public int Id { get; set; }

        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public DateTime LikedAt { get; set; } = DateTime.UtcNow;
    }
}
