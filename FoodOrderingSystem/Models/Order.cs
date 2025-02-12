namespace FoodOrderingSystem.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; } // Foreign key to User
        public int FoodId { get; set; } // Foreign key to Food
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public User? User { get; set; }
        public Food? Food { get; set; }
    }
}