using Microsoft.AspNetCore.Mvc;
using RecipeSharingApp.Data;
using RecipeSharingApp.Models;
using RecipeSharingApp.Helpers;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Authorization;


namespace RecipeSharingApp.Controllers
{
public class RecipeController : Controller

    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public RecipeController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: Recipe/Create
        [AuthorizeRoles("Creator")]
        public IActionResult Create()
        {
            ViewBag.MealTypes = _context.MealTypes.Where(x => x.IsActive).ToList();
            ViewBag.Dietary = _context.DietaryFlags.Where(x => x.IsActive).ToList();
            return View();
        }

        // POST: Recipe/Create
        [AuthorizeRoles("Creator")]
        [HttpPost]
        public async Task<IActionResult> Create(Recipe model, IFormFile ThumbnailFile)
        {
        if (!ModelState.IsValid)
        {
            foreach (var item in ModelState)
            {
                foreach (var error in item.Value.Errors)
                {
                    Console.WriteLine("MODEL ERROR => " + item.Key + " : " + error.ErrorMessage);
                }
            }

            ViewBag.MealTypes = _context.MealTypes.Where(x => x.IsActive).ToList();
            ViewBag.Dietary = _context.DietaryFlags.Where(x => x.IsActive).ToList();
            ViewBag.Error = "Please fill all required fields.";
            return View(model);
        }


            if (ThumbnailFile != null)
            {
                string fileName = Guid.NewGuid() + Path.GetExtension(ThumbnailFile.FileName);
                string uploadFolder = Path.Combine(_env.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadFolder);
                string path = Path.Combine(uploadFolder, fileName);

                using var stream = new FileStream(path, FileMode.Create);
                await ThumbnailFile.CopyToAsync(stream);

                model.ThumbnailPath = "/uploads/" + fileName;
            }

            model.CreatorId = HttpContext.Session.GetInt32("UserId") ?? 0;
            model.CreatedAt = DateTime.UtcNow;
            model.TotalTimeMinutes = model.PrepTimeMinutes + model.CookTimeMinutes;
            model.Status = RecipeStatus.Draft; //Draft by default

            _context.Recipes.Add(model);
            await _context.SaveChangesAsync();

            // go to edit page to add ingredients/steps
            return RedirectToAction("Edit", new { id = model.Id });
        }

        // GET: Recipe/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var recipe = await _context.Recipes
                .Include(r => r.Ingredients)
                .Include(r => r.Processes)
                .Include(r => r.Instructions)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (recipe == null)
                return NotFound();

  ViewBag.MealTypes = await _context.MealTypes.Where(m => m.IsActive).ToListAsync();
    ViewBag.DietaryFlags = await _context.DietaryFlags.Where(d => d.IsActive).ToListAsync();


            // optional: ensure current user is creator
            var userId = HttpContext.Session.GetInt32("UserId");
            if (recipe.CreatorId != userId)
                return Forbid();

            return View(recipe);
        }

        // POST: Add Ingredient
[AuthorizeRoles("Creator")]
 [HttpPost]
public async Task<IActionResult> AddIngredient(int RecipeId, string Name, string Quantity)
{
    var recipe = await _context.Recipes.FindAsync(RecipeId);
    if (recipe == null) return NotFound();

    int nextOrder = _context.RecipeIngredients
        .Where(i => i.RecipeId == RecipeId)
        .Count() + 1;

    var ing = new RecipeIngredient
    {
        RecipeId = RecipeId,
        Name = Name,
        Quantity = Quantity,
        SortOrder = nextOrder
    };

    _context.RecipeIngredients.Add(ing);
    await _context.SaveChangesAsync();

    return RedirectToAction("Edit", new { id = RecipeId });
}

[AuthorizeRoles("Creator")]
[HttpPost]
public async Task<IActionResult> DeleteIngredient(int id, int recipeId)
{
    var ing = await _context.RecipeIngredients.FindAsync(id);
    if (ing != null)
    {
        _context.RecipeIngredients.Remove(ing);
        await _context.SaveChangesAsync();
    }

    return RedirectToAction("Edit", new { id = recipeId });
}

[AuthorizeRoles("Creator")]
[HttpPost]
public async Task<IActionResult> AddStep(int RecipeId, string StepDescription)
{
    var lastStep = await _context.RecipeProcesses
        .Where(p => p.RecipeId == RecipeId)
        .OrderByDescending(p => p.StepNumber)
        .FirstOrDefaultAsync();

    int nextStepNumber = (lastStep?.StepNumber ?? 0) + 1;

    var step = new RecipeProcess
    {
        RecipeId = RecipeId,
        StepDescription = StepDescription,
        StepNumber = nextStepNumber
    };

    _context.RecipeProcesses.Add(step);
    await _context.SaveChangesAsync();

    return RedirectToAction("Edit", new { id = RecipeId });
}


