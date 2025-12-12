using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace RecipeSharingApp.Models
{
    public enum RecipeDifficulty { Easy = 1, Medium = 2, Hard = 3 }
    public enum RecipeStatus { Draft = 1, Published = 2 }

    public class Recipe
    {
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string Title { get; set; }

        [MaxLength(300)]
        public string ShortDescription { get; set; }

        public string Description { get; set; }

        public int Servings { get; set; }
        public int PrepTimeMinutes { get; set; }
        public int CookTimeMinutes { get; set; }
        public int TotalTimeMinutes { get; set; }

        [MaxLength(500)]
        public string Precautions { get; set; }

        public RecipeDifficulty Difficulty { get; set; }
        public RecipeStatus Status { get; set; } = RecipeStatus.Draft;

        [MaxLength(260)]
        public string? ThumbnailPath { get; set; }   // <-- FIX

        public int CreatorId { get; set; }

        [ValidateNever]
        public User Creator { get; set; }   // <-- FIX

        public int MealTypeId { get; set; }

        [ValidateNever]
        public MealType MealType { get; set; }   

        public int DietaryFlagId { get; set; }

        [ValidateNever]
        public DietaryFlag DietaryFlag { get; set; } 

        public int TotalLikes { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }


        [ValidateNever]
        public ICollection<RecipeIngredient> Ingredients { get; set; }

        [ValidateNever]
        public ICollection<RecipeProcess> Processes { get; set; }

        [ValidateNever]
        public ICollection<RecipeInstruction> Instructions { get; set; }

        [ValidateNever]
        public ICollection<Rating> Ratings { get; set; }   // <-- FIX

        [ValidateNever]
        public ICollection<Favorite> Favorites { get; set; }  // <-- FIX

        [ValidateNever]
        public ICollection<RecipeTag> RecipeTags { get; set; } // <-- FIX

        public Recipe()
        {
            Ingredients = new List<RecipeIngredient>();
            Processes = new List<RecipeProcess>();
            Instructions = new List<RecipeInstruction>();
            Ratings = new List<Rating>();
            Favorites = new List<Favorite>();
            RecipeTags = new List<RecipeTag>();
        }
    }
}
