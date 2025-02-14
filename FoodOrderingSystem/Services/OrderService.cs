using System;
using System.Threading.Tasks;
using FoodOrderingSystem.Models;
using FoodOrderingSystem.Data;
using FoodOrderingSystem.DTOs;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderingSystem.Services
{
    public class OrderService
    {
        private readonly AppDbContext _context;

        public OrderService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Order> CreateOrder(int userId, CreateOrderDTO createOrderDTO)
        {
            // Check if the food exists and is available
            var food = await _context.Foods.FindAsync(createOrderDTO.FoodId);
            if (food == null)
            {
                throw new Exception("Food not found.");
            }

            // Calculate total price
            decimal totalPrice = food.Price * createOrderDTO.Quantity;

            // Create the order
            var order = new Order
            {
                UserId = userId,
                FoodId = createOrderDTO.FoodId,
                Quantity = createOrderDTO.Quantity,
                TotalPrice = totalPrice,
                OrderDate = DateTime.UtcNow
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return order;
        }
 public async Task<List<Order>> GetOrdersForUser(int userId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.Food) // Include food details
                .ToListAsync();
        }

public async Task<List<Order>> GetAllOrders()
{
    return await _context.Orders
        .Include(o => o.Food) // Include food details
        .ToListAsync();
}
public async Task<Order> UpdateOrder(int orderId, int userId, UpdateOrderDTO updateOrderDTO)
{
    var order = await _context.Orders
        .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);

    if (order == null)
    {
        throw new Exception("Order not found or you do not have permission to update this order.");
    }

    if (updateOrderDTO.FoodId.HasValue)
    {
        var food = await _context.Foods.FindAsync(updateOrderDTO.FoodId.Value);
        if (food == null)
        {
            throw new Exception("Food not found.");
        }
        order.FoodId = updateOrderDTO.FoodId.Value;
    }

    if (updateOrderDTO.Quantity.HasValue)
    {
        order.Quantity = updateOrderDTO.Quantity.Value;
    }

    // Fix variable name to avoid redeclaration conflict
    var existingFood = await _context.Foods.FindAsync(order.FoodId);
    if (existingFood == null)
            {
                throw new Exception("Food not found.");
            }
    order.TotalPrice = existingFood.Price * order.Quantity;

    _context.Orders.Update(order);
    await _context.SaveChangesAsync();

    return order;
}
public async Task<bool> DeleteOrder(int orderId, int userId)
{
    var order = await _context.Orders
        .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);

    if (order == null)
    {
        throw new Exception("Order not found or you do not have permission to delete this order.");
    }

    _context.Orders.Remove(order);
    await _context.SaveChangesAsync();

    return true;
}

    }
}