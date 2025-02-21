using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FoodOrderingSystem.Models
{
    public class Food
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        // Navigation property for orders
        [JsonIgnore]
        public List<Order>? Orders { get; set; }
    }
}