namespace FoodOrderingSystem.DTOs
{
    public class LoginUserDTO
    {
        public string Email { get; set; } = string.Empty; // Initialize with default value
        public string Password { get; set; } = string.Empty; // Initialize with default value
        public string Role { get; set; } = "User"; // Initialize with default value
        
    }
}