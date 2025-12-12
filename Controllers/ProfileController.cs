using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeSharingApp.Data;
using RecipeSharingApp.Models;

public class ProfileController : Controller
{
    private readonly AppDbContext _context;

    public ProfileController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Profile
    public async Task<IActionResult> Index()
    {
        int userId = HttpContext.Session.GetInt32("UserId") ?? 0;

        if (userId == 0)
            return RedirectToAction("Login", "Account");

        var user = await _context.Users
            .Include(u => u.Profile)
            .FirstOrDefaultAsync(u => u.Id == userId);

        return View(user);
    }

    // POST: Profile Update
[HttpPost]
public async Task<IActionResult> Update(int id, string bio, IFormFile? profileImage)
{
    var profile = await _context.UserProfiles.FindAsync(id);

    // If no profile exists, create it
    if (profile == null)
    {
        profile = new UserProfile
        {
            Id = id, // same as UserId
            Bio = bio
        };
        _context.UserProfiles.Add(profile);
    }
    else
    {
        profile.Bio = bio;
    }

    if (profileImage != null)
    {
        string fileName = Guid.NewGuid() + Path.GetExtension(profileImage.FileName);
        string folder = Path.Combine("wwwroot/profile-images");
        Directory.CreateDirectory(folder);
        string path = Path.Combine(folder, fileName);

        using var stream = new FileStream(path, FileMode.Create);
        await profileImage.CopyToAsync(stream);

        profile.ProfileImage = "/profile-images/" + fileName;
    }

    await _context.SaveChangesAsync();
    return RedirectToAction("Index");
}
}
