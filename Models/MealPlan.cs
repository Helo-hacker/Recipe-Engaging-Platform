using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RecipeSharingApp.Models
{
    public class MealPlan
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<MealPlanItem> Items { get; set; }
    }
}
