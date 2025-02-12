namespace FoodOrderingSystem.DTOs
{
    public class RegisterUserDTO
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public string Role { get; set; } = "User"; // Default role is "User"
        public string? AdminPassword { get; set; } // Optional field for admin registration
    }
}