[AuthorizeRoles("Creator")]
[HttpPost]
public async Task<IActionResult> DeleteStep(int id, int recipeId)
{
    var step = await _context.RecipeProcesses.FindAsync(id);

    if (step != null)
    {
        _context.RecipeProcesses.Remove(step);
        await _context.SaveChangesAsync();
    }

    // Renumber steps after deletion
    var steps = await _context.RecipeProcesses
        .Where(s => s.RecipeId == recipeId)
        .OrderBy(s => s.StepNumber)
        .ToListAsync();

    int number = 1;
    foreach (var s in steps)
    {
        s.StepNumber = number++;
    }

    await _context.SaveChangesAsync();

    return RedirectToAction("Edit", new { id = recipeId });
}


[AuthorizeRoles("Creator")]
[HttpPost]
public async Task<IActionResult> AddInstruction(int RecipeId, string InstructionText)
{
    var ins = new RecipeInstruction
    {
        RecipeId = RecipeId,
        InstructionText = InstructionText
    };

    _context.RecipeInstructions.Add(ins);
    await _context.SaveChangesAsync();

    return RedirectToAction("Edit", new { id = RecipeId });
}

[AuthorizeRoles("Creator")]
[HttpPost]
public async Task<IActionResult> DeleteInstruction(int id, int recipeId)
{
    var ins = await _context.RecipeInstructions.FindAsync(id);
    if (ins != null)
    {
        _context.RecipeInstructions.Remove(ins);
        await _context.SaveChangesAsync();
    }

    return RedirectToAction("Edit", new { id = recipeId });
}

[AuthorizeRoles("Creator")]
[HttpPost]
public async Task<IActionResult> Publish(int id)
{
    var recipe = await _context.Recipes.FindAsync(id);
    if (recipe == null) return NotFound();

    recipe.Status = RecipeStatus.Published;
    recipe.UpdatedAt = DateTime.UtcNow;

    await _context.SaveChangesAsync();

    return RedirectToAction("PublishManager");
}

[AuthorizeRoles("Creator")]
[HttpPost]
public async Task<IActionResult> Unpublish(int id)
{
    var recipe = await _context.Recipes.FindAsync(id);
    if (recipe == null) return NotFound();

    recipe.Status = RecipeStatus.Draft;
    recipe.UpdatedAt = DateTime.UtcNow;

    await _context.SaveChangesAsync();

    return RedirectToAction("PublishManager");
}


// Manage Recipe page . 
[AuthorizeRoles("Creator")]
public async Task<IActionResult> Manage()
{
    int userId = HttpContext.Session.GetInt32("UserId") ?? 0;

    var recipes = await _context.Recipes
        .Where(r => r.CreatorId == userId)
        .OrderByDescending(r => r.CreatedAt)
        .ToListAsync();

    return View(recipes);
}

[AuthorizeRoles("Creator")]
[HttpPost]
public async Task<IActionResult> UpdateDetails(Recipe updated, IFormFile ThumbnailFile)
{
    var recipe = await _context.Recipes.FindAsync(updated.Id);
    if (recipe == null) return NotFound();

    recipe.Title = updated.Title;
    recipe.ShortDescription = updated.ShortDescription;
    recipe.Description = updated.Description;
    recipe.PrepTimeMinutes = updated.PrepTimeMinutes;
    recipe.CookTimeMinutes = updated.CookTimeMinutes;
    recipe.TotalTimeMinutes = updated.PrepTimeMinutes + updated.CookTimeMinutes;
    recipe.Servings = updated.Servings;
    recipe.MealTypeId = updated.MealTypeId;
    recipe.DietaryFlagId = updated.DietaryFlagId;

    // Thumbnail Upload
    if (ThumbnailFile != null)
    {
        string path = Path.Combine("wwwroot/uploads", ThumbnailFile.FileName);

        using (var stream = new FileStream(path, FileMode.Create))
        {
            await ThumbnailFile.CopyToAsync(stream);
        }

        recipe.ThumbnailPath = "/uploads/" + ThumbnailFile.FileName;
    }

    await _context.SaveChangesAsync();

    return RedirectToAction("Edit", new { id = updated.Id });
}


//Publish/Unpublish page.
[AuthorizeRoles("Creator")]
public async Task<IActionResult> PublishManager()
{
    int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
    Console.WriteLine("DEBUG → Logged-in Creator ID: " + userId);

    var recipes = await _context.Recipes
        .Where(r => r.CreatorId == userId)
        .OrderByDescending(r => r.CreatedAt)
        .ToListAsync();

    Console.WriteLine("DEBUG → Recipes found: " + recipes.Count);

    return View(recipes);
}



//------------------------------------------------------------------------------
//----------------------------GUEST/REGISTERED USER------------------------------
//------------------------------------------------------------------------------


