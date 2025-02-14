using Microsoft.AspNetCore.Mvc;
using FoodOrderingSystem.DTOs;
using FoodOrderingSystem.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;


namespace FoodOrderingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrdersController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [Authorize] // Only authenticated users can create orders
        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder(CreateOrderDTO createOrderDTO)
        {
            try
            {
                // Get the current user's ID from the JWT token
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(userIdClaim, out int userId))
        {
            return Unauthorized(new { message = "Invalid or missing user ID" });
        }

                var order = await _orderService.CreateOrder(userId, createOrderDTO);
                return Ok(new 
                { 
                    message = "Order created successfully!", 
                    order = order 
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [Authorize] // Only authenticated users can access these endpoints
[HttpGet("my-orders")]
public async Task<IActionResult> GetOrdersForUser()
{
    try
    {
        // Get the current user's ID from the JWT token
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return Unauthorized(new { message = "User ID not found in token." });
        }

        var userId = int.Parse(userIdClaim.Value);

        var orders = await _orderService.GetOrdersForUser(userId);
        return Ok(new 
        { 
            message = "Orders retrieved successfully!", 
            orders = orders 
        });
    }
    catch (Exception ex)
    {
        return BadRequest(new { message = ex.Message });
    }
}

[Authorize(Roles = "Admin")] // Only admins can access this endpoint
[HttpGet("all")]
public async Task<IActionResult> GetAllOrders()
{
    try
    {
        var orders = await _orderService.GetAllOrders();
        return Ok(new 
        { 
            message = "All orders retrieved successfully!", 
            orders = orders 
        });
    }
    catch (Exception ex)
    {
        return BadRequest(new { message = ex.Message });
    }
}
[Authorize] // Only authenticated users can update orders
[HttpPut("{id}")]
public async Task<IActionResult> UpdateOrder(int id, UpdateOrderDTO updateOrderDTO)
{
    try
    {
         // Get the current user's ID from the JWT token
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userIdClaim == null)
                {
                    return Unauthorized(new { message = "User ID not found in token." });
                }
        // Get the current user's ID from the JWT token
        var userId = int.Parse(userIdClaim);

       

        var order = await _orderService.UpdateOrder(id, userId, updateOrderDTO);
        return Ok(new 
        { 
            message = "Order updated successfully!", 
            order = order 
        });
    }
    catch (Exception ex)
    {
        return BadRequest(new { message = ex.Message });
    }
}
[Authorize] // Only authenticated users can delete orders
[HttpDelete("{id}")]
public async Task<IActionResult> DeleteOrder(int id)
{
    try
    {
        // Get the current user's ID from the JWT token
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null)
        {
            return Unauthorized(new { message = "User ID not found in token." });
        }

        var userId = int.Parse(userIdClaim);

        var result = await _orderService.DeleteOrder(id, userId);
        if (result)
        {
            return Ok(new { message = "Order deleted successfully!" });
        }
        else
        {
            return BadRequest(new { message = "Failed to delete the order." });
        }
    }
    catch (Exception ex)
    {
        return BadRequest(new { message = ex.Message });
    }
}

    }
}