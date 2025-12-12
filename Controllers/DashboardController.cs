using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeSharingApp.Data;
using RecipeSharingApp.Models;
using RecipeSharingApp.ViewModels;

namespace RecipeSharingApp.Controllers
{
    public class DashboardController : Controller
    {

        private readonly AppDbContext _context;

public DashboardController(AppDbContext context)
{
    _context = context;
}

        public IActionResult Index()
        {
            var role = HttpContext.Session.GetString("UserRole");

            if (role == null)
                return RedirectToAction("Login", "Account");

            return role switch
            {
                "Admin" => RedirectToAction("AdminDashboard"),
                "Creator" => RedirectToAction("CreatorDashboard"),
                _ => RedirectToAction("UserDashboard"),
            };
        }


        
        public async Task<IActionResult> UserDashboard()
        {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            if (userId == 0)
                return RedirectToAction("Login", "Account");

            // STATIC placeholder for saved recipes (you asked for this)
            int totalSaved = 0;

            // Real counts (if tables exist)
            int totalLiked = await _context.Set<RecipeLike>()
                .Where(l => l.UserId == userId)
                .CountAsync();

            int totalComments = await _context.Set<RecipeComment>()
                .Where(c => c.UserId == userId)
                .CountAsync();

            // Latest published recipe
            var latestRecipe = await _context.Recipes
                .Where(r => r.Status == RecipeStatus.Published)
                .OrderByDescending(r => r.CreatedAt)
                .FirstOrDefaultAsync();

            // Most liked published recipe (uses TotalLikes stored in recipe)
            var mostLikedRecipe = await _context.Recipes
                .Where(r => r.Status == RecipeStatus.Published)
                .OrderByDescending(r => r.TotalLikes)
                .FirstOrDefaultAsync();

            // Meal types for chips
            var mealTypes = await _context.MealTypes
                .Where(m => m.IsActive)
                .ToListAsync();

            // Recent viewed fallback: if you have RecipeViews table use that; otherwise show latest 6 published
            var recentViewed = await _context.Recipes
                .Where(r => r.Status == RecipeStatus.Published)
                .OrderByDescending(r => r.CreatedAt)
                .Take(6)
                .ToListAsync();

            var top3 = await _context.Recipes
                .Where(r => r.Status == RecipeStatus.Published)
                .OrderByDescending(r => r.TotalLikes)
                .Take(3)
                .ToListAsync();

            var vm = new RegisteredDashboardViewModel
            {
                TotalSaved = totalSaved,
                TotalLiked = totalLiked,
                TotalComments = totalComments,
                LatestRecipe = latestRecipe,
                MostLikedRecipe = mostLikedRecipe,
                MealTypes = mealTypes,
                RecentViewed = recentViewed,
                Top3Recipes = top3
            };

            return View(vm);
        }



public async Task<IActionResult> CreatorDashboard()
{
    int userId = HttpContext.Session.GetInt32("UserId") ?? 0;

    if (userId == 0)
        return RedirectToAction("Login", "Account");

    var recipes = await _context.Recipes
        .Where(r => r.CreatorId == userId)
        .OrderByDescending(r => r.CreatedAt)
        .ToListAsync();

    ViewBag.TotalRecipes = recipes.Count;
    ViewBag.Published = recipes.Count(r => r.Status == RecipeStatus.Published);
    ViewBag.Unpublished = recipes.Count(r => r.Status == RecipeStatus.Draft);

    // Placeholder likes (until you implement likes)
    ViewBag.TotalLikes = 0;

    ViewBag.Recent = recipes.Take(3).ToList();

    return View(recipes);
}




        public IActionResult AdminDashboard()
        {
            return View();
        }
    }
}
