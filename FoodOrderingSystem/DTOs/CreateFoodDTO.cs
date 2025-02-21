
namespace FoodOrderingSystem.DTOs
{
    public class CreateFoodDTO
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public IFormFile? Image { get; set; } // Add this property for the image file
    }
}