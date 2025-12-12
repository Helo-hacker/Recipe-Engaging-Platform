using Microsoft.AspNetCore.Mvc;
using RecipeSharingApp.Models;
using RecipeSharingApp.Services;

namespace RecipeSharingApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly AuthService _auth;

        public AccountController(AuthService auth)
        {
            _auth = auth;
        }

        // Register Page
        public IActionResult Register()
        {
            return View();
        }

  [HttpPost]
public async Task<IActionResult> Register(string fullName, string email, string password, string role)
{
    if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
    {
        ViewBag.Error = "All fields are required.";
        return View();
    }

    var user = new User
    {
        FullName = fullName,
        Email = email,
        PasswordHash = password,
        Role = role
    };

    bool success = await _auth.RegisterUserAsync(user);

    if (!success)
    {
        ViewBag.Error = "Email already exists!";
        return View();
    }

    TempData["Success"] = "Registration successful!";
    return RedirectToAction("Login");
}



        // Login Page
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = await _auth.ValidateUserAsync(email, password);

            if (user == null)
            {
                ViewBag.Error = "Invalid credentials!";
                return View();
            }

            // Store session
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserRole", user.Role);
            HttpContext.Session.SetString("UserName", user.FullName);

            return RedirectToAction("Index", "Dashboard");
        }

        // Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
