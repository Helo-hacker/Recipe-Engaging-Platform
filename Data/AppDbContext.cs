using Microsoft.EntityFrameworkCore;
using RecipeSharingApp.Models;

namespace RecipeSharingApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<RecipeIngredient> RecipeIngredients { get; set; }
        public DbSet<RecipeProcess> RecipeProcesses { get; set; }
        public DbSet<RecipeInstruction> RecipeInstructions { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<MealPlan> MealPlans { get; set; }
        public DbSet<MealPlanItem> MealPlanItems { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<MealType> MealTypes { get; set; }
        public DbSet<DietaryFlag> DietaryFlags { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<RecipeTag> RecipeTags { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<RecipeLike> RecipeLikes { get; set; }
        public DbSet<RecipeComment> RecipeComments { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

// Favorite Composite Key
modelBuilder.Entity<Favorite>()
    .HasKey(f => new { f.UserId, f.RecipeId });

// Favorite → User (NO CASCADE)
modelBuilder.Entity<Favorite>()
    .HasOne(f => f.User)
    .WithMany(u => u.Favorites)
    .HasForeignKey(f => f.UserId)
    .OnDelete(DeleteBehavior.NoAction);

// Favorite → Recipe (CASCADE)
modelBuilder.Entity<Favorite>()
    .HasOne(f => f.Recipe)
    .WithMany(r => r.Favorites)
    .HasForeignKey(f => f.RecipeId)
    .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<RecipeTag>()
                .HasKey(rt => new { rt.RecipeId, rt.TagId });

// RecipeIngredient → Recipe
modelBuilder.Entity<RecipeIngredient>()
    .HasOne(ri => ri.Recipe)
    .WithMany(r => r.Ingredients)
    .HasForeignKey(ri => ri.RecipeId)
    .OnDelete(DeleteBehavior.Cascade);

// RecipeProcess → Recipe
modelBuilder.Entity<RecipeProcess>()
    .HasOne(rp => rp.Recipe)
    .WithMany(r => r.Processes)
    .HasForeignKey(rp => rp.RecipeId)
    .OnDelete(DeleteBehavior.Cascade);

// RecipeInstruction → Recipe
modelBuilder.Entity<RecipeInstruction>()
    .HasOne(ri => ri.Recipe)
    .WithMany(r => r.Instructions)
    .HasForeignKey(ri => ri.RecipeId)
    .OnDelete(DeleteBehavior.Cascade);



// Rating → Recipe
modelBuilder.Entity<Rating>()
    .HasOne(rt => rt.Recipe)
    .WithMany(r => r.Ratings)
    .HasForeignKey(rt => rt.RecipeId)
    .OnDelete(DeleteBehavior.Cascade);

// Rating → User
modelBuilder.Entity<Rating>()
    .HasOne(rt => rt.User)
    .WithMany(u => u.Ratings)
    .HasForeignKey(rt => rt.UserId)
    .OnDelete(DeleteBehavior.NoAction);



// MealPlanItem → MealPlan
modelBuilder.Entity<MealPlanItem>()
    .HasOne(mpi => mpi.MealPlan)
    .WithMany(mp => mp.Items)
    .HasForeignKey(mpi => mpi.MealPlanId)
    .OnDelete(DeleteBehavior.Cascade);

    // MealPlanItem → Recipe
modelBuilder.Entity<MealPlanItem>()
    .HasOne(mpi => mpi.Recipe)
    .WithMany()
    .HasForeignKey(mpi => mpi.RecipeId)
    .OnDelete(DeleteBehavior.NoAction);

// MealPlanItem → MealType
modelBuilder.Entity<MealPlanItem>()
    .HasOne(mpi => mpi.MealType)
    .WithMany(mt => mt.MealPlanItems)
    .HasForeignKey(mpi => mpi.MealTypeId)
    .OnDelete(DeleteBehavior.NoAction);



            // Default seed: Meal Types
            modelBuilder.Entity<MealType>().HasData(
                new MealType { Id = 1, Name = "Breakfast", IsActive = true },
                new MealType { Id = 2, Name = "Lunch / Dinner", IsActive = true },
                new MealType { Id = 3, Name = "Snacks", IsActive = true },
                new MealType { Id = 4, Name = "Tea", IsActive = true }
            );

            // Default seed: Dietary Flags
            modelBuilder.Entity<DietaryFlag>().HasData(
                new DietaryFlag { Id = 1, Name = "Veg", IsActive = true },
                new DietaryFlag { Id = 2, Name = "Non-Veg", IsActive = true },
                new DietaryFlag { Id = 3, Name = "All", IsActive = true }
            );

            modelBuilder.Entity<User>()
            .HasOne(u => u.Profile)
            .WithOne(p => p.User)
            .HasForeignKey<UserProfile>(p => p.Id);   // PK = FK

                // ------- LIKES -------
    modelBuilder.Entity<RecipeLike>()
        .HasOne(l => l.User)
        .WithMany()
        .HasForeignKey(l => l.UserId)
        .OnDelete(DeleteBehavior.Restrict); // prevents cascade loop

    modelBuilder.Entity<RecipeLike>()
        .HasOne(l => l.Recipe)
        .WithMany()
        .HasForeignKey(l => l.RecipeId)
        .OnDelete(DeleteBehavior.Cascade);

    // ------- COMMENTS -------
    modelBuilder.Entity<RecipeComment>()
        .HasOne(c => c.User)
        .WithMany()
        .HasForeignKey(c => c.UserId)
        .OnDelete(DeleteBehavior.Restrict);

    modelBuilder.Entity<RecipeComment>()
        .HasOne(c => c.Recipe)
        .WithMany()
        .HasForeignKey(c => c.RecipeId)
        .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
