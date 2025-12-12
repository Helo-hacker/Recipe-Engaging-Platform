namespace RecipeSharingApp.Models
{
    public class UserProfile
    {
        public int Id { get; set; }   // Same as UserId

        public string? ProfileImage { get; set; }
        public string? Bio { get; set; }

        // Navigation back to User
        public User User { get; set; }
    }
}
