using System.Security.Cryptography;
using System.Text;
using RecipeSharingApp.Data;
using RecipeSharingApp.Models;
using Microsoft.EntityFrameworkCore;

namespace RecipeSharingApp.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;

        public AuthService(AppDbContext context)
        {
            _context = context;
        }

        // SHA256 Hash Generator
        public string HashPassword(string password)
        {
            using SHA256 sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToHexString(hash);
        }

        // Validate Login
        public async Task<User?> ValidateUserAsync(string email, string password)
        {
            string hashed = HashPassword(password);

            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email && u.PasswordHash == hashed);
        }

        // Register new user
        public async Task<bool> RegisterUserAsync(User user)
        {
            // Check duplicate email
            bool exists = await _context.Users.AnyAsync(u => u.Email == user.Email);
            if (exists) return false;

            // Hash password
            user.PasswordHash = HashPassword(user.PasswordHash);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
