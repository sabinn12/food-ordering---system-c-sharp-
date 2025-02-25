namespace FoodOrderingSystem.DTOs
{
    public class UpdateFoodDTO
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public string? ImageUrl { get; set; } // Add this field
    }
}