// View Data as per Guest/Registered
[AllowAnonymous]
public async Task<IActionResult> Details(int id)
{
    var recipe = await _context.Recipes
        .Include(r => r.Ingredients)
        .Include(r => r.Processes)
        .Include(r => r.Instructions)
        .Include(r => r.Creator)
            .ThenInclude(u => u.Profile)
        .FirstOrDefaultAsync(r => r.Id == id && r.Status == RecipeStatus.Published);

        var comments = await _context.RecipeComments
    .Include(c => c.User)
    .Where(c => c.RecipeId == id)
    .OrderByDescending(c => c.CreatedAt)
    .ToListAsync();

    // Determine if CURRENT user already liked it
int userId = HttpContext.Session.GetInt32("UserId") ?? 0;

bool alreadyLiked = await _context.RecipeLikes
    .AnyAsync(l => l.UserId == userId && l.RecipeId == id);

ViewBag.AlreadyLiked = alreadyLiked;


ViewBag.Comments = comments;

    if (recipe == null)
        return NotFound();

    return View(recipe);
}

[AllowAnonymous]
public async Task<IActionResult> List(
    string? search,
    string? ingredient,
    int? mealTypeId,
    int? dietaryId,
    int? cookTime)
{
    var query = _context.Recipes
        .Include(r => r.MealType)
        .Include(r => r.DietaryFlag)
        .Include(r => r.Ingredients) // needed for ingredient-based filtering
        .Where(r => r.Status == RecipeStatus.Published)
        .AsQueryable();

    /* ================================
       SEARCH BY TITLE / DESCRIPTION
       ================================= */
    if (!string.IsNullOrWhiteSpace(search))
    {
        string s = search.Trim().ToLower();
        query = query.Where(r =>
            r.Title.ToLower().Contains(s) ||
            r.ShortDescription.ToLower().Contains(s) ||
            r.Description.ToLower().Contains(s)
        );
    }

    /* ================================
       SEARCH BY INGREDIENT NAME
       ================================= */
    if (!string.IsNullOrWhiteSpace(ingredient))
    {
        string ing = ingredient.Trim().ToLower();
        query = query.Where(r =>
            r.Ingredients.Any(i => i.Name.ToLower().Contains(ing))
        );
    }

    /* ================================
       FILTER: MEAL TYPE
       ================================= */
    if (mealTypeId.HasValue && mealTypeId.Value > 0)
    {
        query = query.Where(r => r.MealTypeId == mealTypeId.Value);
    }

    /* ================================
       FILTER: DIET PREFERENCE
       ================================= */
    if (dietaryId.HasValue && dietaryId.Value > 0)
    {
        query = query.Where(r => r.DietaryFlagId == dietaryId.Value);
    }

    /* ================================
       FILTER: COOK TIME
       ================================= */
    if (cookTime.HasValue && cookTime.Value > 0)
    {
        query = query.Where(r => r.TotalTimeMinutes <= cookTime.Value);
    }

    /* ================================
       POPULATE FILTER DROPDOWNS
       ================================= */
    ViewBag.MealTypes = await _context.MealTypes.ToListAsync();
    ViewBag.DietFlags = await _context.DietaryFlags.ToListAsync();

    /* ================================
       RETURN SORTED LIST
       ================================= */
    var recipes = await query
        .OrderByDescending(r => r.CreatedAt)
        .ToListAsync();

    return View(recipes);
}



[HttpGet]
[AllowAnonymous]
public async Task<IActionResult> Search()
{
    ViewBag.MealTypes = await _context.MealTypes.Where(x => x.IsActive).ToListAsync();
    ViewBag.DietFlags = await _context.DietaryFlags.Where(x => x.IsActive).ToListAsync();
    return View();
}

// Like - ratings
[HttpPost]
public async Task<IActionResult> ToggleLike(int recipeId)
{
    int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
    if (userId == 0) return Unauthorized();

    var existing = await _context.RecipeLikes
        .FirstOrDefaultAsync(x => x.RecipeId == recipeId && x.UserId == userId);

    var recipe = await _context.Recipes.FindAsync(recipeId);

    if (existing != null)
    {
        _context.RecipeLikes.Remove(existing);
        recipe.TotalLikes -= 1;
    }
    else
    {
        _context.RecipeLikes.Add(new RecipeLike
        {
            RecipeId = recipeId,
            UserId = userId
        });

        recipe.TotalLikes += 1;
    }

    await _context.SaveChangesAsync();
    return RedirectToAction("Details", new { id = recipeId });
}
// comments - users(registered & creator)
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> AddComment(int recipeId, string commentText)
{
    int userId = HttpContext.Session.GetInt32("UserId") ?? 0;

    if (userId == 0)
        return RedirectToAction("Login", "Account");

    if (string.IsNullOrWhiteSpace(commentText))
        return RedirectToAction("Details", new { id = recipeId });

    var comment = new RecipeComment
    {
        RecipeId = recipeId,
        UserId = userId,
        CommentText = commentText.Trim(),
        CreatedAt = DateTime.UtcNow
    };

    _context.RecipeComments.Add(comment);
    await _context.SaveChangesAsync();

    return RedirectToAction("Details", new { id = recipeId });
}






    }
}
