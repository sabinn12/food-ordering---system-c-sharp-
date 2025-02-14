namespace FoodOrderingSystem.DTOs
{
    public class UpdateOrderDTO
    {
        public int? FoodId { get; set; } // Optional: Update the food item
        public int? Quantity { get; set; } // Optional: Update the quantity
    }
}