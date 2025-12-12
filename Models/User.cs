using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RecipeSharingApp.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string FullName { get; set; }

        [Required, MaxLength(100)]
        public string Email { get; set; }

        [Required, MaxLength(200)]
        public string PasswordHash { get; set; }   // SHA256

        [Required, MaxLength(20)]
        public string Role { get; set; }  // "Registered", "Creator", "Admin"

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation
        public UserProfile Profile { get; set; }

        public ICollection<Recipe> Recipes { get; set; }
        public ICollection<Rating> Ratings { get; set; }
        public ICollection<Favorite> Favorites { get; set; }
        public ICollection<MealPlan> MealPlans { get; set; }
        public ICollection<Notification> Notifications { get; set; }
    }
}
