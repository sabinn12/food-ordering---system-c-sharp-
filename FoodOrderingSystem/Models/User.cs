namespace FoodOrderingSystem.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // Initialize with default value
        public string Email { get; set; } = string.Empty; // Initialize with default value
        public string PasswordHash { get; set; } = string.Empty; // Initialize with default value
        public string Role { get; set; } = "User"; // Default role is "User"
    }
}