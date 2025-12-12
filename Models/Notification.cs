using System;
using System.ComponentModel.DataAnnotations;

namespace RecipeSharingApp.Models
{
    public enum NotificationType
    {
        RecipeComment = 1,
        RecipeRated = 2,
        MealPlanUpdated = 3,
        SystemMessage = 4
    }

    public class Notification
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        [Required, MaxLength(150)]
        public string Title { get; set; }
        [Required, MaxLength(400)]
        public string Message { get; set; }
        public NotificationType Type { get; set; }
        public bool IsRead { get; set; } = false;
        [MaxLength(260)]
        public string LinkUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
